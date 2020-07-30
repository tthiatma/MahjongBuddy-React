using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MahjongBuddy.Core
{
    public class RoundPlayer
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        public int RoundId { get; set; }

        public virtual Round Round { get; set; }
        
        //the first player that throw the dice when the very game started
        public bool IsInitialDealer { get; set; }
        
        public bool IsDealer { get; set; }

        public bool HasAction { get; set; }

        public bool IsMyTurn { get; set; }

        public bool MustThrow { get; set; }

        public WindDirection Wind { get; set; }

        public int Points { get; set; }

        public virtual ICollection<RoundPlayerAction> RoundPlayerActions { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public RoundPlayer()
        {
            RoundPlayerActions = new List<RoundPlayerAction>();
        }
    }
}
