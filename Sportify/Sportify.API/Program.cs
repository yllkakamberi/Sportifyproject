using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sportify
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DbContext with SQL Server and specify migrations assembly
            builder.Services.AddDbContext<SportifyDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("SportifyDb"),
                    b => b.MigrationsAssembly("Infrastructure") // Set migrations assembly
                )
            );

            // Register repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Generic repository
            builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Product repository
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>(); // Review repository

            var app = builder.Build();

            // Get the logger from the DI container
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Apply database migrations in development
                await EnsureDatabaseIsMigrated(app.Services, logger);
            }

            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }

        /// <summary>
        /// Ensures the database is migrated to the latest version.
        /// </summary>
        /// <param name="services">The service provider.</param>
        /// <param name="logger">The logger instance.</param>
        /// <exception cref="Exception">Throws if migration fails.</exception>
        private static async Task EnsureDatabaseIsMigrated(IServiceProvider services, ILogger<Program> logger)
        {
            try
            {
                using var scope = services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SportifyDbContext>();

                if (dbContext is not null)
                {
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Database migration completed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database migration.");
                throw;
            }
        }
    }
}