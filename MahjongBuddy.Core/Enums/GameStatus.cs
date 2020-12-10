namespace MahjongBuddy.Core.Enums
{
    public enum GameStatus
    {
        Created,
        Playing,
        //when there is a round not over, but host forcing to end the game...host powaaaa
        OverPrematurely,
        Over,
    }
}
