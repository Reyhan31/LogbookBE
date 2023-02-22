namespace LogBookAPI.Models.Authentication
{
    public class AuthenticationResponse : User
    {
        public string Token {get; set;}

        public AuthenticationResponse(User user, string token) : base(user){
            Token = token;
        }
    }
}