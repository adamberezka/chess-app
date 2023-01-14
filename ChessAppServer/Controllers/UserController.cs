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
    public ActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        _logger.Log(LogLevel.Information, "Adding User: {1}", request);

        var user = _context.User
            .FirstOrDefault(u => u.Username.Equals(request.Username));

        if (user != null)
            return new StatusCodeResult(409);

        var entityEntry =
            _context.User.Add(new Persistence.Entities.User(request.Username, request.Password, request.Rating));
        _context.SaveChanges();

        return new JsonResult(new CreateUserResponse(entityEntry.Entity.Id,
            entityEntry.Entity.Username,
            entityEntry.Entity.Rating));
    }

    [HttpPost]
    [Route("/login")]
    public ActionResult LoginUser([FromBody] LoginRequest request)
    {
        var userToLogin = _context.User
            .FirstOrDefault(user => user.Username.Equals(request.Username));
        
        if (userToLogin!= null && userToLogin.Password.Equals(request.Password))
        {
            return new JsonResult(
                new LoginResponse(userToLogin.Id,
                    userToLogin.Username,
                    userToLogin.Rating));
        }

        return new StatusCodeResult(401);
    }
}