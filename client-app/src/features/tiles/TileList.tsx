import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import _ from "lodash";

const TileList: React.FC<{containerStyleName : string, tileStyleName: string}> = ({containerStyleName, tileStyleName}) => {  
  return (
    <Fragment>
      {_.times(13, i => (
        <span className={containerStyleName}>
          <img
          alt='facedown-tile'
            className={tileStyleName}
            src="/assets/tiles/50px/face-down.png"
          />
        </span>
      ))}
    </Fragment>
  );
};
export default observer(TileList);
