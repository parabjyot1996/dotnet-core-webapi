using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.Elasticsearch;

namespace NetCoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => 
                {
                    configuration.Enrich.FromLogContext()
                        .WriteTo.Elasticsearch(
                            new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticSearchConfig:Uri"]))
                            {
                                IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}",
                                AutoRegisterTemplate = true,
                                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                                NumberOfShards = 2
                            })
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class CustomConverter : TraceTelemetryConverter
    {
        public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
        {
            // first create a default TraceTelemetry using the sink's default logic
            // .. but without the log level, and (rendered) message (template) included in the Properties
            foreach (ITelemetry telemetry in base.Convert(logEvent, formatProvider))
            {
                // then go ahead and post-process the telemetry's context to contain the user id as desired
                if (logEvent.Properties.ContainsKey("UserId"))
                {
                    telemetry.Context.User.Id = logEvent.Properties["UserId"].ToString();
                }
                // post-process the telemetry's context to contain the operation id
                if (logEvent.Properties.ContainsKey("operation_Id"))
                {
                    telemetry.Context.Operation.Id = logEvent.Properties["operation_Id"].ToString();
                }
                // post-process the telemetry's context to contain the operation parent id
                if (logEvent.Properties.ContainsKey("operation_parentId"))
                {
                    telemetry.Context.Operation.ParentId = logEvent.Properties["operation_parentId"].ToString();
                }
                // typecast to ISupportProperties so you can manipulate the properties as desired
                ISupportProperties propTelemetry = (ISupportProperties)telemetry;

                // find redundant properties
                var removeProps = new[] { "UserId", "operation_parentId", "operation_Id" };
                removeProps = removeProps.Where(prop => propTelemetry.Properties.ContainsKey(prop)).ToArray();

                foreach (var prop in removeProps)
                {
                    // remove redundant properties
                    propTelemetry.Properties.Remove(prop);
                }

                yield return telemetry;
            }
        }
    }
}
