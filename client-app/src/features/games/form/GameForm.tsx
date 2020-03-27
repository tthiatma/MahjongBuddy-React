import React, { useState, useContext, useEffect } from "react";
import { Segment, Form, Button, Grid } from "semantic-ui-react";
import { GameFormValues } from '../../../app/models/game';
import { v4 as uuid } from "uuid";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { Form as FinalForm, Field } from 'react-final-form';
import { RootStoreContext } from "../../../app/stores/rootStore";
import TextInput from '../../../app/common/form/TextInput';
import {
  combineValidators,
  isRequired
} from 'revalidate';

const validate = combineValidators({
  title: isRequired({ message: 'The event title is required' }),
});

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
    loadGame,
  } = rootStore.gameStore;

  const [game, setGame] = useState(new GameFormValues());
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (match.params.id) {
      setLoading(true);
      loadGame(match.params.id)
        .then(game => {
          setGame(new GameFormValues(game));
        })
        .finally(() => setLoading(false));
    }
  }, [loadGame, match.params.id, game]);

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = new Date();
    const { date, time, ...game } = values;
    game.date = dateAndTime;
    if (!game.id) {
      let newGame = {
        ...game,
        id: uuid()
      };
      createGame(newGame);
    } else {
      editGame(game);
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <FinalForm
            validate={validate}
            initialValues={game}
            onSubmit={handleFinalFormSubmit}
            render={({ handleSubmit, invalid, pristine }) => (
              <Form onSubmit={handleSubmit} loading={loading}>
                <Field
                  name='title'
                  placeholder='Title'
                  value={game.title}
                  component={TextInput}
                />
                <Button
                  loading={submitting}
                  disabled={loading || invalid || pristine}
                  floated='right'
                  positive
                  type='submit'
                  content='Submit'
                />
                <Button
                  onClick={
                    game.id
                      ? () => history.push(`/lobby/${game.id}`)
                      : () => history.push('/games')
                  }
                  disabled={loading}
                  floated='right'
                  type='button'
                  content='Cancel'
                />
              </Form>
            )}
          />
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameForm);
