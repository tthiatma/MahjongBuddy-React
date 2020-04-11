import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile, TileStatus } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const TileList: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
  const rootStore = useContext(RootStoreContext);
  const {selectedTile} = rootStore.gameStore;
  return (
    <Fragment>
      <div id="rawr">
        {roundTiles
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
        {roundTiles
          .filter((t) => t.status === TileStatus.UserActive)
          .map((rt) => (
            <Fragment key={rt.id}>
              <span className={selectedTile?.id === rt.id ? `${containerStyleName} selectedTile` : containerStyleName}>
                <img
                  onClick={() =>
                    runInAction(() => {
                      rootStore.gameStore.selectedTile = rt;
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
        {roundTiles
          .filter((t) => t.status === TileStatus.JustPicked)
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
