using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dd.Tata.Buku.Auth.Repositories;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using System;

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

            services.AddDbContext<AuthContext>(Context);

            services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AuthContext>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(ConfigurationStore)
                .AddOperationalStore(OperationalStore);
        }

        private void Context(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(Configuration.GetConnectionString("Default"), Options);
        }

        private void ConfigurationStore(ConfigurationStoreOptions options)
        {
            options.ConfigureDbContext = Context;
        }

        private void OperationalStore(OperationalStoreOptions options)
        {
            options.ConfigureDbContext = Context;
        }

        private void Options(Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.NpgsqlDbContextOptionsBuilder options)
        {
            options.MigrationsAssembly(GetType().Assembly.GetName().Name);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Initialize();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
