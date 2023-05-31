﻿using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.Services.Statisticals;
using GoStay.Services.Tours;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerRoomPriceService _schedulerService;
        public SchedulerController(ISchedulerRoomPriceService schedulerService)
        {
            _schedulerService = schedulerService;
        }
        
        [HttpPost("insert")]
        public ResponseBase Insert(SchedulerRoomPrice scheduler)
        {
            var items = _schedulerService.Insert(scheduler);
            return items;
        }
        [HttpPost("update")]
        public ResponseBase Update(SchedulerRoomPrice scheduler)
        {
            var items = _schedulerService.Update(scheduler);
            return items;
        }
        [HttpGet("get")]
        public ResponseBase GetScheduler(int Id)
        {
            var items = _schedulerService.GetScheduler(Id);
            return items;
        }
        [HttpGet("get-list")]
        public ResponseBase GetListScheduler(int month, int year, int RoomId)
        {
            var items = _schedulerService.GetListScheduler(month, year, RoomId);
            return items;
        }
        [HttpDelete("destroy")]
        public ResponseBase Destroy(int Id)
        {
            var items = _schedulerService.Destroy(Id);
            return items;
        }
    }
}
