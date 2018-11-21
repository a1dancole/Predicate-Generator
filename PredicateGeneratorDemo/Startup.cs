﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PredicateGenerator.Generator;
using PredicateGeneratorDemo.Data;
using PredicateGeneratorDemo.Framework;
using PredicateGeneratorDemo.Framework.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace PredicateGeneratorDemo
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info {Title = "Predicate Generator Demo", Version = "v1"}));

            services.AddScoped<IGameQueryer, GameQueryer>();
            services.AddScoped<IDataStore, DataStore>();
            services.AddScoped(typeof(IPredicateGenerator<>), typeof(PredicateGenerator<>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Predicate Generator Demo"));
        }
    }
}
