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

const TileListOtherPlayer: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
return <Fragment></Fragment>;
};

export default observer(TileListOtherPlayer);

