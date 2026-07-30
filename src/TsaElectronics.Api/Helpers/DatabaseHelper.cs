using Microsoft.EntityFrameworkCore;
using TsaElectronics.Api.Data;

namespace TsaElectronics.Api.Helpers;

public static class DatabaseHelper
{
    // Applies any pending EF Core migrations against Azure SQL on startup.
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}
