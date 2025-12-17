namespace E_Benta.Dtos
{
    public class CreateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isBentador { get; set; }

    }
}
