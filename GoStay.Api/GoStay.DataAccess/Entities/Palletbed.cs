

namespace GoStay.DataAccess.Entities
{ 
    public partial class Palletbed
    {
        public Palletbed()
        {
            HotelRooms = new HashSet<HotelRoom>();
        }

        public byte Id { get; set; }
        public string? Text { get; set; }
        public byte? Value { get; set; }

        public virtual ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
