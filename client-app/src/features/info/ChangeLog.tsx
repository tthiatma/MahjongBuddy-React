import React from "react";
import {
  Container,
  Divider,
  Header,
  Icon,
  List,
  Popup,
  Segment,
} from "semantic-ui-react";

const ChangeLog = () => {
  return (
    <Container>
      <Segment>
        <Header textAlign="center" size="medium">
          Change Log
        </Header>
        <Divider></Divider>
        <List divided>
          <List.Item>
            <p>
              <strong>1/2/2021 update</strong>
            </p>
            <p>
              - Joining game now use 5 letters code instead of number eg. RAWRR
            </p>
            <p>
              - Added player's picture in the game. Go to "My Profile" (top
              right navigation bar), and add your photo if you want to display
              your picture in the game.
            </p>
            <p>
              - Added different color highlight in player's turn. Green when
              user's turn but have not thrown a tile and light green when player
              thrown a tile
            </p>
            <p>
              - Reduce delay after throwing tile to 1.5 seconds.
            </p>
            <p>
              - Allow more than one winners. Player can now declare win after
              another player declared win by dismissing the result and click on
              the win button.
            </p>
            <p>
              - Added <Icon name="book" /> in the game (located in the top right
              when playing) so that player can easily access list of supported
              hand types and supported extra points
            </p>
            <p>
              - When a round is finished, game host now has an option to either
              end the game or continue to next round. If host decided to end the
              game, there will be simple calculation on points
            </p>
            <p>
              - Added pop over(hover your mouse on the icon) to explain{" "}
              <Popup
                content="I threw the dice this round"
                trigger={<Icon name="cube" />}
              />{" "}
              and{" "}
              <Popup
                content="I'm the very first dealer of the game"
                trigger={<Icon name="star" />}
              />{" "}
              icon (I promise I will use better image when I start styling the
              game)
            </p>
            <p>
              - I've created Facebook and twitter page. Share your experience
              with me :){" "}
              <List horizontal>
                <List.Item
                  as="a"
                  target="_blank"
                  href="https://www.facebook.com/MahjongBuddyFB"
                >
                  <List.Content verticalAlign="top">
                    <Icon name="facebook" size="big" color="blue" />
                  </List.Content>
                </List.Item>
                <List.Item
                  as="a"
                  target="_blank"
                  href="https://www.twitter.com/MahjongBuddy"
                >
                  <List.Content verticalAlign="top">
                    <Icon name="twitter square" size="big" color="blue" />
                  </List.Content>
                </List.Item>
              </List>
            </p>
          </List.Item>
        </List>
      </Segment>
    </Container>
  );
};

export default ChangeLog;
