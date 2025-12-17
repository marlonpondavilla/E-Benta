namespace E_Benta.Dtos
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string HashPassword{ get; set; }
        public bool isBentador { get; set; }

    }
}
