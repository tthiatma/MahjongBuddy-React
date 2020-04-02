import React, { Fragment, useContext, useEffect } from "react";
import { Container } from "semantic-ui-react";
import NavBar from "../../features/nav/NavBar";
import { observer } from "mobx-react-lite";
import { Route, withRouter, RouteComponentProps, Switch } from "react-router-dom";
import GameDashboard from "../../features/games/dashboard/GameDashboard";
import GameForm from "../../features/games/form/GameForm";
import GameOn from "../../features/games/play/GameOn";
import GameLobby from "../../features/games/lobby/GameLobby";
import NotFound from "./NotFound";
import { LoginForm } from "../../features/user/LoginForm";
import {ToastContainer} from 'react-toastify';
import { RootStoreContext } from "../stores/rootStore";
import { LoadingComponent } from "./LoadingComponent";
import ModalContainer from "../common/modals/ModalContainer";
import PrivateRoute from "./PrivateRoute";
import HomePage from "../../features/home/HomePage";

const App: React.FC<RouteComponentProps> = ({ location }) => {

  const rootStore = useContext(RootStoreContext);
  const {setAppLoaded, token, appLoaded} = rootStore.commonStore;
  const {getUser} = rootStore.userStore;

  useEffect(() => {
    if(token){
      getUser().finally(() => setAppLoaded())
    } else {
      setAppLoaded()
    }
  }, [getUser, setAppLoaded, token])

  if(!appLoaded) return <LoadingComponent content='Loading App...' />

  return (
    <Fragment>
      <ModalContainer />
      <ToastContainer position="bottom-right" />
      <Route exact path="/" component={HomePage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <Fragment>
            <NavBar />
            <Container style={{ marginTop: "5em" }}>
              <Switch>
                <PrivateRoute exact path="/games" component={GameDashboard} />
                <PrivateRoute path="/games/:id" component={GameOn} />
                <PrivateRoute path="/lobby/:id" component={GameLobby} />
                <PrivateRoute path="/login" component={LoginForm} />
                <PrivateRoute
                  key={location.key}
                  path={["/createGame", "/manage/:id"]}
                  component={GameForm}
                />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
};

export default withRouter(observer(App));