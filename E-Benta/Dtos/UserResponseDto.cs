namespace E_Benta.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool isBentador { get; set; } = false;
    }
}
