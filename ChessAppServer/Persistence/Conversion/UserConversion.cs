using ChessApp.Api;

namespace ChessAppServer.Persistence.Conversion;

public class UserConversion
{
    public static User ToApi(Entities.User user)
    {
        return new User(user.Username, user.Rating);
    }
    
}