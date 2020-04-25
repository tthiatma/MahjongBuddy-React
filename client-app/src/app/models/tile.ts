import { TileStatus } from "./tileStatus";

export interface IRoundTile{
    id: string;
    boardGraveyardCounter: number;
    activeTileCounter: number;
    owner: string;
    roundId: number;
    isWinner: boolean;
    tileSetGroup: TileSetGroup;
    status: TileStatus;
    tile: ITile;
}

export interface ITile {
    id: number;
    title: string;
    tileType: TileType;
    tileValue:  TileValue;
    image: string;
    imageSmall: string;
}

export enum TileValue
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    DragonRed = 10,
    DragonGreen = 11,
    DragonWhite = 12,
    WindNorth = 13,
    WindSouth = 14,
    WindEast = 15,
    WindWest = 16,
    FlowerRomanOne = 17,
    FlowerRomanTwo = 18,
    FlowerRomanThree = 19,
    FlowerRomanFour = 20,
    FlowerNumericOne = 21,
    FlowerNumericTwo = 22,
    FlowerNumericThree = 23,
    FlowerNumericFour = 24
}

enum TileType{
    Circle,
    Money,
    Stick,
    Wind,
    Dragon,
    Flower
}

enum TileSetGroup {
    None,
    Chow,
    Pong,
    Kong,
    Eye
}