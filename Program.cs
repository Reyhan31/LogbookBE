using LogBookAPI.Authentication;
using LogBookAPI.Services.Commons;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRequiredServices(builder.Configuration, builder.Environment.EnvironmentName);

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("NUXT", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
// {
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("*"));

app.UseMiddleware<LogbookJwtMiddleware>();

app.UseHttpsRedirection();

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.Services.SubscribeEvent();

app.Run();
