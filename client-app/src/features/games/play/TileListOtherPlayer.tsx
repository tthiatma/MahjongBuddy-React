import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { TileStatus } from "../../../app/models/tileStatus";

interface IProps {
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const TileListOtherPlayer: React.FC<IProps> = ({
  roundTiles,
  tileStyleName,
  containerStyleName
}) => {
  return (
    <Fragment>
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
