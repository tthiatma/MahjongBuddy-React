import React, { Fragment } from "react";
import { Button, Icon, Segment, Header } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import queryString from "query-string";
import { RouteComponentProps } from "react-router-dom";
import agent from "../../app/api/agent";
import { toast } from "react-toastify";

const ForgotPasswordSuccess: React.FC<RouteComponentProps> = ({ location }) => {
  const email = queryString.parse(location.search);
  const handleConfirmEmailResend = () => {
    agent.User.resendForgotPassword(email.toString())
      .then(() => {
        toast.success("Reset password email sent - please check your email");
      })
      .catch((error) => console.log(error));
  };

  return (
    <Fragment>
      <Segment placeholder>
        <Header icon>
          <Icon name="check" />
          Password reset link sent!(if the account exists)
        </Header>

        <Segment.Inline>
          <div className="center">
            <p> 
              Please check your email (including junk folder) for the
              verification email
            </p>
            {email && (
              <div>
                <p>
                  Didn't receive the email? Please click below button to resend
                </p>
                <Button
                  onClick={handleConfirmEmailResend}
                  primary
                  content="Resend email"
                  size="huge"
                />
              </div>
            )}
          </div>
        </Segment.Inline>
      </Segment>
    </Fragment>
  );
};

export default observer(ForgotPasswordSuccess);
