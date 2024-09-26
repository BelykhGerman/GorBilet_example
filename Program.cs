using GorBilet_example.Data;
using GorBilet_example.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("RedisConnection");
if (connectionString != null)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
}
builder.Services.AddScoped<IData<Post>, PostData>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
