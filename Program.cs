using CrudApi.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TokenAuthenticationHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddDbContext<PersonContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")))
	.AddDbContext<UserContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDb")));
builder.Services.ConfigureTokenServices(Encoding.ASCII.GetBytes(builder.Configuration["TokenKey"]));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
