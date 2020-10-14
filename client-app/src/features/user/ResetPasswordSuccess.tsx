import React, { Fragment, useContext } from "react";
import { Button, Icon, Segment, Header } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoginForm from "./LoginForm";

const ResetPasswordSuccess: React.FC<RouteComponentProps> = () => {
  const rootStore = useContext(RootStoreContext);
  const { openModal } = rootStore.modalStore;
  return (
    <Fragment>
      <Segment placeholder>
        <Header icon>
          <Icon name="check" />
          Password has been reset - you can now login with the new password
        </Header>

        <Segment.Inline>
          <div className="center">
          <Button
              primary
              onClick={() => openModal(<LoginForm />)}
              size="large"
              content="Login"
            />
          </div>
        </Segment.Inline>
      </Segment>
    </Fragment>
  );
};

export default observer(ResetPasswordSuccess);
