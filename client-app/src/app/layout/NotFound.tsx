import React, { Fragment } from "react";
import { Segment, Button, Header, Icon, Container } from "semantic-ui-react";
import { Link } from "react-router-dom";
import NavBar from "../../features/nav/NavBar";

const NotFound = () => {
  return (
    <Fragment>
      <NavBar />
      <Container style={{ paddingTop: "5em" }}>
        <Segment placeholder>
          <Header icon>
            <Icon name="search" />
            Oops - we've looked everywhere but couldn't find this.
          </Header>
          <Segment.Inline>
            <Button as={Link} to="/games" primary>
              Return to Games page
            </Button>
          </Segment.Inline>
        </Segment>
      </Container>
    </Fragment>
  );
};

export default NotFound;
