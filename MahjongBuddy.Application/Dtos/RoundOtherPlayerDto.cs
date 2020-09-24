using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundOtherPlayerDto
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public bool IsDealer { get; set; }

        public bool IsInitialDealer { get; set; }

        public bool IsMyTurn { get; set; }

        public bool MustThrow { get; set; }

        public WindDirection Wind { get; set; }

        public int Points { get; set; }

        public bool HasAction { get; set; }

        //below props using custom resolver, check mappingprofile
        public int ActiveTilesCount { get; set; }

        public ICollection<RoundTileDto> GraveyardTiles { get; set; }
    }
}
