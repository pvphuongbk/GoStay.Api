using GoStay.DataAccess.Base;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Hotel
{
    public partial class RoomAddDto
    {
        
        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public int[]? ViewRoom { get; set; }
        public int[]? ServiceRoom { get; set; }
        public int? Iduser { get; set; }

    }
    public partial class RoomEditDto
    {
        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string? ViewRoom { get; set; }
        public string? ServiceRoom { get; set; }
        public int? IdUser { get; set; }

    }
    public class RoomDto
    {
        public int Id { get; set; }
        public int Idhotel { get; set; }
        public int Iduser { get; set; }
        public string Name { get; set; }
        public string HotelName { get; set; }

        public decimal RoomSize { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int RoomStatus { get; set; }
        public decimal PriceValue { get; set; }
        public double Discount { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }
        public byte Palletbed { get; set; }
        public string PalletbedText { get; set; }
        public List<ViewRoomDto> ViewsRoom { get; set; }
        public List<ServiceRoomDto> ServicesRoom { get; set; }
        public List<PictureRoomDto> PicturesRoom { get; set; }


    }
    public class ViewRoomDto
    {
        public int Id { get; set; }
        public string? ViewDirection1 { get; set; }
    }
    public class PictureRoomDto
    {
        public int Id { get; set; }
        public string? UrlOut { get; set; }
    }
    public class ServiceRoomDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte? AdvantageLevel { get; set; }

    }
    public class HotelListUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
