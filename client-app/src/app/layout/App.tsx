import React, { Fragment } from "react";
import { Container } from "semantic-ui-react";
import NavBar from "../../features/nav/NavBar";
import { observer } from "mobx-react-lite";
import { Route, withRouter, RouteComponentProps } from "react-router-dom";
import GameDashboard from "../../features/games/dashboard/GameDashboard";
import HomePage from "../../features/home/HomePage";
import GameForm from "../../features/games/form/GameForm";
import GameOn from "../../features/games/play/GameOn";
import GameLobby from "../../features/games/lobby/GameLobby";

const App: React.FC<RouteComponentProps> = ({ location }) => {
  return (
    <Fragment>
      <Route exact path="/" component={HomePage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <Fragment>
            <NavBar />
            <Container style={{ marginTop: "5em" }}>
            <Route exact path="/games" component={GameDashboard} />
            <Route path="/games/:id" component={GameOn} />
            <Route path="/lobby/:id" component={GameLobby} />
            <Route
                key={location.key}
                path={["/createGame", "/manage/:id"]}
                component={GameForm}
                />
                </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
};

export default withRouter(observer(App));