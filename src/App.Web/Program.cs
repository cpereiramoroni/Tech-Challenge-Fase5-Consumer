using App.Infra.CrossCutting.IoC;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
// Needed for minimal APIs

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo

        {
            Title = "Lambda FIAP05 Videos -- https://docs.aws.amazon.com/pt_br/lambda/latest/dg/csharp-package-asp.html",
            Version = "v1",
            Description = "API for FIAP Lambda Videos Fase 05- Consumer de Videos.",
            Contact = new OpenApiContact
            {
                Name = "G24",  // Replace with your name or team
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",  // Replace with your desired license
                Url = new Uri("https://opensource.org/licenses/MIT")  // Link to the license
            }
        });

    c.EnableAnnotations();
});

//IOC
NativeInjectorBootStrapper.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string swaggerJsonbasePatch = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonbasePatch}/swagger/v1/swagger.json", "Lambda V1");
    c.OAuthAppName("FIAP Lambda auth Fase 05");
    c.OAuthScopeSeparator(" ");
    c.OAuthUsePkce();
});


app.UseHttpsRedirection();
app.UseAuthorization();
//app.MapControllers();

app.Run();
