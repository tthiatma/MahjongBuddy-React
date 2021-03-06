import { WindDirection } from "./windEnum";
import { IRoundTile } from "./tile";
import { HandType } from "./handTypeEnum";
import { ExtraPoint } from "./extraPointEnum";
import { IRoundOtherPlayer, IRoundPlayer } from "./player";
import { PlayResult } from "./playResultEnum";

export interface IRound {

  id: number;
  roundCounter: number;
  tileConter: number;
  remainingTiles: number;
  wind: WindDirection;
  dateCreated: Date;
  isEnding: boolean;
  isOver: boolean;
  isPaused: boolean;
  isTied: boolean;
  gameId: number;  
  roundResults: IRoundResult[] | null;
  boardTiles: IRoundTile[];
  mainPlayer: IRoundPlayer;
  otherPlayers: IRoundOtherPlayer[];
}

export interface IRoundResult {
  id: number;
  playResult: PlayResult;
  userName: string;
  displayName: string;
  roundResultHands: IRoundResultHand[];
  roundResultExtraPoints: IRoundResultExtraPoint[];
  playerTiles: IRoundTile[];
  points: number;
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
