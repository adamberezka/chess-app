namespace ChessApp.Api;

public record CreateUserRequest (
    string Username,
    string Password,
    int Rating
    );