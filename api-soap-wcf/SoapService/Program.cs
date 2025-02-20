using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoapCore;
using System.ServiceModel;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços SOAP ao container
builder.Services.AddSoapCore();
builder.Services.AddSingleton<IService, Service>();
builder.Services.AddControllers(); // Para APIs REST, caso necessário

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Configurando o endpoint SOAP
app.UseSoapEndpoint<IService>("/Service.svc", new SoapEncoderOptions());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Para suporte a APIs REST
});

app.Run();
