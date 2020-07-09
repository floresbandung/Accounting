using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dd.Tata.Buku.Auth.Migrations;
using Dd.Tata.Buku.Auth.Repositories;

namespace Dd.Tata.Buku.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<AuthContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("IdentityServerConnection")));

            services.AddFluentMigratorCore()
                .ConfigureRunner(ConRun)
                .AddLogging(l => l.AddFluentMigratorConsole());
        }

        public void ConRun(IMigrationRunnerBuilder builder)
        {
            builder.AddPostgres()
                .WithGlobalConnectionString(Configuration.GetConnectionString("IdentityServerConnection"))
                .ScanIn(typeof(InitMigration).Assembly)
                .For.Migrations();
        }

        private void Context(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(Configuration.GetConnectionString("IdentityServerConnection"), Opt);
        }

        private void Opt(Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.NpgsqlDbContextOptionsBuilder options)
        {
            options.MigrationsAssembly(GetType().Assembly.GetName().Name);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migration, AuthContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            migration.MigrateUp();
        }
    }
}
