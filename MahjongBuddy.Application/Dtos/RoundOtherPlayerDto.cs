using MahjongBuddy.Application.Enums;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundOtherPlayerDto : GamePlayerDto
    {
        public bool IsDealer { get; set; }

        public bool IsInitialDealer { get; set; }

        public bool IsMyTurn { get; set; }

        public bool MustThrow { get; set; }

        public WindDirection Wind { get; set; }

        //below props using custom resolver, check mappingprofile

        public int ActiveTilesCount { get; set; }

        public SeatOrientation SeatOrientation { get; set; }

        public ICollection<RoundTileDto> GraveyardTiles { get; set; }
    }
}
