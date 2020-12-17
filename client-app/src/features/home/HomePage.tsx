import React, { useContext, Fragment, useEffect } from "react";
import {
  Container,
  Segment,
  Header,
  Button,
  Image,
  Menu,
  Grid,
  Item,
  Label,
} from "semantic-ui-react";
import { Link, NavLink } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoginForm from "../user/LoginForm";
import { RegisterForm } from "../user/RegisterForm";
import JoinGameForm from "../user/JoinGameForm";
import NavBar from "../nav/NavBar";
import Footer from "../footer/Footer";
import { observer } from "mobx-react-lite";

const HomePage = () => {
  const token = window.localStorage.getItem("jwt");
  const rootStore = useContext(RootStoreContext);
  const { isLoggedIn, user } = rootStore.userStore;
  const { predicate, setPredicate, gamesByDate } = rootStore.gameStore;
  const { openModal } = rootStore.modalStore;

  useEffect(() => {
    if (isLoggedIn) {
      setPredicate("isInGame", "true");
    } else {
      console.log("not logged in");
    }
  }, [isLoggedIn, setPredicate]);

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
              <Segment>
                <Grid>
                  <Grid.Column width={9}>
                    <Item.Group>
                      {gamesByDate.map(([group, games]) => (
                        <Fragment key={group}>
                          <Label size="large" color="blue">
                            {group}
                          </Label>
                          <Item.Group divided>
                            {games.map((game) => (
                              <Item key={game.id}>
                                <Item.Content>
                                  <Item.Header><Link to={`/games/${game.id}`}>{game.title}</Link></Item.Header>
                                  <Item.Description>
                                    {game.players.map((p) => (<Label key={p.userName}>{p.displayName}</Label>))}
                                    </Item.Description>
                                </Item.Content>
                              </Item>
                            ))}
                          </Item.Group>
                        </Fragment>
                      ))}
                    </Item.Group>
                  </Grid.Column>
                  <Grid.Column width={3}>
                    <Menu vertical>
                      <Menu.Item
                        active={predicate.has("isInGame")}
                        onClick={() => setPredicate("isInGame", "true")}
                        color={"blue"}
                        name={"all"}
                        content={"Games - I'm In"}
                      />
                      <Menu.Item
                        active={predicate.has("isHost")}
                        onClick={() => setPredicate("isHost", "true")}
                        color={"blue"}
                        name={"username"}
                        content={"Games - I'm Hosting"}
                      />
                    </Menu>
                  </Grid.Column>
                </Grid>
              </Segment>
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

export default observer(HomePage);
