using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Services.Hotels;
using GoStay.Services.Statisticals;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoStay.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class StatisticalsController : ControllerBase
	{
		private readonly IStatisticalService _statisticalService;
		public StatisticalsController(IStatisticalService statisticalService)
		{
            _statisticalService = statisticalService;
		}

        [HttpGet("hotel-chart")]
        public ResponseBase GetListHotelTopFlashSale()
        {
            var items = _statisticalService.GetValueChart();
            return items;
        }
    }
}