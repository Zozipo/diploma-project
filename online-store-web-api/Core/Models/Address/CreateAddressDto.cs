namespace Core.Models.Address
{
    public class CreateAddressDto
    {
        public string Street { get; set; }
        public int HomeNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public int FlatNumber { get; set; }
        public int ZipCode { get; set; }
        public bool IsThereElevator { get; set; }
        public string? UserId { get; set; }
    }
}
