import React, { Fragment } from "react";
import { Segment, Button, Header, Icon } from "semantic-ui-react";
import { Link } from "react-router-dom";

const NotFound = () => {
  return (
    <Fragment>
      <Segment placeholder>
        <Header icon>
          <Icon name="search" />
          Oops - we've looked everywhere but couldn't find this.
        </Header>
        <Segment.Inline>
          <Button as={Link} to="/" primary>
            Return to home page
          </Button>
        </Segment.Inline>
      </Segment>
    </Fragment>
  );
};

export default NotFound;
