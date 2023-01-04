using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ChessAppServer.Persistence.Entities;

public class User
{
    [Key]
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Rating { get; set; }

    public User()
    {
    }

    public User(long id, string username, string password, int rating)
    {
        Id = id;
        Username = username;
        Password = password;
        Rating = rating;
    }

    public User(string username, string password, int rating)
    {
        Username = username;
        Password = password;
        Rating = rating;
    }
}