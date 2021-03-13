using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using mySimpleMessageService.API.Filters;
using mySimpleMessageService.Common.Models;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using MediatR;
using mySimpleMessageService.API.Infrastructure;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;
using System.IO;
using mySimpleMessageService.Domain.Message.Queries;
using mySimpleMessageService.Domain.Message.Commands;
using mySimpleMessageService.Domain;
using mySimpleMessageService.Domain.Message.Validators;
using mySimpleMessageService.Domain.Contact.Commands;
using mySimpleMessageService.Domain.Validators;
using mySimpleMessageService.Persistance.Repositories;
using Persistance;
using mySimpleMessageService.API.Common;

namespace mySimpleMessageService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddTransient<MessagesRepository>();
            services.AddTransient<ContactsRepository>();
            services.AddTransient<IValidator<SendMessageCommand> ,MessageValidator>();
            services.AddTransient<IValidator<DeleteMessageCommand> ,MessageValidator>();
            services.AddTransient<IValidator<UpdateContactCommand> ,ContactValidator>();
            services.AddTransient<IValidator<DeleteContactCommand> ,ContactValidator>();

            services.AddDbContext<MessageServiceContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DataContext")).EnableSensitiveDataLogging());

            services.AddMediatR(new[] { typeof(ReadMessagesQuery).Assembly, typeof(ReadMessagesQueryHandler).Assembly });
            services.AddControllers(opt =>
            {
                opt.Filters.Add<HttpResponseExceptionFilter>();
            });

            services.AddOData();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", SwaggerUtils.GetSwaggerHeader());
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentation V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Count()
                    .Filter()
                    .OrderBy()
                    .Expand()
                    .Select()
                    .MaxTop(null);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
            });

           
        }
        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<MessageReadModel>("Messages");
            builder.EntitySet<ContactReadModel>("Contacts");
            return builder.GetEdmModel();
        }
    }
}
