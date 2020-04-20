import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { TileStatus } from "../../../app/models/tileStatus";
import { IRoundPlayer } from "../../../app/models/round";
import { Label } from "semantic-ui-react";

interface IProps {
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
  player: IRoundPlayer;
}

const TileListOtherPlayer: React.FC<IProps> = ({
  roundTiles,
  tileStyleName,
  containerStyleName,
  player
}) => {
  return (
    <Fragment>
      <div>
      {player && (<Label>{player.userName}</Label>)}
      </div>
      <div className={containerStyleName}>
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div
                key={rt.id}
                style={{
                  backgroundImage: `url(${rt.tile.imageSmall}`,
                }}
                className={tileStyleName}
              />
            ))}
      </div>
      <div className={containerStyleName}>
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserGraveyard)
            .map((rt) => (
              <div
                key={rt.id}
                style={{
                  backgroundImage: `url(${rt.tile.imageSmall}`,
                }}
                className={tileStyleName}
              />
            ))}
      </div>
    </Fragment>
  );
};
export default observer(TileListOtherPlayer);
