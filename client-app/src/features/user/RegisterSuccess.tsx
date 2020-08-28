import React from "react";
import { Button, Icon, Segment, Header } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import queryString from "query-string";
import { RouteComponentProps } from "react-router-dom";
import agent from "../../app/api/agent";
import { toast } from "react-toastify";

const RegisterSuccess: React.FC<RouteComponentProps> = ({ location }) => {
  const email = queryString.parse(location.search);
  const handleConfirmEmailResend = () => {
    agent.User.resendVerifyEmailConfirm(email.toString())
      .then(() => {
        toast.success("Verification email sent - please check your email");
      })
      .catch((error) => console.log(error));
  };

  return (
    <Segment placeholder>
      <Header icon>
        <Icon name="check" />
        Successfully registered!
      </Header>

      <Segment.Inline>
        <div className="center">
          <p>
            Please check your email (including junk folder) for the verification
            email
          </p>
          {email && (
            <>
              <p>
                Didn't receive the email? Please click below button to resend
              </p>
              <Button
                onClick={handleConfirmEmailResend}
                primary
                content="Resend email"
                size="huge"
              />
            </>
          )}
        </div>
      </Segment.Inline>
    </Segment>
  );
};

export default observer(RegisterSuccess);
