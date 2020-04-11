import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const TileList: React.FC<IProps> = ({ containerStyleName, tileStyleName, roundTiles }) => {
  return (    
    <Fragment>
      {roundTiles.map((rt) => (
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

      {/* {_.times(13, (i) => (
        <span className={containerStyleName}>
          <img
            alt="facedown-tile"
            className={tileStyleName}
            src="/assets/tiles/50px/face-down.png"
          />
        </span>
      ))} */}
    </Fragment>
  );
};
export default observer(TileList);
