using CmsConfeitaria.Business;
using CmsConfeitaria.Business.AutoMapperProfile;
using CmsConfeitaria.Core.Entity;
using CmsConfeitaria.Integration;
using CmsConfeitaria.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ReceitaIngredienteProfile>();
    cfg.AddProfile<ReceitaProfile>();
    cfg.AddProfile<IngredienteProfile>();
    cfg.AddProfile<UnidadeMedidaProfile>();
    cfg.AddProfile<ComrpaProfile>();
}) ;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder =>
        {
            builder.SetIsOriginAllowed((url) =>
            {
                return true;
            })
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<DBContextCm>(options =>
                options.UseSqlServer(
                    (builder.Configuration.GetConnectionString("cms")),
                    x => x.MigrationsAssembly("CmsConfeitaria.Integration")
                ), ServiceLifetime.Scoped);


builder.Services.Configurar();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowMyOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
