namespace E_Benta.Dtos
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public bool isBentador { get; set; }
    }
}
