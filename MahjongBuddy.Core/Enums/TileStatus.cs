namespace MahjongBuddy.Core
{
    public enum TileStatus
    {
        //Tile belongs to the board
        Unrevealed = 0,
        //Tile just picked from the board
        UserJustPicked = 1,
        //Tile that is on player's hand
        UserActive = 2,
        //Tile is kept by player
        UserGraveyard = 3,
        //Tile is alive on board
        BoardActive = 4,
        //Tile is dead on board
        BoardGraveyard = 5
    }
}
