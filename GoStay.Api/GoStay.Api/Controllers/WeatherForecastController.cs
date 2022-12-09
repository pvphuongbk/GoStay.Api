using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Services.Hotels;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly ICommonRepository<Hotel> _hotelRepository;
		private readonly IHotelService _hotelService;
		public WeatherForecastController(ILogger<WeatherForecastController> logger, ICommonRepository<Hotel> hotelRepository, IHotelService hotelService)
		{
			_logger = logger;
			_hotelRepository = hotelRepository;
			_hotelService = hotelService;
		}


	}
}