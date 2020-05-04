namespace MahjongBuddy.Core
{
    //https://www.coololdgames.com/tile-games/mahjong/hong-kong/
    public enum HandType
    {
        None,

        Chicken,

        //ping wu
        Straight,

        //pong
        Triplets,

        //wan jat sik
        MixedOneSuit,

        //cing jat sik
        AllOneSuit,

        SevenPairs,

        //sap saam jiu
        ThirteenOrphans,

        //jiu gau
        //one nine one nine
        Orphans,

        //siu saam jyun
        SmallDragon,
        
        //daai saam jyun
        BigDragon,

        //siu sei hei
        SmallFourWind,

        //daai sei hei
        BigFourWind,

        AllKong,

        //Four pong and all concealed, and win by self pick
        HiddenTreasure,

        //zi jat sik
        AllHonors,

        //gau zi lin waan
        //one suit 1112345678999
        NineGates
    }
}