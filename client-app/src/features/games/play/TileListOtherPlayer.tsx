import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { TileStatus } from "../../../app/models/tileStatus";
import { IRoundPlayer } from "../../../app/models/round";
import { Grid } from "semantic-ui-react";
import { WindDirection } from "../../../app/models/windEnum";

interface IProps {
  roundTiles: IRoundTile[];
  player: IRoundPlayer;
}

const TileListOtherPlayer: React.FC<IProps> = ({
  roundTiles,
  player
}) => {
  return (
    <Grid verticalAlign="middle">
      <Grid.Row centered className='playerStatusContainer' {...(player.isMyTurn && { className: 'playerTurn playerStatusContainer' })}>
        <span>
          {player.userName} - {WindDirection[player.wind]} - {player.points}
        </span>
      </Grid.Row>
      <Grid.Row centered>
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div key={rt.id}>
                <img
                  alt={rt.tile.title}
                  src={rt.tile.imageSmall}
                  style={{ overflow: "hidden" }}
                />
              </div>
            ))}
      </Grid.Row>
      <Grid.Row>
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserGraveyard)
            .map((rt) => (
              <div
                key={rt.id}
                style={{
                  backgroundImage: `url(${rt.tile.imageSmall}`,
                }}
              />
            ))}
      </Grid.Row>
    </Grid>
  );
};
export default observer(TileListOtherPlayer);
