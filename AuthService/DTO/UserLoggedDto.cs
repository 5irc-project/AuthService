namespace AuthService.DTO
{
    public class UserLoggedDto
    {
        public int UserId { get; set; }

        public string? Nom { get; set; }

        public string Email { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
