import React, { useContext, Fragment } from "react";
import { Item, Label, Grid, Image } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import GameStore from "../../app/stores/gameStore";
import _ from "lodash";

const TileList: React.FC<{containerStyleName : string, tileStyleName: string}> = ({containerStyleName, tileStyleName}) => {
  const gameStore = useContext(GameStore);
  
  return (
    <Fragment>
      {_.times(13, i => (
        <span className={containerStyleName}>
          <img
            className={tileStyleName}
            src="/assets/tiles/50px/face-down.png"
          />
        </span>
      ))}
    </Fragment>
  );
};
export default observer(TileList);
