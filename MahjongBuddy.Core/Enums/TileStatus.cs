namespace MahjongBuddy.Core
{
    public enum TileStatus
    {
        //Tile belongs to the board
        Unrevealed = 0,
        //Tile just picked from the board
        JustPicked = 1,
        //Tile that is on player's hand
        UserActive = 2,
        //Tile is kept by player
        UserGraveyard = 3,
        //Tile is open and thrown to the board
        BoardGraveyard = 4
    }
}
