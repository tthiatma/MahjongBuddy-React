import React, { useContext, Fragment } from "react";
import {
  Container,
  Segment,
  Header,
  Button,
  Image,
  Message,
  Icon,
} from "semantic-ui-react";
import { NavLink } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoginForm from "../user/LoginForm";
import { RegisterForm } from "../user/RegisterForm";
import JoinGameForm from "../user/JoinGameForm";
import NavBar from "../nav/NavBar";
import Footer from "../footer/Footer";

const HomePage = () => {
  const token = window.localStorage.getItem("jwt");
  const rootStore = useContext(RootStoreContext);
  const { isLoggedIn, user } = rootStore.userStore;
  const { openModal } = rootStore.modalStore;

  return (
    <Fragment>
      <Segment
        inverted
        textAlign="center"
        vertical
        className="masthead mainContent"
      >
        <NavBar />
        <Container text>
          <Message icon>
            <Icon name="archive" />
            <Message.Content>
              <Message.Header>Game restart and update</Message.Header>
              <Message.List>
                <Message.Item>
                  There will be maintenance on <strong>January 4th, 2021 at 11:00 PM - 11:30 MST</strong>.
                </Message.Item>
                <Message.Item>
                  If you are playing on above day, please finish your game
                  before the maintenance time to avoid losing your game
                  progress
                </Message.Item>
              </Message.List>
            </Message.Content>
          </Message>
          <Header as="h1" inverted>
            <Image
              size="massive"
              src="/assets/logo.png"
              alt="logo"
              style={{ marginBottom: 12 }}
            />
            MahjongBuddy
          </Header>
          {isLoggedIn && user && token ? (
            <Fragment>
              <Header
                as="h2"
                inverted
                content={`Welcome back ${user.displayName}`}
              />
              <Button
                onClick={() => openModal(<JoinGameForm />)}
                size="huge"
                inverted
              >
                Join Game
              </Button>
              <Button
                as={NavLink}
                size="huge"
                inverted
                to="/createGame"
                content="Create Game"
              />{" "}
            </Fragment>
          ) : (
            <Fragment>
              <Header as="h2" inverted content={`Welcome to MahjongBuddy`} />
              <Button
                onClick={() => openModal(<LoginForm />)}
                size="huge"
                inverted
              >
                Login
              </Button>
              <Button
                onClick={() => openModal(<RegisterForm />)}
                size="huge"
                inverted
              >
                Register
              </Button>
            </Fragment>
          )}
        </Container>
      </Segment>
      <Footer />
    </Fragment>
  );
};

export default HomePage;
