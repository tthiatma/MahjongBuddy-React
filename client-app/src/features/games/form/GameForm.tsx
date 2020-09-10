import React, { useState, useContext, useEffect, Fragment } from "react";
import { Segment, Form, Button, Grid, Container } from "semantic-ui-react";
import { GameFormValues } from "../../../app/models/game";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { Form as FinalForm, Field } from "react-final-form";
import { RootStoreContext } from "../../../app/stores/rootStore";
import TextInput from "../../../app/common/form/TextInput";
import { combineValidators, isRequired, isNumeric, composeValidators, createValidator } from "revalidate";
import NavBar from "../../nav/NavBar";

const isGreaterThan = (n: number) => createValidator(
  message => value => {
    if (value && Number(value) <= n) {
      return message
    }
  },
  field => `${field} must be greater than ${n}`
)


const validate = combineValidators({
  title: isRequired({ message: "The game title is required" }),
  minPoint:composeValidators(isRequired,isNumeric,isGreaterThan(-1))("minPoint"),
  maxPoint:composeValidators(isRequired,isNumeric,isGreaterThan(0))("maxPoint"),
});

interface DetailParams {
  id: string;
}
const GameForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { createGame, editGame, submitting, loadGame } = rootStore.gameStore;

  const [game, setGame] = useState(new GameFormValues());
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (match.params.id) {
      setLoading(true);
      loadGame(match.params.id)
        .then((game) => {
          setGame(new GameFormValues(game));
        })
        .finally(() => setLoading(false));
    }
  }, [loadGame, match.params.id]);

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = new Date();
    const { date, ...game } = values;
    game.date = dateAndTime;
    if (!game.id) {
      let newGame = {
        ...game,
      };
      createGame(newGame);
    } else {
      editGame(game);
    }
  };

  return (
    <Fragment>
      <NavBar />
      <Container style={{ paddingTop: "5em" }}>
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
                      name="title"
                      placeholder="Game Title"
                      value={game.title}
                      component={TextInput}
                    />
                    <Field                      
                      name="minPoint"
                      placeholder="Minimum point to win: eg. 3"
                      value={game.minPoint}
                      component={TextInput}
                    />
                    <Field
                      name="maxPoint"
                      placeholder="Maximum point player can win: eg. 10"
                      value={game.maxPoint}
                      component={TextInput}
                    />
                    <Button
                      loading={submitting}
                      disabled={loading || invalid || pristine}
                      floated="right"
                      positive
                      type="submit"
                      content="Submit"
                    />
                    <Button
                      onClick={
                        game.id
                          ? () => history.push(`/games/${game.id}`)
                          : () => history.push("/games")
                      }
                      disabled={loading}
                      floated="right"
                      type="button"
                      content="Cancel"
                    />
                  </Form>
                )}
              />
            </Segment>
          </Grid.Column>
        </Grid>
      </Container>
    </Fragment>
  );
};

export default observer(GameForm);
