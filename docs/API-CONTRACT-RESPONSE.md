# API Contract — Backend Response

Response to the frontend's `ARCHITECTURE.md` and `api/openapi.yaml` draft. Goes
through each of the 10 shared-contract items, plus one correction to the
`Product` schema that the draft didn't account for.

A reconciled `openapi.yaml` reflecting all of this is at
[`./openapi.yaml`](./openapi.yaml) in this same folder.

## 1. Auth — different from your draft, already decided

Not a cookie set by this API. `POST /api/auth/login` and `POST
/api/auth/register` return the JWT as a **plain JSON body**, not a cookie.

Reasoning: this API and the Next.js app will live on different domains
(separate repos/deploys). A cookie set directly by the API's domain won't
reliably reach the Next.js domain on subsequent requests without
`SameSite=None; Secure`, which runs into third-party-cookie blocking in
Safari/Chrome.

What we need from you: Auth.js's Credentials provider calls our
`/api/auth/login` endpoint **server-side** (not from the browser), gets the
JWT back in the response body, and stores it inside Auth.js's own encrypted
session token. Auth.js sets its cookie on the Next.js domain (first-party, no
cross-site issues). Your server-side code (Route Handlers / Server Actions /
Server Components) then pulls the JWT out of the Auth.js session and attaches
it as `Authorization: Bearer <token>` when calling our API. The browser never
sees the raw JWT — only Auth.js's own session cookie.

## 2. OpenAPI/Swagger — agreed, with one change

Agreed that OpenAPI is the source of truth. But rather than a hand-maintained
YAML file, our API auto-generates a live OpenAPI spec from the actual
controllers (Swashbuckle) — it's always in sync with what's really deployed,
which a hand-written file can't guarantee.

`docs/openapi.yaml` in this repo is a **snapshot** to unblock you now, before
more endpoints exist. Once we have a shared dev/staging environment, switch
your type generation to pull from the live spec (`/swagger/v1/swagger.json`)
instead of a static file, so it can't drift from reality.

## 3. Errors — agreed

`ProblemDetails` (RFC 7807) for all error responses, matching your draft
exactly. Not wired up yet on our side — it's on the list before we ship any
endpoint you'll be handling errors for in the UI (checkout first).

## 4. Pagination — agreed

`page` / `pageSize` / `total`, matching your draft. `GET /api/products`
currently returns an unpaged list (fine for the handful of seed products
we're testing with) — pagination goes in before that matters. Keeping your
default `pageSize=20` unless you want something else.

## 5. Money — agreed, adopting integer minor units

Internally we store price as SQL `decimal(18,2)` (exact, no floating-point
issues at the DB layer), but the API will convert to your `{ amountMinor,
currency }` shape at the response boundary, so the wire format matches your
contract exactly. `currency` will always be `"USD"` for now — single
currency/locale was a deliberate scope decision for this project, not an
oversight.

## 6. IDs & slugs — agreed, already matches

`Product.id` is a GUID, `Product.slug` is a unique indexed slug. One addition
you'll need — see §11 below on variants: variant ids are also GUIDs, since
cart items reference a specific variant, not just a product.

## 7. JSON casing — no action needed

Good news: nothing to configure. ASP.NET Core's default `System.Text.Json`
serialization for Web API controllers already uses `camelCase` out of the
box. (The "PascalCase by default" note in your doc applies to
`Newtonsoft.Json` or older ASP.NET MVC — not modern ASP.NET Core Web API,
which is what we're using.)

## 8. Search — pushing back on Algolia

Not adopting Algolia. We'd already scoped search/filtering as backend-driven
SQL Server queries (indexed columns), specifically to avoid a third-party
dependency and a SQL → Algolia sync pipeline for a project this size.

The good news: **your own OpenAPI draft already specifies exactly the right
shape for this** — `q`, `brand`, `minPrice`, `maxPrice`, `page`, `pageSize` as
query params on `GET /products`. Those map directly onto our SQL queries. So
there's no rework on your side beyond calling our REST endpoint with those
params instead of `react-instantsearch`/Algolia's client SDK. Happy to
revisit if search relevance becomes a real pain point later, but it's not a
day-one need.

## 9. Stripe — agreed

Backend creates the `PaymentIntent` and handles webhooks. Not built yet
(queued after the Cart and Orders slices) — will share the checkout endpoint
contract once it exists. Use Stripe's `automatic_payment_methods` on the
intent and let Payment Element handle SCA/3-D Secure client-side; no special
backend work needed beyond creating the intent correctly.

## 10. Image URLs — agreed, one open question for you

Backend returns ready-to-use CDN URLs (Azure Blob Storage + CDN), matching
your draft. Open question we need your input on: do you need multiple sized
variants served from the backend (thumbnail/medium/full), or is one canonical
size fine with `next/image` handling resizing client-side? Recommend the
latter for now unless you have a specific reason to need pre-sized variants —
let us know.

## 11. Correction: products have variants, not a single price/image

This is the one place your draft schema doesn't match the real domain.
Electronics have real variants — a phone in different colors/storage sizes, a
laptop in different RAM/SSD configs — each with its **own SKU, price, and
stock**. A single `price` + `imageUrl` per product can't represent "iPhone
15, 256GB, Space Black — $999" vs. "iPhone 15, 512GB, Blue — $1099" as
distinct purchasable things.

Corrected shape (see `docs/openapi.yaml` for the full schema):

```jsonc
{
  "id": "guid",
  "slug": "iphone-15",
  "name": "iPhone 15",
  "brand": "Apple",
  "description": "...",
  "imageUrls": ["https://cdn.../1.jpg", "https://cdn.../2.jpg"],
  "variants": [
    {
      "id": "guid",
      "sku": "IPH15-256-BLK",
      "price": { "amountMinor": 99900, "currency": "USD" },
      "stockQuantity": 12,
      "color": "Space Black",
      "storage": "256GB",
      "ram": null
    }
  ]
}
```

Practical implications for the frontend:
- Product detail page needs a variant selector (color/storage/RAM), not just
  an "Add to Cart" button — price and stock depend on which variant is
  selected.
- Cart items reference a `variantId`, not just a `productId`.
- The list/catalog endpoint (`GET /products`) will show a representative
  price (e.g. the lowest variant price, or "from $X") since a product itself
  no longer has one canonical price.

Let us know if this changes your catalog/PDP component design meaningfully —
happy to hop on a call to walk through it if useful.
