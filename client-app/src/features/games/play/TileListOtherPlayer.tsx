import React from "react";
import { observer } from "mobx-react-lite";
import { Grid } from "semantic-ui-react";
import PlayerStatus from "./PlayerStatus";
import { IRoundOtherPlayer } from "../../../app/models/player";
import { sortTiles } from "../../../app/common/util/util";

interface IProps {
  player: IRoundOtherPlayer;
}

const TileListOtherPlayer: React.FC<IProps> = ({ player }) => {
  const displayClosedTile = () => {
    let closedTiles = [];
    for (let i = 0; i < player.activeTilesCount; i++) {
      closedTiles.push(
        <div key={`${player.userName}${i}`}>
          <img
            alt={"face-down-tile"}
            src={"/assets/tiles/50px/face-down.png"}
            style={{ overflow: "hidden" }}
          />
        </div>
      );
    }
    return closedTiles;
  };
  return (
    <Grid verticalAlign="middle">
      <Grid.Row
        style={{ borderRadius: "25px" }}
        centered
        className="playerStatusContainer"
        {...(player && player.isMyTurn && player.mustThrow && {
          className: "mustThrow playerStatusContainer",
        })}
        {...(player && player.isMyTurn && !player.mustThrow && {
          className: "playerTurn playerStatusContainer",
        })}
      >
        <span>
          <PlayerStatus player={player} />
        </span>
      </Grid.Row>
      <Grid.Row centered style={{ padding: "1px" }}>
        {player && displayClosedTile()}
        {/* {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div key={rt.id}>
                <img
                  alt={"face-down-tile"}
                  src={"/assets/tiles/50px/face-down.png"}
                  style={{ overflow: "hidden" }}
                />
              </div>
            ))} */}
      </Grid.Row>
      <Grid.Row centered style={{ padding: "1px" }}>
        {player && player.graveyardTiles &&
          player.graveyardTiles.sort(sortTiles).map((rt) => (
            <div key={rt.id}>
              <img alt={rt.tile.title} src={rt.tile.imageSmall} />
            </div>
          ))}
      </Grid.Row>
    </Grid>
  );
};
export default observer(TileListOtherPlayer);
