import React, { Fragment, useContext } from "react";
import { WindDirection } from "../../../app/models/windEnum";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Button, Label, Image, Item } from "semantic-ui-react";
import { GameStatus } from "../../../app/models/gameStatusEnum";

interface IProps {
  wind: WindDirection;
}

const GameLobbySeat: React.FC<IProps> = ({ wind }) => {
  const rootStore = useContext(RootStoreContext);
  const { game, getMainUser } = rootStore.gameStore;
  const { sitGame, standUpGame, hubLoading } = rootStore.hubStore;
  const playerWind = game?.players.find((p) => p.initialSeatWind === wind);

  return (
    <Item>
      {playerWind ? (
        <Fragment>
          <Item.Content>
            <Label>{playerWind.displayName}</Label>
          </Item.Content>
          <Item.Content>
            <Image
              circular
              centered
              size="tiny"
              src={playerWind.image || "/assets/user.png"}
            />
          </Item.Content>
          {game?.status === GameStatus.Created && getMainUser?.initialSeatWind === wind &&  (
            <Item.Content>
              <Button loading={hubLoading} onClick={standUpGame}>
                Stand-Up
              </Button>
            </Item.Content>
          )}
        </Fragment>
      ) : (
        <Fragment>
          {game?.status === GameStatus.Created && (
            <Item.Content>
              <Button
                loading={hubLoading}
                onClick={() => sitGame(wind)}
                color="teal"
              >
                Sit
              </Button>
            </Item.Content>
          )}
        </Fragment>
      )}
    </Item>
  );
};

export default observer(GameLobbySeat);
