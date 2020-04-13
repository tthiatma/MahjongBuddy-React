import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const TileListOtherPlayer: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
return <Fragment></Fragment>;
};

export default observer(TileListOtherPlayer);

