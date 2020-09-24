import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import { Form, Button, Header, Segment } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { RootStoreContext } from "../../app/stores/rootStore";
import { IUserFormValues } from "../../app/models/user";
import { FORM_ERROR } from "final-form";
import { combineValidators, isRequired, composeValidators } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import { isValidEmail } from "../../app/common/validators/validators";

const validate = combineValidators({
  email: isRequired("Email"),
  displayName: isRequired("DisplayName"),
  userName: isRequired("UserName"),
  password: isRequired("Password"),
});

export const RegisterForm = () => {
  const rootStore = useContext(RootStoreContext);
  const { register } = rootStore.userStore;

  return (
    <FinalForm
      onSubmit={(values: IUserFormValues) =>
        register(values).catch((error) => ({
          [FORM_ERROR]: error
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
        <Form onSubmit={handleSubmit} error>
          <Header
            as="h2"
            content="Sign up to MahjongBuddy"
            color="teal"
            textAlign="center"
          />
          <Field name="userName" component={TextInput} placeholder="UserName" />
          <Field
            name="displayName"
            component={TextInput}
            placeholder="DisplayName"
          />
          <Field name="email" component={TextInput} placeholder="Email" />
          <Field
            name="password"
            component={TextInput}
            placeholder="Password"
            type="password"
          />
          {submitError && !dirtySinceLastSubmit && (
            <ErrorMessage error={submitError} />
          )}
          <Segment>
          By signing up, you agree to our{" "}
              <a href="/termsandconditions">terms and conditions</a> and{" "}
              <a href="/privacypolicy" target="_blank">
                privacy policy
              </a>
              .
          </Segment>
          <Button
            disabled={(invalid && !dirtySinceLastSubmit) || pristine}
            loading={submitting}
            color="teal"
            content="Register"
            fluid
          />
        </Form>
      )}
    />
  );
};
