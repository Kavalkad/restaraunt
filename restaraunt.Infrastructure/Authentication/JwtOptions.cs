namespace restaraunt.Infrastructure
{
    public class JwtOptions
    {
        public string? SecretKey { get; set; }
        public int ExpiredHours { get; set; }
    }
}