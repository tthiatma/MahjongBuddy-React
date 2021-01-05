import React, { useContext, Fragment } from "react";
import {
  Container,
  Segment,
  Header,
  Button,
  Image,
  List,
  Icon,
  Message,
} from "semantic-ui-react";
import { NavLink } from "react-router-dom";
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
  const { openModal } = rootStore.modalStore;

  // useEffect(() => {
  //   if (isLoggedIn) {
  //     setPredicate("isInGame", "true");
  //   }
  // }, [isLoggedIn, setPredicate]);

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
          <Message size="small" color="teal" info compact>
            <Message.Content>
              <p>New Update! Go to <NavLink to="/changelog">Change Log</NavLink> to see what's new</p>
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
          <div>
            <List horizontal>
              <List.Item
                as="a"
                target="_blank"
                href="https://www.facebook.com/MahjongBuddyFB"
              >
                <List.Content verticalAlign="top">
                  <Icon name="facebook" size="huge" color="blue" />
                </List.Content>
              </List.Item>
              <List.Item
                as="a"
                target="_blank"
                href="https://www.twitter.com/MahjongBuddy"
              >
                <List.Content verticalAlign="top">
                  <Icon name="twitter square" size="huge" color="blue" />
                </List.Content>
              </List.Item>
            </List>
          </div>
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
              {/* <Segment
                attached="top"
                inverted
                color="teal"
                style={{ border: "none" }}
              >
                <Header textAlign="center">Recent Games</Header>
              </Segment>
              <Segment attached>
                <Table basic="very">
                  <Table.Header>
                    <Table.Row>
                      <Table.HeaderCell>Code</Table.HeaderCell>
                      <Table.HeaderCell>Date</Table.HeaderCell>
                      <Table.HeaderCell>Title</Table.HeaderCell>
                      <Table.HeaderCell>Players</Table.HeaderCell>
                      <Table.HeaderCell />
                    </Table.Row>
                  </Table.Header>
                  <Table.Body>
                    {gamesByDate.map((game) => (
                      <Table.Row key={game.id}>
                        <Table.Cell>{game.code}</Table.Cell>
                        <Table.Cell>{format(new Date(game.date), "MMM do, yyyy")}</Table.Cell>
                        <Table.Cell>{game.title} </Table.Cell>
                        <Table.Cell>
                          {game.gamePlayers.map((p) => (
                            <Label key={p.userName}>{p.displayName}</Label>
                          ))}
                        </Table.Cell>
                        <Table.Cell>
                          <Button
                            color="blue"
                            size="mini"
                            as={Link}
                            to={`/games/${game.code}`}
                          >
                            Go
                          </Button>
                        </Table.Cell>
                      </Table.Row>
                    ))}
                  </Table.Body>
                </Table>
              </Segment> */}
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
