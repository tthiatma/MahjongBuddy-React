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
              <strong>1/4/2021 update</strong>
            </p>
            <p>
              - Joining game now use 5 letters code instead of number eg. RAWRR.
            </p>
            <p>
              - Added player's picture in the game. Go to "My Profile" (top
              right navigation bar) to add/change your photo and set it as main picture.
            </p>
            <p>
              - Added different color highlight in player's turn. Green when
              it's player's turn but have not thrown a tile, and light green when player has
              thrown a tile.
            </p>
            <p>
              - Reduce delay after throwing tile to 1.5 seconds.
            </p>
            <p>
              - Allow more than one winners. Player can now declare win after
              another player declared win.
            </p>
            <p>
              - Added <Icon name="book" /> in the game (located in the top right
              when playing) so that player can easily access list of supported
              hand types and supported extra points.
            </p>
            <p>
              - When a round is finished, game host now has an option to either
              end the game or continue to the next round. If game host decided to end the
              game, there will be a points calculation.
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
              game).
            </p>
            <p>
              - Added min and max point information in the game. Limited the max point value to be 0-100 when creating a game.   
            </p>
            <p>
              - Added sound effect when hovering on tile and when there's a new tile thrown to board (more sound effects are coming in the next update). Also added <Icon name="volume up" /> icon in the game to turn on/off the sound.   
            </p>
            <p>
              - I have created Facebook and twitter page. Share your experience
              with me {" "}
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
              or email me directly at <a href="mailto:info@mahjongbuddy.com">info@mahjongbuddy.com</a> for your feedback.
            </p>
          </List.Item>
        </List>
      </Segment>
    </Container>
  );
};

export default ChangeLog;
