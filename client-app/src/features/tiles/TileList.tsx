import React, { useContext, Fragment } from "react";
import { Item, Label } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import GameStore from "../../app/stores/gameStore";
import TileListItem from "./TileListItem";

const ActivityList: React.FC = () => {
  const gameStore = useContext(GameStore);
  const { gamesByDate } = gameStore;
  return (
    <Fragment>
      {gamesByDate.map(([group, tiles]) => (
        <Fragment key={group}>
          <Label size="large" color="blue">
            {group}
          </Label>
            <Item.Group divided>
              {tiles.map(tile => (
                <TileListItem key={tile.id} tile={tile} />
              ))}
            </Item.Group>
        </Fragment>
      ))}
    </Fragment>
  );
};
export default observer(ActivityList);
