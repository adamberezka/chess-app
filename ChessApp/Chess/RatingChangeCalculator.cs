namespace ChessAppServer.Infrastructure;

public static class RatingChangeCalculator
{
    public static int ratingChange(int yourRating, int opponentsRating, bool? isWon)
    {
        int difference = opponentsRating - yourRating;
        int additionalPoints = difference / 25;

        if (isWon == null)
        {
            return 0 + additionalPoints;
        }
        else if (isWon == true)
        {
            return 16 + additionalPoints;
        }
        else
        {
            return -16 + additionalPoints;
        }
    }
}