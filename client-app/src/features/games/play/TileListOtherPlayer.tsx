import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { Grid, Image } from "semantic-ui-react";
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
    <Fragment>
      <Grid verticalAlign="middle">
        <Grid.Row centered>
          <Grid>
            <Grid.Column width={3}></Grid.Column>
            <Grid.Column width={10}>
              <div                
                style={{ borderRadius: "25px", verticalAlign:"middle" }}
                className="playerStatusContainer"
                {...(player &&
                  player.isMyTurn &&
                  player.mustThrow && {
                    className: "mustThrow playerStatusContainer",
                  })}
                {...(player &&
                  player.isMyTurn &&
                  !player.mustThrow && {
                    className: "playerTurn playerStatusContainer",
                  })}
              >
                <PlayerStatus player={player} />
              </div>
            </Grid.Column>
            <Grid.Column width={3}>
              <Image
                floated="left"
                style={{ width: "50%" }}
                circular
                src={player.image || "/assets/user.png"}
              />
            </Grid.Column>
          </Grid>
        </Grid.Row>
        <Grid.Row centered style={{ padding: "1px" }}>
          {player && displayClosedTile()}
        </Grid.Row>
        <Grid.Row centered style={{ padding: "1px" }}>
          {player &&
            player.graveyardTiles &&
            player.graveyardTiles
              .slice()
              .sort(sortTiles)
              .map((rt) => (
                <div key={rt.id}>
                  <img alt={rt.tile.title} src={rt.tile.imageSmall} />
                </div>
              ))}
        </Grid.Row>
      </Grid>
    </Fragment>
  );
};
export default observer(TileListOtherPlayer);
