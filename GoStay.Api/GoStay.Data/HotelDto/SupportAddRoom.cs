﻿using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class SupportAddRoom
    {
        public List<ViewDirection> views { get; set; }
        public List<Service> servicesRoom { get; set; }
        public List<Palletbed> palletbed { get; set; }
    }
    public class UpdateStatusRoomParam
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
    }
    public class UpdateDiscountRoomParam
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int Discount { get; set; }
    }
}
