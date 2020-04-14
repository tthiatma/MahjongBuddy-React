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

const TileListOtherPlayer: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
  const rootStore = useContext(RootStoreContext);
  const {selectedTile} = rootStore.roundStore;
  return (
    <Fragment>
      <div className="flexTilesContainer">
        {roundTiles &&
          roundTiles
            .filter((t) => t.status === TileStatus.UserActive)
            .map((rt) => (
              <div
                onClick={() =>
                  runInAction(() => {
                    rootStore.roundStore.selectedTile = rt;
                  })
                }
                key={rt.id}
                className={selectedTile?.id === rt.id ? `flexTiles selectedTile` : 'flexTiles'}
                style={{ backgroundImage: `url(${rt.tile.imageSmall}` }}
              ></div>
            ))}
      </div>
    </Fragment>
  );
};
export default observer(TileListOtherPlayer);
