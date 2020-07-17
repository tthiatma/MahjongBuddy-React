import { WindDirection } from "./windEnum";
import { IRoundTile } from "./tile";
import { HandType } from "./handTypeEnum";
import { ExtraPoint } from "./extraPointEnum";

export interface IRoundSimple {
  id: number;
  roundCounter: number;
  tileConter: number;
  wind: WindDirection;
  dateCreated: Date;
  isHalted: boolean;
  isEnding:boolean;
  isOver: boolean;
  isPaused: boolean;
  isTied: boolean;
  isWinnerSelfPicked: boolean;
  gameId: number;
}
export interface IRound extends IRoundSimple {
  roundTiles: IRoundTile[];
  updatedRoundTiles?: IRoundTile[];
  updatedRoundPlayers?: IRoundPlayer[];
  roundPlayers: IRoundPlayer[];
  roundResults: IRoundResult[];
}

export interface IRoundResult {
  id: number;
  isWinner: boolean;
  userName: string;
  roundResultHands: IRoundResultHand[];
  roundResultExtraPoints: IRoundResultExtraPoint[];
  pointsResult: number;
}

export interface IRoundResultHand {
    id: number;
    name: string;
    handType: HandType;
    point: number;
}

export interface IRoundResultExtraPoint {
    id: number;
    name: string;
    extraPoint: ExtraPoint;
    point: number;
}

export interface IRoundPlayer {
  userName: string;
  displayName: string;
  image: string;
  isInitialDealer: boolean;
  isDealer: boolean;
  isMyTurn: boolean;
  canDoNoFlower: boolean;
  wind: WindDirection;
  points: number;
}
