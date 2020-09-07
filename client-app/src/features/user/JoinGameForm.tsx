import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import { Form, Button, Header } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { RootStoreContext } from "../../app/stores/rootStore";
import { FORM_ERROR } from "final-form";
import { combineValidators, isRequired, composeValidators, isNumeric } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import { observer } from "mobx-react-lite";

const validate = combineValidators({
  gameId: composeValidators(isRequired,isNumeric)("gameId"),
});

const JoinGameForm = () => {
  const rootStore = useContext(RootStoreContext);
  const {joinGameById} = rootStore.gameStore;

  return (
    <FinalForm 
      onSubmit={(values: any) =>
        joinGameById(values.gameId).catch((error) => ({
          [FORM_ERROR]: error,
        }))
      }
      validate={validate}
      render={({
        handleSubmit,
        submitting,
        submitError,
        invalid,
        pristine,
        dirtySinceLastSubmit,
      }) => (
        <Form onSubmit={handleSubmit} error autoComplete="off">
          <Header
            as="h2"
            content="Enter Game #"
            color="teal"
            textAlign="center"
          />
          <Field
            name="gameId"
            component={TextInput}
            placeholder="Game #"
          />
          {submitError && !dirtySinceLastSubmit && (
            <ErrorMessage error={submitError} text="Invalid Game#" />
          )}
          <Button
            disabled={(invalid && !dirtySinceLastSubmit) || pristine}
            loading={submitting}
            color="teal"
            content="Join"
            fluid
          />
        </Form>
      )}
    />
  );
};

export default observer(JoinGameForm);
