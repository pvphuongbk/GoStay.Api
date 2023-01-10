using Microsoft.EntityFrameworkCore;
using GoStay.Common.Configuration;
using GoStay.DataAccess.DBContext;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Repositories;
using GoStay.DataAccess.UnitOfWork;
using Q101.ServiceCollectionExtensions.ServiceCollectionExtensions;
using GoStay.Services.Hotels;
using GoStay.Api.Configurations;
using GoStay.Common.Helpers.Order;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", optional: false)
	.Build();
AppConfigs.LoadAll(config);
//--register CommonDBContext
builder.Services.AddDbContext<CommonDBContext>(options =>
			options.UseSqlServer(AppConfigs.SqlConnection, options => { }),
			ServiceLifetime.Scoped
			);
builder.Services.AddTransient(typeof(ICommonRepository<>), typeof(CommonRepository<>));
builder.Services.AddTransient(typeof(ICommonUoW), typeof(CommonUoW));
builder.Services.AddScoped(typeof(IOrderFunction), typeof(OrderFunction));
//--register Service
builder.Services.RegisterAssemblyTypesByName(typeof(IHotelService).Assembly,
	 name => name.EndsWith("Service")) // Condition for name of type
	 .AsScoped()
	 .AsImplementedInterfaces()
	 .Bind();
builder.Services.AddCommonServices();

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoStay Api"));
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
