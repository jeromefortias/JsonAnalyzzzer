namespace JsonAnalyzzzer
{
    using Democrite.Framework.Builders;
    using Democrite.Framework.Core.Abstractions;
    using JsonAnalyzzzer.AgentCollector;
    using JsonAnalyzzzer.AgentInspector;
    using JsonAnalyzzzer.AgentStorage;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // builder pour le host de base



            // Ajout Swagger dépendence :  Swashbuckle.AspNetCore
            builder.Services.AddSwaggerGen(s => s.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo() {  Title ="Tuto Demo" , Version="v1"}   )).AddEndpointsApiExplorer();


            var jsonProcessSeq = Sequence.Build("JFO", fixUid: new Guid("D1F7C7EB-77F7-488A-91D7-77E4D5D16448"), metadataBuilder: m =>
                                         {
                                             m.Description("Load, analyze and store the json result")
                                              .AddTags("Json", "ES");
                                         })
                                         .RequiredInput<string>()
                                         .Use<IJsonLoaderVGrain>().Call((g, i, ctx) => g.LoadJsonAsync(i, ctx)).Return
                                         .Use<IJsonInspectorVGrain>().Configure("JFO").Call((g, i, ctx) => g.AnalyzeJsonAsync(i, ctx)).Return
                                         .Use<IJsonSaveVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.SaveJsonAsync(i, ctx)).Return
                                         .Build();

            builder.Host.UseDemocriteNode(b =>
            {
                b.WizardConfig()
                .NoCluster()
                .ConfigureLogging(c => c.AddConsole())
                .AddInMemoryDefinitionProvider(d => d.SetupSequences(jsonProcessSeq));

                b.ManualyAdvancedConfig().UseDashboard();
            }
                                                );
            var app = builder.Build();
            app.UseSwagger();
            
            app.MapPost("/root",async (string filePath, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                var result = await handler.VGrain<IJsonLoaderVGrain>()
                                       .SetInput(filePath)
                                       .Call((g, i, ctx) => g.LoadJsonAsync(i,ctx))
                                       .RunAsync();

                var content = result.SafeGetResult();
                return content;

                // return Task.CompletedTask;
            });


            app.MapPost("/root/seq", async (string filePath, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                var result = await handler.Sequence<string>(jsonProcessSeq.Uid)
                                       .SetInput(filePath)
                                       .RunAsync<string>();

                var content = result.SafeGetResult();
                return content;

                // return Task.CompletedTask;
            });


            app.MapPost("/counter", async (string json, string name, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                var result = await handler.VGrain<IJsonInspectorVGrain>()
                                       .SetInput(json)
                                       .SetConfiguration(name)
                                       .Call((g, i, ctx) => g.AnalyzeJsonAsync(i, ctx))
                                       .RunAsync();

                var content = result.SafeGetResult();
                return content;

                // return Task.CompletedTask;
            });




            app.MapGet("/counter", async ( string name, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                var result = await handler.VGrain<IJsonInspectorVGrain>()
                                       
                                       .SetConfiguration(name)
                                       .Call((g, ctx) => g.GetCounter( ctx))
                                       .RunAsync();

                var content = result.SafeGetResult();
                return content;

                // return Task.CompletedTask;
            });


            app.MapPost("/store", async (string json, string name, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                var result = await handler.VGrain<IJsonSaveVGrain>()
                                       .SetInput(json)
                                       .SetConfiguration(name)
                                       .Call((g, i, ctx) => g.SaveJsonAsync(i, ctx))
                                       .RunAsync();

                var content = result.SafeGetResult();
                return content;

                // return Task.CompletedTask;
            });


            app.MapGet("/", (request) =>
            {
                request.Response.Redirect("swagger");
                return Task.CompletedTask;
            });
            app.UseSwaggerUI();
           
            app.Run(); 

        }
    }
}
