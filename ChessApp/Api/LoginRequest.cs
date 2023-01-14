namespace ChessApp.Api;

public record LoginRequest(
    string Username,
    string Password
    );