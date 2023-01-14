namespace ChessApp.Api;

public record CreateUserResponse (
    long Id,
    string Username,
    int Rating
    );