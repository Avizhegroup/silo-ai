using Microsoft.EntityFrameworkCore;
using SiloAI.Identity.Server.Utilities;

public static partial class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureAiApiServices(builder.Configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AiApiContext>();

            db.Database.Migrate();

            SeedDefaultAdminUser(db);
        }

        app.ConfigureAiApi();

        app.Run();
    }

    private static void SeedDefaultAdminUser(AiApiContext db)
    {
        if (!db.AiAdminUsers.Any())
        {
            db.AiAdminUsers.Add(new AiAdminUser
            {
                Username = "admin",
                PasswordHash = AiCryptoTools.GetHashedStringSha256("Admin@123"),
                Name = "System Administrator",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            db.SaveChanges();
        }
    }
}
