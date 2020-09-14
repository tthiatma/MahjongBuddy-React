import React, { Fragment, useContext, useEffect } from "react";
import { observer } from "mobx-react-lite";
import {
  Route,
  withRouter,
  RouteComponentProps,
  Switch,
} from "react-router-dom";
import GameDashboard from "../../features/games/dashboard/GameDashboard";
import GameForm from "../../features/games/form/GameForm";
import GameOn from "../../features/games/play/GameOn";
import GameLobby from "../../features/games/lobby/GameLobby";
import NotFound from "./NotFound";
import { ToastContainer } from "react-toastify";
import { RootStoreContext } from "../stores/rootStore";
import { LoadingComponent } from "./LoadingComponent";
import ModalContainer from "../common/modals/ModalContainer";
import PrivateRoute from "./PrivateRoute";
import HomePage from "../../features/home/HomePage";
import PrivacyPolicy from "../../features/policy/PrivacyPolicy";
import ProfilePage from "../../features/profiles/ProfilePage";
import RegisterSuccess from "../../features/user/RegisterSuccess";
import VerifyEmail from "../../features/user/VerifyEmail";
import RulesPage from "../../features/rules/RulesPage";
import NavBar from "../../features/nav/NavBar";
import { Container } from "semantic-ui-react";
import Footer from "../../features/footer/Footer";

const App: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore = useContext(RootStoreContext);
  const {showNavBar} = rootStore.commonStore;
  const { setAppLoaded, token, appLoaded } = rootStore.commonStore;
  const { getUser } = rootStore.userStore;

  useEffect(() => {
    if (token) {
      getUser().finally(() => setAppLoaded());
    } else {
      setAppLoaded();
    }
  }, [getUser, setAppLoaded, token]);

  if (!appLoaded) return <LoadingComponent content="Loading App..." />;

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
            <Container  style={{minHeight: '90vh', paddingTop: showNavBar ? "5em" : "0" }}>
              <Switch>
                <Route exact path="/privacypolicy" component={PrivacyPolicy} />
                <Route exact path="/rules" component={RulesPage} />
                <PrivateRoute exact path="/games" component={GameDashboard} />
                <PrivateRoute
                  path="/games/:id/rounds/:roundId"
                  component={GameOn}
                />
                <PrivateRoute path="/games/:id" component={GameLobby} />
                <PrivateRoute
                  key={location.key}
                  path={["/createGame", "/manage/:id"]}
                  component={GameForm}
                />
                <PrivateRoute
                  path="/profile/:username"
                  component={ProfilePage}
                />
                <Route
                  path="/user/registerSuccess"
                  component={RegisterSuccess}
                />
                <Route path="/user/verifyEmail" component={VerifyEmail} />
                <Route component={NotFound} />
              </Switch>
            </Container>
            <Footer />
          </Fragment>
        )}
      />
    </Fragment>
  );
};

export default withRouter(observer(App));
