import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";

interface IProps{
  roundTiles: IRoundTile[];
}

const TileListMainPlayer: React.FC<IProps> = ({ roundTiles }) => {
return <Fragment></Fragment>;
};

export default observer(TileListMainPlayer);

