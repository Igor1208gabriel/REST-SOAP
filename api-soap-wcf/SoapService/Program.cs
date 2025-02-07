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
builder.Services.AddControllers(); // Se houver APIs REST

var app = builder.Build();

// Configurar pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseRouting();

// 🔹 UseSoapEndpoint deve ser chamado diretamente no app
app.UseSoapEndpoint<IService>("/Service.svc", new SoapEncoderOptions());

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers(); // Para suporte a APIs REST
});

app.Run();
