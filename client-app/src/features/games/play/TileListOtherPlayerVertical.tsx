import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { TileStatus } from "../../../app/models/tileStatus";
import { IRoundPlayer } from "../../../app/models/round";
import { Grid } from "semantic-ui-react";
import PlayerStatus from "./PlayerStatus";

interface IProps {
  roundTiles: IRoundTile[];
  player: IRoundPlayer;
  isReversed: boolean;
}

const TileListOtherPlayerVertical: React.FC<IProps> = ({
  roundTiles,
  player,
  isReversed
}) => {
  return (
    <Grid {...(isReversed && { reversed: "computer" })}>
      <Grid.Column width={5} className="flexTilesVerticalContainer">
        <div className="playerStatusContainerVertical">
          <div
          style={{minHeight: '500px'}}
            className="playerStatusHeaderVertical"
            {...(player.isMyTurn && { className: "playerTurn playerStatusHeaderVertical" })}
          >
            <span
              className="rotate90"
              {...(isReversed && { className: "rotateMinus90" })}
            >
              <PlayerStatus player={player} />
            </span>
          </div>
        </div>
      </Grid.Column>
      <Grid.Column width={5} className="flexTilesVerticalContainer">
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div key={rt.id} {...(isReversed && { className: "rotate180" })}>
                <img
                  alt='face-down-tile'
                  src={'/assets/tiles/v50px/face-down.png'}
                />
              </div>
            ))}
      </Grid.Column>

      <Grid.Column width={5} className="flexTilesVerticalContainer">
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserGraveyard)
            .map((rt) => (
              <div key={rt.id} {...(isReversed && { className: "rotate180" })}>
                <img
                  alt={rt.tile.title}
                  src={rt.tile.imageSmall.replace("50px", "v50px")}
                />
              </div>
            ))}
      </Grid.Column>
    </Grid>
  );
};
export default observer(TileListOtherPlayerVertical);
