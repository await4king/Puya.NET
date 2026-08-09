using Microsoft.AspNetCore.Builder;
using Puya.Api;
using Puya.Net.Samples.Config;

var builder = WebApplication.CreateBuilder(args);

StartupConfig.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.ConfigureMvc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDb();
builder.Services.ConfigureDebugging();
builder.Services.ConfigureLogging();
builder.Services.ConfigureApi();
builder.Services.ConfigureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapPuyaGateway("/api");
    endpoints.MapApiAndControllers();
});
//app.MapControllers();

app.Run();
