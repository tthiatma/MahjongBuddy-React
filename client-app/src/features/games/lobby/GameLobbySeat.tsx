import React, { Fragment, useContext } from 'react';
import { WindDirection } from "../../../app/models/windEnum";import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Button, Label, Image, Item } from "semantic-ui-react";

interface IProps{
    wind: WindDirection;
  }

const GameLobbySeat: React.FC<IProps> = ({wind}) => {
    const rootStore = useContext(RootStoreContext);
    const {connectToGame, disconnectFromGame, loading, game} = rootStore.gameStore;
    const playerWind = game?.players.find(p => p.initialSeatWind === wind) 
   
    return (
      <Item>
        {playerWind && (
          <Fragment>
            <Item.Content>
              <Image
                centered
                size="tiny"
                src={playerWind.image || "/assets/user.png"}
              />
            </Item.Content>
            <Item.Content>
              <Label>{playerWind.displayName}</Label>
            </Item.Content>
          </Fragment>
        )}
        {game?.initialSeatWind === wind && (
          <Item.Content>
            <Button loading={loading} onClick={disconnectFromGame}>
              Leave
            </Button>
          </Item.Content>
        )}
        {!game?.isCurrentPlayerConnected &&
          !game?.players.some((p) => p.initialSeatWind === wind) && (
            <Item.Content>
              <Button
                loading={loading}
                onClick={() => connectToGame(wind)}
                color="teal"
              >
                Sit
              </Button>
            </Item.Content>
          )}
      </Item>
    );
}

export default observer(GameLobbySeat);
