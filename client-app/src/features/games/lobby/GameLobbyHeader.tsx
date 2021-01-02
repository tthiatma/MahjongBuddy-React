import React, { Fragment, useContext } from "react";
import { Segment, Item, Header, Button, Image, Icon } from "semantic-ui-react";
import { IGame } from "../../../app/models/game";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { format } from "date-fns";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { IRound } from "../../../app/models/round";
import { GameStatus } from "../../../app/models/gameStatusEnum";

const gameImageStyle = {
  filter: "brightness(30%)",
};

const gameImageTextStyle = {
  position: "absolute",
  bottom: "5%",
  left: "5%",
  width: "100%",
  height: "auto",
  color: "white",
};

const GameLobbyHeader: React.FC<{
  game: IGame;
  latestRound: IRound | null;
}> = ({ game, latestRound }) => {
  const rootStore = useContext(RootStoreContext);
  const { userNoWind, gameIsOver } = rootStore.gameStore;
  const {
    hubLoading,
    startRound,
    joinGame,
    endGame,
    cancelGame,
    leaveGame,
    randomizeWind,
  } = rootStore.hubStore;
  const host = game.gamePlayers.filter((p) => p.isHost)[0];

  return (
    <Segment.Group>
      
      <Segment basic attached="top" style={{ padding: "0" }}>
        <Image src={`/assets/mahjong-tiles.jpg`} fluid style={gameImageStyle} />
        <Segment style={gameImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="huge"
                  content={game.title}
                  style={{ color: "white" }}
                />
                <p>{format(new Date(game.date), "MMMM do, yyyy")}</p>
                {host && (
                  <p>
                    Hosted by{" "}
                    <Link to={`/profile/${host.userName}`}>
                      <strong>{host.displayName}</strong>
                    </Link>
                  </p>
                )}
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      
      <Segment clearing attached="bottom">
        {game.gamePlayers.length < 4 && (
          <p>Waiting for 4 players to be in the game...</p>
        )}
        {game.gamePlayers.length === 4 && userNoWind && (
          <p>Waiting for players to take their seat...</p>
        )}
        {game.status === GameStatus.Created &&
          game.isHost &&
          game.gamePlayers.length === 4 &&
          !userNoWind && (
            <Button
              icon
              labelPosition="right"
              floated="right"
              color="blue"
              loading={hubLoading}
              onClick={startRound}
            >
              Start Game
              <Icon name="play" />
            </Button>
          )}
        {game.status === GameStatus.Created &&
          game.isHost &&
          game.gamePlayers.length === 4 && (
            <Button color="orange" loading={hubLoading} onClick={randomizeWind}>
              Shuffle Seat
            </Button>
          )}
        {latestRound && (
          <Button
            as={Link}
            loading={hubLoading}
            to={`/games/${game.code}/rounds/${latestRound.roundCounter}`}
            color="blue"
            floated="right"
          >
            Go to game
          </Button>
        )}
        {game.isCurrentPlayerConnected &&
          !game.isHost &&
          game.status === GameStatus.Created && (
            <Button floated="right" loading={hubLoading} onClick={leaveGame}>
              Leave Game
            </Button>
          )}

        {!game.isCurrentPlayerConnected &&
          !game.isHost &&
          game.status === GameStatus.Created && (
            <Button
              floated="right"
              loading={hubLoading}
              onClick={joinGame}
              color="teal"
            >
              Join Game
            </Button>
          )}

        {/* {game.isHost && (
          <Button
            as={Link}
            to={`/manage/${game.id}`}
            color="orange"
            floated="right"
          >
            Edit Game
          </Button>
        )} */}
      </Segment>
      {game.isHost && !gameIsOver && (
        <Segment clearing attached="bottom">
          <Icon size="large" color="teal" name="cogs" />
          {gameIsOver && <span>Game Over</span>}
          {!gameIsOver && game.status === GameStatus.Created && (
            <Fragment>
            Cancel game if you no longer need the game. 
            <Button
              floated="right"
              loading={hubLoading}
              onClick={() => cancelGame(game.code)}
              color="violet"
            >
              Cancel Game
            </Button>

            </Fragment>
          )}
          {game.isHost && !gameIsOver && game.status === GameStatus.Playing && (
            <Fragment>
              End game when you are done
            <Button
              floated="right"
              loading={hubLoading}
              onClick={endGame}
              color="red"
            >
              End game
            </Button>

            </Fragment>
          )}
        </Segment>
      )}
    </Segment.Group>
  );
};

export default observer(GameLobbyHeader);
