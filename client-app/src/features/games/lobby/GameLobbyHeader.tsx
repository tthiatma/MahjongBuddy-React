import React, { useContext } from "react";
import { Segment, Item, Header, Button, Image } from "semantic-ui-react";
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
  const { hubLoading, startRound, joinGame, leaveGame, randomizeWind } = rootStore.hubStore;
  const host = game.players.filter((p) => p.isHost)[0];
  const userNoWind = game.players.some((p) => p.initialSeatWind === null);

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
                <p>{format(new Date(game.date), "eeee do MMMM")}</p>
                {host && (
                  <p>
                    Hosted by
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
        {game.status === GameStatus.Created &&
          game.isHost &&
          game.players.length === 4 && !userNoWind && (
            <Button loading={hubLoading} onClick={startRound}>
              Start
            </Button>
          )}
          {game.status === GameStatus.Created &&
          game.isHost &&
          game.players.length === 4 &&  (
            <Button loading={hubLoading} onClick={randomizeWind}>
              Randomize
            </Button>
          )}
        {game.status === GameStatus.Playing && latestRound !== null && (
          <Button
            as={Link}
            loading={hubLoading}
            to={`/games/${game.id}/rounds/${latestRound.id}`}
            color="orange"
            floated="right"
          >
            Play
          </Button>
        )}
        {game.isCurrentPlayerConnected && !game.isHost && game.status === GameStatus.Created && (
          <Button floated="right" loading={hubLoading} onClick={leaveGame}>
            Leave Game
          </Button>
        )}

        {!game.isCurrentPlayerConnected && !game.isHost && game.status === GameStatus.Created && (
          <Button floated="right" loading={hubLoading} onClick={joinGame} color="teal">
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
    </Segment.Group>
  );
};

export default observer(GameLobbyHeader);
