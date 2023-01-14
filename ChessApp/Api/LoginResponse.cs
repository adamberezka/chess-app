namespace ChessApp.Api;

public record LoginResponse(
    long Id,
    string Username,
    int Rating
    );