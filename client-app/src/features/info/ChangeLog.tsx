import React from "react";
import { Container, Divider, Header, List, Segment } from "semantic-ui-react";

const ChangeLog = () => {
  return (
    <Container>
      <Segment>
        <Header textAlign="center" size="medium">Change Log</Header>
        <Divider></Divider>
        <List divided>
            <List.Item>
                <p><strong>12/8/2020</strong></p>
                <p>- change1</p>
            </List.Item>
        </List>
      </Segment>
    </Container>
  );
};

export default ChangeLog;
