namespace MahjongBuddy.Core.Enums
{
    public enum ExtraPoint
    {
        None,
        RedDragon,
        GreenDragon,
        WhiteDragon,
        SeatWind,
        PrevailingWind,

        //zi mo
        SelfPick,

        //mun cing
        ConcealedHand,

        NoFlower,
        RomanFlower,
        NumericFlower,
        AllFourFlowerSameType,

        //hoi dai lau jyut
        WinOnLastTile,

        //TODO: implement below

        //gong soeng hoi faa
        //KongAndFlowerAndWin

        //gong soeng gong
        //KongAndKongAndWin

        //dealer instant win when grabbing all 14 tiles
        //HeavenlyHand,

        //someone throw first tile and other player call win
        //EarthlyHand,
    }
}
