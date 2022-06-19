using Services.DTOs.Users;

namespace Services.DTOs.Sessions
{
    public class SessionDto
    {
        public JwtDto Jwt { get; set; }

        public UserDto UserAuthorized { get; set; }

        public SessionDto()
        {
        }

        public SessionDto(JwtDto jwt, UserDto userAuthorized)
        {
            Jwt = jwt;
            UserAuthorized = userAuthorized;
        }
    }
}