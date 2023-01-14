using ChessAppServer.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChessAppServer.Persistence;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
}