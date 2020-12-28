import { ActionType } from "./actionTypeEnum";
import { SeatOrientation } from "./seatOrientationEnum";
import { IRoundTile } from "./tile";
import { WindDirection } from "./windEnum";

export interface IGamePlayer {
  userName: string;
  displayName: string;
  points: number;
  initialSeatWind: WindDirection | undefined;
  image: string;
  isHost: boolean;
}

export interface IRoundPlayer extends IGamePlayer {
  isDealer: boolean;
  isInitialDealer: boolean;
  isMyTurn: boolean;
  mustThrow: boolean;
  wind: WindDirection;
  points: number;
  isManualSort: boolean;
  roundPlayerActiveActions: IRoundPlayerAction[];
  playerTiles: IRoundTile[];
}

export interface IRoundOtherPlayer extends IGamePlayer {
  isDealer: boolean;
  isInitialDealer: boolean;
  isMyTurn: boolean;
  mustThrow: boolean;
  wind: WindDirection;
  points: number;
  activeTilesCount: number;
  seatOrientation: SeatOrientation;
  graveyardTiles: IRoundTile[];
}

export interface IRoundPlayerAction {
  id: number;
  actionType: ActionType;
}