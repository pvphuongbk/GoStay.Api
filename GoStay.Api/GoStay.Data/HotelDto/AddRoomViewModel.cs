using GoStay.DataDto.Hotel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PartnerGostay.Models
{
    public class AddRoomViewModel
    {

        public int IdHotel { get; set; }
        public string Name { get; set; }
        public int RoomSize { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public decimal PriceValue { get; set; }
        public double Discount { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }

        public List<int> ViewRoom { get; set; }
        public byte? Palletbed { get; set; }
        public List<int> ServicesRooms { get; set; }

        public string? NameAlbum { get; set; }
        public List<IFormFile>? filesRoom { get; set; }
        public List<IFormFile>? filesAlbum { get; set; }
        public int UserId { get; set; }
    }
    public class AddRoomModel
    {
        public int IdHotel { get; set; }
        public string Name { get; set; }
        public int RoomSize { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public decimal PriceValue { get; set; }
        public double Discount { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }

        public List<int> ViewRoom { get; set; }
        public byte? Palletbed { get; set; }
        public List<int> ServicesRooms { get; set; }
        public List<PictureRoomDto> PicturesRoom { get; set; }

        public List<int> PicturesRoomRemove { get; set; }
        public int Iduser { get; set; }
    }
    public class EditRoomModel
    {
        public int Id { get; set; }
        public int IdHotel { get; set; }
        public string Name { get; set; }
        public int RoomSize { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public decimal PriceValue { get; set; }
        public double Discount { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }

        public List<int> ViewRoom { get; set; }
        public byte? Palletbed { get; set; }
        public List<int> ServicesRooms { get; set; }
        public List<PictureRoomDto> PicturesRoom { get; set; }

        public List<int> PicturesRoomRemove { get; set; }
        public int Iduser { get; set; }
    }
}
