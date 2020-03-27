import React, { useState, FormEvent, useContext, useEffect } from "react";
import { Segment, Form, Button, Grid } from "semantic-ui-react";
import { IGame } from "../../../app/models/game";
import { v4 as uuid } from "uuid";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../../app/stores/rootStore";

interface DetailParams {
  id: string;
}
const GameForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    createGame,
    editGame,
    submitting,
    game: initialFormState,
    loadGame,
    clearGame
  } = rootStore.gameStore;

  const [game, setGame] = useState<IGame>({
    id: "",
    title: "",
    date: "",
    players: []
  });

  useEffect(() => {
    if (match.params.id && game.id.length === 0) {
      loadGame(match.params.id).then(
        () => initialFormState && setGame(initialFormState)
      );
    }
    return () => {
      clearGame();
    };
  }, [
    loadGame,
    match.params.id,
    clearGame,
    initialFormState,
    game.id.length
  ]);

  const handleInputChange = (
    event: FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = event.currentTarget;
    setGame({ ...game, [name]: value });
  };

  const handleSubmit = () => {
    if (game.id.length === 0) {
      let newGame = {
        ...game,
        id: uuid()
      };
      createGame(newGame).then(() =>
        history.push(`/games/${newGame.id}`)
      );
    } else {
      editGame(game).then(() =>
        history.push(`/games/${game.id}`)
      );
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <Form onSubmit={handleSubmit}>
            <Form.Input
              onChange={handleInputChange}
              name="title"
              placeholder="Title"
              value={game.title}
            />
            <Form.Input
              onChange={handleInputChange}
              name="date"
              type="datetime-local"
              placeholder="Date"
              value={game.date}
            />
            <Button
              loading={submitting}
              floated="right"
              positive
              type="submit"
              content="Submit"
            />
            <Button
              onClick={() => history.push("/games")}
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameForm);
