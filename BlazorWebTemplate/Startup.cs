using BlazorWebTemplate.Data;
using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebTemplate
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllers( );

            services.AddRazorPages( );
            services.AddTelerikBlazor( );
            services.AddServerSideBlazor( );

            services.AddSingleton<SessionService>( );
            services.AddSingleton<ICustomAuthService, CustomAuthService>( );

            services.AddSingleton<NLog.ILogger>( serviceProvider =>
            {
                return LogManager.GetCurrentClassLogger( );
            } );

            services.AddHttpContextAccessor( );
            services.AddAuthorization( );
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>( );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment( ) )
            {
                app.UseDeveloperExceptionPage( );
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts( );
            }

            app.UseHttpsRedirection( );
            app.UseStaticFiles( );

            app.UseRouting( );

            app.UseAuthentication( );
            app.UseAuthorization( );

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers( );
                endpoints.MapBlazorHub( );
                endpoints.MapFallbackToPage( "/_Host" );
            } );
        }
    }
}
