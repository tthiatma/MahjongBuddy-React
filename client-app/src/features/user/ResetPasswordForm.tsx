import React, { useContext, Fragment } from "react";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import queryString from "query-string";
import { Form as FinalForm, Field } from "react-final-form";
import { Button, Form, Segment, Header, Icon } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { FORM_ERROR } from "final-form";
import { IResetPasswordFormValues } from "../../app/models/user";
import { combineValidators, isRequired, matchesField } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import TextInput from "../../app/common/form/TextInput";

const ResetPasswordForm: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore = useContext(RootStoreContext);
  const { resetPassword } = rootStore.userStore;
  const { token, email } = queryString.parse(location.search);

  const updateFormValue = (values: IResetPasswordFormValues) => {
    values.email = email as string;
    values.token = token as string;
    return values;
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
  return (
    <Fragment>
      <Segment placeholder>
      <Header icon>
          <Icon name="envelope" />
          Reset Password
        </Header>
        <Segment.Inline>
          <FinalForm
            onSubmit={(values: IResetPasswordFormValues) =>
              resetPassword(updateFormValue(values)).catch((error) => ({
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
              <Form onSubmit={handleSubmit} error>
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
        </Segment.Inline>
      </Segment>
    </Fragment>
  );
};

export default observer(ResetPasswordForm);
