import React, { useContext, Fragment } from "react";
import { Item, Label } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import GameListItem from "./GameListItem";
import { RootStoreContext } from "../../../app/stores/rootStore";

const GameList: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { gamesByDate } = rootStore.gameStore;
  return (
    <Fragment>
      {gamesByDate.map(([group, games]) => (
        <Fragment key={group}>
          <Label size="large" color="blue">
            {group}
          </Label>
            <Item.Group divided>
              {games.map(game => (
                <GameListItem key={game.id} game={game} />
              ))}
            </Item.Group>
        </Fragment>
      ))}
    </Fragment>
  );
};
export default observer(GameList);
