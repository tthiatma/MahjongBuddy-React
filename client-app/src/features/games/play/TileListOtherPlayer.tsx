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
}

const TileListOtherPlayer: React.FC<IProps> = ({ roundTiles, player }) => {
  return (
    <Grid verticalAlign="middle">
      <Grid.Row style={{borderRadius: "25px"}}
        centered
        className="playerStatusContainer"
        {...(player.isMyTurn && {
          className: "playerTurn playerStatusContainer",
        })}
      >
        <span>
          <PlayerStatus player={player} />
        </span>
      </Grid.Row>
      <Grid.Row centered style={{ padding: "1px" }}>
        {roundTiles &&
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
            ))}
      </Grid.Row>
      <Grid.Row centered style={{ padding: "1px" }}>
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserGraveyard)
            .map((rt) => (
              <div key={rt.id}>
                <img
                  alt={rt.tile.title}
                  src={rt.tile.imageSmall}
                />
              </div>
            ))}
      </Grid.Row>
    </Grid>
  );
};
export default observer(TileListOtherPlayer);
