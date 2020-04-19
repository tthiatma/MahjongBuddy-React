import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";
import { TileStatus } from "../../../app/models/tileStatus";
import { Grid, Label } from "semantic-ui-react";
import { IRoundPlayer } from "../../../app/models/round";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
  player: IRoundPlayer;
  rotation: string;
}

const TileList: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles, player, rotation }) => {
  const rootStore = useContext(RootStoreContext);
  const {selectedTile} = rootStore.roundStore;
  return (
    <Fragment>
      <Grid.Column width={1}>
        {rotation === "rotate90" ? (
          <Label>{player.userName}</Label>
        ) : (
          <div id="rawr">
            {roundTiles &&
              roundTiles
                .filter((t) => t.status === TileStatus.UserGraveyard)
                .map((rt) => (
                  <Fragment key={rt.id}>
                    <span className={`${containerStyleName} ${rotation}`}>
                      <img
                        alt="facedown-tile"
                        className={tileStyleName}
                        src={rt.tile.imageSmall}
                      />
                    </span>
                  </Fragment>
                ))}
          </div>
        )}
      </Grid.Column>

      <Grid.Column width={1}>
        <div>
          {roundTiles &&
            roundTiles
              .filter((t) => t.status === TileStatus.UserActive)
              .map((rt) => (
                <Fragment key={rt.id}>
                  <span
                    className={
                      selectedTile?.id === rt.id
                        ? `${containerStyleName} ${rotation} selectedTile`
                        : `${containerStyleName} ${rotation}`
                    }
                  >
                    <img
                      onClick={() =>
                        runInAction(() => {
                          rootStore.roundStore.selectedTile = rt;
                        })
                      }
                      alt="facedown-tile"
                      className={
                        selectedTile?.id === rt.id
                          ? `${tileStyleName} selectedTile`
                          : tileStyleName
                      }
                      src={rt.tile.imageSmall}
                    />
                  </span>
                </Fragment>
              ))}
        </div>
      </Grid.Column>

      <Grid.Column width={1}>
        {rotation === "rotate90" ? (
          <div id="rawr">
            {roundTiles &&
              roundTiles
                .filter((t) => t.status === TileStatus.UserGraveyard)
                .map((rt) => (
                  <Fragment key={rt.id}>
                    <span className={`${containerStyleName} ${rotation}`}>
                      <img
                        alt="facedown-tile"
                        className={tileStyleName}
                        src={rt.tile.imageSmall}
                      />
                    </span>
                  </Fragment>
                ))}
          </div>
        ) : (
          <Label>{player.userName}</Label>
        )}
      </Grid.Column>
    </Fragment>
  );
};
export default observer(TileList);
