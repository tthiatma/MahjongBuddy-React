import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile, TileStatus } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";

interface IProps {
  roundTiles: IRoundTile[];
}

const TileListBoard: React.FC<IProps> = ({ roundTiles }) => {
  return (
    <Fragment>
      {roundTiles.sort(rt => rt.boardGraveyardCounter).map((rt) => (
        <Fragment key={rt.id}>
            <span>{rt.boardGraveyardCounter}</span>
          <img src={rt.tile.imageSmall} />
        </Fragment>
      ))}
    </Fragment>
  );
};

export default observer(TileListBoard);
