﻿using GoStay.Api.Attributes;
using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Statistical;
using GoStay.Services.Hotels;
using GoStay.Services.Statisticals;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace GoStay.Api.Controllers
{
	[ApiController]
    [Authorize]
    [Route("[controller]")]
	public class StatisticalsController : ControllerBase
	{
		private readonly IStatisticalService _statisticalService;
		public StatisticalsController(IStatisticalService statisticalService)
		{
            _statisticalService = statisticalService;
		}

        [HttpGet("hotel-chart")]
        public ResponseBase GetValueChart()
        {
            var items = _statisticalService.GetValueChart();
            return items;
        }

        [HttpGet("room-by-day")]
        public ResponseBase GetRoomInMonthByDay(int month, int year)
        {
            var items = _statisticalService.GetRoomInMonthByDay(month, year);
            return items;
        }
        [HttpPost("price-by-user")]
        public Task<ResponseBase> GetAllOrderPriceByUser(PriceDetailByUserRequest request)
        {
            var items = _statisticalService.GetAllOrderPriceByUser(request);
            return items;
        }
        [HttpGet("price-char-by-user")]
        public Task<ResponseBase> GetPriceChartByUser(PriceChartType type, int userID, int year, int month)
        {
            var items = _statisticalService.GetPriceChartByUser(type, userID, year, month);
            return items;
        }
        [HttpGet("order-by-user")]
        public Task<ResponseBase> GetAllOrderByUser(int userID, int pageIndex, int pageSize, int style)
        {
            var items = _statisticalService.GetAllOrderByUser(userID, pageIndex, pageSize, style);
            return items;
        }
    }
}