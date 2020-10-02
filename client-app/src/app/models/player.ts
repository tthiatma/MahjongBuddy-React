import { ActionType } from "./actionTypeEnum";
import { SeatOrientation } from "./seatOrientationEnum";
import { IRoundTile } from "./tile";
import { WindDirection } from "./windEnum";

export interface IPlayer {
  userName: string;
  displayName: string;
  initialSeatWind: WindDirection | undefined;
  image: string;
  isHost: boolean;
}

export interface IRoundPlayer extends IPlayer {
  isDealer: boolean;
  isInitialDealer: boolean;
  isMyTurn: boolean;
  mustThrow: boolean;
  wind: WindDirection;
  points: number;
  isManualSort: boolean;
  roundPlayerActions: IRoundPlayerAction[];
  playerTiles: IRoundTile[];
}

export interface IRoundOtherPlayer extends IPlayer {
  isDealer: boolean;
  isInitialDealer: boolean;
  isMyTurn: boolean;
  mustThrow: boolean;
  wind: WindDirection;
  points: number;
  hasAction: boolean;
  activeTilesCount: number;
  seatOrientation: SeatOrientation;
  graveyardTiles: IRoundTile[];
}

export interface IRoundPlayerAction {
  id: number;
  playerAction: ActionType;
}