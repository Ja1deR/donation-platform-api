namespace Software_2.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
