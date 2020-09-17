import React, { useContext, useState, useEffect, Fragment } from "react";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import queryString from "query-string";
import { Form as FinalForm, Field } from "react-final-form";
import agent from "../../app/api/agent";
import { Button, Segment, Header, Icon, Form } from "semantic-ui-react";
import LoginForm from "./LoginForm";
import { toast } from "react-toastify";
import { observer } from "mobx-react-lite";
import { FORM_ERROR } from "final-form";
import { IResetPasswordFormValues } from "../../app/models/user";
import { combineValidators, isRequired, matchesField } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import TextInput from "../../app/common/form/TextInput";

const ResetPasswordForm: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore = useContext(RootStoreContext);
  const { resetPassword } = rootStore.userStore;
  const Status = {
    Initial: "Initial",
    Resetting: "Resetting",
    Failed: "Failed",
    Success: "Success",
  };
  const [status, setStatus] = useState(Status.Initial);
  const { openModal } = rootStore.modalStore;
  const { token, email } = queryString.parse(location.search);
  const handleResetPasswordResend = () => {
    agent.User.resendForgotPassword(email as string)
      .then(() => {
        toast.success("Reset password email sent - please check your email");
      })
      .catch((error) => console.log(error));
  };

  const validate = combineValidators({
    newPassword: isRequired("NewPassword"),
    confirmNewPassword: matchesField(
      "newPassword",
      "NewPassword"
    )({
      message: "Passwords do not match",
    }),
  });

  const getBody = () => {
    switch (status) {
      case Status.Initial:
        return (
          <FinalForm
            onSubmit={(values: IResetPasswordFormValues) => {
              values.email = email as string;
              values.token = token as string;
              resetPassword(values).catch((error) => ({
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
                  content="Reset Password"
                  color="teal"
                  textAlign="center"
                />
                <Field
                  name="newPassword"
                  component={TextInput}
                  placeholder="New Password"
                  type="password"
                />

                <Field
                  name="confirmNewPassword"
                  component={TextInput}
                  placeholder="Confirm New Password"
                  type="password"
                />

                {submitError && !dirtySinceLastSubmit && (
                  <ErrorMessage error={submitError} />
                )}
                <Button
                  disabled={(invalid && !dirtySinceLastSubmit) || pristine}
                  loading={submitting}
                  color="teal"
                  content="Reset"
                  fluid
                />
              </Form>
            )}
          />
        );
      case Status.Resetting:
        return <p>Resetting</p>;
      case Status.Failed:
        return (
          <div className="center">
            <p>
              Reset password failed - you can try resending the forgot password
            </p>
            <Button
              onClick={handleResetPasswordResend}
              primary
              size="huge"
              content="Resend forgot password"
            />
          </div>
        );
      case Status.Success:
        return (
          <div className="center">
            <p>
              Password has been reset - you can now login with the new password
            </p>
            <Button
              primary
              onClick={() => openModal(<LoginForm />)}
              size="large"
              content="Login"
            />
          </div>
        );
    }
  };

  return (
    <Fragment>
      <Segment placeholder>
        <Header icon>
          <Icon name="envelope" />
          Reset Password
        </Header>
        <Segment.Inline>{getBody()}</Segment.Inline>
      </Segment>
    </Fragment>
  );
};

export default observer(ResetPasswordForm);
