namespace AuthService.DTO
{
    public class UserLoggedDto
    {
        public int UserId { get; set; }

        public string Nom { get; set; }

        public string Email { get; set; }

        public string ProfilePictureUrl { get; set; }

        public UserLoggedDto(int userId, string nom, string email, string profilePictureUrl)
        {
            UserId = userId;
            Nom = nom;
            Email = email;
            ProfilePictureUrl = profilePictureUrl;
        }
    }
}
