import React from "react";
import { observer } from "mobx-react-lite";
import { Grid } from "semantic-ui-react";
import PlayerStatus from "./PlayerStatus";
import { IRoundOtherPlayer } from "../../../app/models/player";

interface IProps {
  player: IRoundOtherPlayer;
  isReversed: boolean;
}

const TileListOtherPlayerVertical: React.FC<IProps> = ({
  player,
  isReversed,
}) => {
  const displayClosedTile = () => {
    let closedTiles = [];
    for (let i = 0; i < player.activeTilesCount; i++) {
      closedTiles.push(
        <div
          style={{ height: "34px" }}
          {...(isReversed && { className: "rotate180" })}
        >
          <img alt="face-down-tile" src={"/assets/tiles/v50px/face-down.png"} />
        </div>
      );
      return closedTiles;
    }
  };

  return (
    <Grid {...(isReversed && { reversed: "computer" })}>
      <Grid.Column
        style={{ padding: "2px" }}
        width={3}
        className="flexTilesVerticalContainer"
      >
        <div
          className="playerStatusContainerVertical"
          {...(player.isMyTurn && {
            className: "playerTurn playerStatusHeaderVertical",
          })}
        >
          <div
            style={{ minHeight: "450px" }}
            className="playerStatusHeaderVertical"
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
      {displayClosedTile()}
        {/* {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div
                style={{ height: "34px" }}
                key={rt.id}
                {...(isReversed && { className: "rotate180" })}
              >
                <img
                  alt="face-down-tile"
                  src={"/assets/tiles/v50px/face-down.png"}
                />
              </div>
            ))} */}
      </Grid.Column>

      <Grid.Column
        style={{ padding: "2px" }}
        width={5}
        className="flexTilesVerticalContainer"
      >
        {player.graveyardTiles
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
