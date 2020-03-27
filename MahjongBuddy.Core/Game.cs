using System;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class Game
    {
        public int Id { get; set; }

        public virtual ICollection<GameTile> GameTiles { get; set; }

        public virtual ICollection<UserGame> UserGames { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        //public Player MainPlayer { get; set; }

        //public Player LeftPlayer { get; set; }

        //public Player TopPlayer { get; set; }

        //public Player RightPlayer { get; set; }

        //public WindDirection CurrentWind { get; set; }

        //public List<TileInGame> GraveyardTiles { get; set; }

        //public List<TileInGame> AliveTiles { get; set; }

        //public int TilesLeft { get; set; }

        //public TileInGame LastTile { get; set; }

        //public bool HaltMove { get; set; }

        //public string PlayerTurn { get; set; }
    }
}
