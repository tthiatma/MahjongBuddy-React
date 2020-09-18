import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import { Form, Button, Header } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { RootStoreContext } from "../../app/stores/rootStore";
import { FORM_ERROR } from "final-form";
import { combineValidators, isRequired } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import { IResetPasswordFormValues } from "../../app/models/user";

const validate = combineValidators({
  email: isRequired("Email"),
});

export const ForgotPasswordForm = () => {
  const rootStore = useContext(RootStoreContext);
  const { forgotPassword } = rootStore.userStore;

  return (
    <FinalForm
      onSubmit={(values: IResetPasswordFormValues) => {
        forgotPassword(values).catch((error) => ({
          [FORM_ERROR]: error,
        }));
      }}
      validate={validate}
      render={({
        handleSubmit,
        submitting,
        submitError,
        invalid,
        pristine,
        dirtySinceLastSubmit,
      }) => (
        <Form onSubmit={handleSubmit} error>
          <Header
            as="h2"
            content="Forgot Password"
            color="teal"
            textAlign="center"
          />
          <Field name="email" component={TextInput} placeholder="Email" />
          {submitError && !dirtySinceLastSubmit && (
            <ErrorMessage error={submitError} />
          )}
          <Button
            disabled={(invalid && !dirtySinceLastSubmit) || pristine}
            loading={submitting}
            color="teal"
            content="Submit"
            fluid
          />
        </Form>
      )}
    />
  );
};
