import React, { Fragment, useContext } from "react";
import { WindDirection } from "../../../app/models/windEnum";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Button, Label, Image, Item } from "semantic-ui-react";

interface IProps {
  wind: WindDirection;
}

const GameLobbySeat: React.FC<IProps> = ({ wind }) => {
  const rootStore = useContext(RootStoreContext);
  const { game } = rootStore.gameStore;
  const { connectToGame, disconnectFromGame, hubLoading } = rootStore.hubStore;
  const playerWind = game?.players.find((p) => p.initialSeatWind === wind);

  return (
    <Item>
      {playerWind && (
        <Fragment>
          <Item.Content>
            <Label>{playerWind.displayName}</Label>
          </Item.Content>
          <Item.Content>
            <Image
              centered
              size="tiny"
              src={playerWind.image || "/assets/user.png"}
            />
          </Item.Content>
        </Fragment>
      )}
      {game?.initialSeatWind === wind && (
        <Item.Content>
          <Button loading={hubLoading} onClick={disconnectFromGame}>
            Leave
          </Button>
        </Item.Content>
      )}
      {!game?.isCurrentPlayerConnected &&
        !game?.players.some((p) => p.initialSeatWind === wind) && (
          <Item.Content>
            <Button
              loading={hubLoading}
              onClick={() => connectToGame(wind)}
              color="teal"
            >
              Sit
            </Button>
          </Item.Content>
        )}
    </Item>
  );
};

export default observer(GameLobbySeat);
