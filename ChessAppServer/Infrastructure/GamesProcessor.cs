using ChessAppServer.Persistence;

namespace ChessAppServer.Infrastructure;

public class GamesProcessor
{
    private readonly Random _random = new Random();
    
    private readonly List<Player> _waitingPlayers = new List<Player>();
    private readonly List<GameInProgress> _gamesInProgresses = new List<GameInProgress>();

    public void AddPlayer(Player player, DataContext dataContext)
    {
        if (_waitingPlayers.Count > 0)
        {
            Player secondPlayer = _waitingPlayers.FirstOrDefault()!;
            _waitingPlayers.Remove(secondPlayer);

            var randInt = _random.NextInt64(0, 100);

            Player whitePlayer = randInt > 50 ? player : secondPlayer;
            Player blackPlayer = whitePlayer == player ? secondPlayer : player;

            GameInProgress gameInProgress = new GameInProgress(blackPlayer, whitePlayer, dataContext);
            _gamesInProgresses.Add(gameInProgress);

            Task.Run(() => gameInProgress.Start());
        }
        else
        {
            _waitingPlayers.Add(player);
        }
    }
}