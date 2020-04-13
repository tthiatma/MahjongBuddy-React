import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";
import { TileStatus } from "../../../app/models/tileStatus";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const TileList: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
  const rootStore = useContext(RootStoreContext);
  const {selectedTile} = rootStore.roundStore;
  return (
    <Fragment>
      <div id="rawr">
        {roundTiles && roundTiles
          .filter((t) => t.status === TileStatus.UserGraveyard)
          .map((rt) => (
            <Fragment key={rt.id}>
              <span className={containerStyleName}>
                <img
                  alt="facedown-tile"
                  className={tileStyleName}
                  src={rt.tile.imageSmall}
                />
              </span>
            </Fragment>
          ))}
      </div>
      <div>
        {roundTiles && roundTiles
          .filter((t) => t.status === TileStatus.UserActive)
          .map((rt) => (
            <Fragment key={rt.id}>
              <span className={selectedTile?.id === rt.id ? `${containerStyleName} selectedTile` : containerStyleName}>
                <img
                  onClick={() =>
                    runInAction(() => {
                      rootStore.roundStore.selectedTile = rt;
                    })
                  }
                  alt="facedown-tile"
                  className={selectedTile?.id === rt.id ? `${tileStyleName} selectedTile` : tileStyleName}
                  src={rt.tile.imageSmall}
                />
              </span>
            </Fragment>
          ))}
      </div>
      <div>
        {roundTiles && roundTiles
          .filter((t) => t.status === TileStatus.UserJustPicked)
          .map((rt) => (
            <Fragment key={rt.id}>
              <span className={containerStyleName}>
                <img
                  alt="facedown-tile"
                  className={tileStyleName}
                  src={rt.tile.imageSmall}
                />
              </span>
            </Fragment>
          ))}
      </div>
    </Fragment>
  );
};
export default observer(TileList);
