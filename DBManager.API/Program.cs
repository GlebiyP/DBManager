using DBManager.API.Hateoas.LinkGetters;
using DBManager.LocalDB;
using DBManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDBContext, LocalDBContext>();
builder.Services.AddScoped<IDBManagerService, DBManagerService>();
builder.Services.AddScoped<DbLinkGetter>();
builder.Services.AddScoped<TableLinkGetter>();
builder.Services.AddScoped<ColumnLinkGetter>();
builder.Services.AddScoped<RowLinkGetter>();

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