import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { TileStatus } from "../../../app/models/tileStatus";
import { IRoundPlayer } from "../../../app/models/round";
import { Label, Segment } from "semantic-ui-react";
import { runInAction } from "mobx";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";

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
  const rootStore = useContext(RootStoreContext);
  return (
    <Fragment>
      <div>
        {player && (
          <Segment.Group horizontal size="tiny">
            <Segment {...(player.isMyTurn && { color: "green" })}>
              <Label>{player.userName}</Label>
            </Segment>
            <Segment {...(player.isMyTurn && { color: "green" })}>
              {WindDirection[player.wind]}
            </Segment>
          </Segment.Group>
        )}
      </div>
      <div className={containerStyleName}>
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
