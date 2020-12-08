import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import { Form, Button, Header, Divider, Menu } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { RootStoreContext } from "../../app/stores/rootStore";
import { IUserFormValues } from "../../app/models/user";
import { FORM_ERROR } from "final-form";
import { combineValidators, composeValidators, createValidator, isRequired } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import SocialLogin from "./SocialLogin";
import { observer } from "mobx-react-lite";
import { ForgotPasswordForm } from "./ForgotPasswordForm";

const isValidEmail = createValidator(
  message => value => {
    if (value && !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(value)) {
      return message
    }
  },
  'Invalid email address'
)

const validate = combineValidators({
  email: composeValidators(isRequired, isValidEmail)("email"),
  password: isRequired("password"),
});

const LoginForm = () => {
  const rootStore = useContext(RootStoreContext);
  const { login, fbLogin, loading } = rootStore.userStore;
  const { openModal } = rootStore.modalStore;
  
  return (
    <FinalForm
      onSubmit={(values: IUserFormValues) =>
        login(values).catch((error) => ({
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
            content="Login to MahjongBuddy"
            color="teal"
            textAlign="center"
          />
          <Field name="email" component={TextInput} placeholder="Email"/>
          <Field
            name="password"
            component={TextInput}
            placeholder="Password"
            type="password"
          />
          {submitError && !dirtySinceLastSubmit && (
            <ErrorMessage
              error={submitError}
              text="Invalid username or password"
            />
          )}
          <div style={{ textAlign: "right", paddingBottom: "10px", cursor: "pointer" }}>
            <Menu.Item onClick={() => openModal(<ForgotPasswordForm />)}>forgot password?</Menu.Item>
          </div>

          <Button
            disabled={(invalid && !dirtySinceLastSubmit) || pristine}
            loading={submitting}
            color="teal"
            content="Login"
            fluid
          />
          <Divider horizontal>Or</Divider>
          <SocialLogin loading={loading} fbCallback={fbLogin} />
        </Form>
      )}
    />
  );
};

export default observer(LoginForm);
