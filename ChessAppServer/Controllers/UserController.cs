using ChessApp.Api;
using ChessAppServer.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessAppServer.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public User CreateUser(string username)
    {
        _logger.Log(LogLevel.Information, "Adding User: {1}", username);
        
        _context.User.Add(new Persistence.Entities.User(username, "password", 1002));
        _context.SaveChanges();
        
        return new User(username, 1002);
    }
}