import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile, TileStatus } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";

interface IProps{
  roundTiles: IRoundTile[];
}

const TileListMainPlayer: React.FC<IProps> = ({ roundTiles }) => {
return <Fragment></Fragment>;
};

export default observer(TileListMainPlayer);

