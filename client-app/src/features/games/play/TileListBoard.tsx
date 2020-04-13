import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";

interface IProps {
  roundTiles: IRoundTile[];
}

const TileListBoard: React.FC<IProps> = ({ roundTiles }) => {
  return (
    <Fragment>
      {roundTiles.sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter).map((rt) => (
        <Fragment key={rt.id}>
            <span>{rt.boardGraveyardCounter}</span>
          <img src={rt.tile.imageSmall} />
        </Fragment>
      ))}
    </Fragment>
  );
};

export default observer(TileListBoard);
