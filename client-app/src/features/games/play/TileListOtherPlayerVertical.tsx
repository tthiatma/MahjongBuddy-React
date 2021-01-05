import React from "react";
import { observer } from "mobx-react-lite";
import { Grid, Image } from "semantic-ui-react";
import PlayerStatus from "./PlayerStatus";
import { IRoundOtherPlayer } from "../../../app/models/player";
import { sortTiles } from "../../../app/common/util/util";

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
          key={`${player.userName}${i}`}
          style={{ height: "34px" }}
          {...(isReversed && { className: "rotate180" })}
        >
          <img alt="face-down-tile" src={"/assets/tiles/v50px/face-down.png"} />
        </div>
      );
    }
    return closedTiles;
  };

  return (
    <Grid {...(isReversed && { reversed: "computer" })}>
      <Grid.Column width={4} className="flexTilesVerticalContainer">
        <Grid>
          <Grid.Row>
            {!isReversed && <Image circular src={player.image || "/assets/user.png"} />}
          </Grid.Row>
          <Grid.Row>
            <div
              style={{marginLeft: isReversed? "12px": "0px"}}
              className="playerStatusContainerVertical"
              {...(player &&
                player.isMyTurn &&
                player.mustThrow && {
                  className: "mustThrow playerStatusHeaderVertical",
                })}
              {...(player &&
                player.isMyTurn &&
                !player.mustThrow && {
                  className: "playerTurn playerStatusHeaderVertical",
                })}
            >
              <div
                style={{ minHeight: "350px" }}
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
          </Grid.Row>
          <Grid.Row>
            {isReversed && <Image verticalAlign="top" circular src={player.image || "/assets/user.png"} />}
          </Grid.Row>
        </Grid>
      </Grid.Column>

      <Grid.Column width={5} className="flexTilesVerticalContainer">
        {player && displayClosedTile()}
      </Grid.Column>

      <Grid.Column
        style={{ padding: "2px" }}
        width={5}
        className="flexTilesVerticalContainer"
      >
        {player &&
          player.graveyardTiles
            .slice()
            .sort(sortTiles)
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
