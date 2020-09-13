import React from "react";
import { Container, Segment, Header, List } from "semantic-ui-react";

const RulesExtraPoints = () => {
  return (
    <Container>
      <Segment>
        <Header>Supported Extra Points</Header>
        <List bulleted className="ruleList">
          <List.Item>
            <Header size="medium">SelfPick - 1 pt</Header>
            <p>Win by picking your own winning tile</p>
          </List.Item>

          <List.Item>
            <Header size="medium">ConcealedHand - 1 pt</Header>
            <p>Win by not forming any chow/pong/kong with discarded tile</p>
          </List.Item>
          <List.Item>
            <Header size="medium">SeatWind - 1 pt</Header>
            <p>Win with meld of your own seat's wind</p>
          </List.Item>
          <List.Item>
            <Header size="medium">PrevailingWind - 1 pt</Header>
            <p>Win with meld of current round wind</p>
          </List.Item>

          <List.Item>
            <Header size="medium">RedDragon - 1 pt</Header>
            <p>Win with pong/kong of red dragon</p>
          </List.Item>
          <List.Item>
            <Header size="medium">GreenDragon - 1 pt</Header>
            <p>Win with pong/kong of green dragon - 1 pt</p>
          </List.Item>
          <List.Item>
            <Header size="medium">WhiteDragon - 1 pt</Header>
            <p>Win with pong/kong of white dragon</p>
          </List.Item>

          <List.Item>
            <Header size="medium">NoFlower - 1 pt</Header>
            <p>Win with not getting any flower</p>
          </List.Item>

          <List.Item>
            <Header size="medium">RomanFlower - 1 pt</Header>
            <p>Win with your seat flower</p>
          </List.Item>

          <List.Item>
            <Header size="medium">NumericFlower - 1 pt</Header>
            <p>Win with your seat flower</p>
          </List.Item>

          <List.Item>
            <Header size="medium">AllFourFlowerSameType - 1 pt</Header>
            <p>Win with 4 of same type of flower</p>
          </List.Item>

          <List.Item>
            <Header size="medium">WinOnLastTile - 1 pt</Header>
            <p>Win by picking the last tile of the round</p>
          </List.Item>
        </List>
      </Segment>
    </Container>
  );
};

export default RulesExtraPoints;
