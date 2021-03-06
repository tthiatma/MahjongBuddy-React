import React from "react";
import { Container, Segment, Header, List, Image } from "semantic-ui-react";

const RulesHands = () => {
  return (
    <Container>
      <Segment>
        <Header>Supported Hands Type</Header>
        <List bulleted className="ruleList">
          <List.Item>
            <Header size="medium">Chicken (雞糊) - 0 pt</Header>
            <p>Meld (winning hand) it’s a combination of pong/kong and chow that is NOT in a same suit.</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo2.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo3.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/man/man6.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/pin/pin7.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />

            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-east.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">Straight (平糊) - 1 pt</Header>
            <p>Every meld is a chow</p>

            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo2.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo3.png" />

            <Image src="/assets/tiles/50px/pin/pin4.png" />
            <Image src="/assets/tiles/50px/pin/pin5.png" />
            <Image src="/assets/tiles/50px/pin/pin6.png" />

            <Image src="/assets/tiles/50px/man/man6.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/pin/pin7.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">Triplets (對對糊) - 3 pt</Header>
            <p>Every meld is either a pong or kong</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />

            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">MixedOneSuit (清一色) - 3 pt</Header>
            <p>Mix of tiles from one suit and honor tiles</p>
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />

            <Image src="/assets/tiles/50px/pin/pin2.png" />
            <Image src="/assets/tiles/50px/pin/pin3.png" />
            <Image src="/assets/tiles/50px/pin/pin4.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">AllOneSuit (全清一色) - 7 pt</Header>
            <p>All tiles are from one suit</p>
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin2.png" />
            <Image src="/assets/tiles/50px/pin/pin3.png" />
            <Image src="/assets/tiles/50px/pin/pin4.png" />

            <Image src="/assets/tiles/50px/pin/pin2.png" />
            <Image src="/assets/tiles/50px/pin/pin3.png" />
            <Image src="/assets/tiles/50px/pin/pin4.png" />

            <Image src="/assets/tiles/50px/pin/pin5.png" />
            <Image src="/assets/tiles/50px/pin/pin6.png" />
            <Image src="/assets/tiles/50px/pin/pin7.png" />

            <Image src="/assets/tiles/50px/pin/pin9.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">SevenPairs (七對) - 6 pt</Header>
            <p>All tiles containing seven pairs</p>

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />

            <Image src="/assets/tiles/50px/pin/pin9.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo2.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo2.png" />

            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">ThirteenOrphans (十三腰) - 13 pt</Header>
            <p>One of each 1, 9, wind, and dragon, and a 14th tile forming a pair with any of these.</p>
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />

            <Image src="/assets/tiles/50px/man/man1.png" />
            <Image src="/assets/tiles/50px/man/man9.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />

            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />

            <Image src="/assets/tiles/50px/wind/wind-north.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">SmallDragon (小三完) - 5 pt</Header>
            <p>Melds of 2 dragons and a pair of the 3rd dragon</p>

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/man/man6.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">BigDragon (大三完) - 10 pt</Header>
            <p>Melds of all 3 dragons</p>
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/man/man6.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-green.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">SmallFourWind (小四喜) - 13 pt</Header>
            <p>Melds of 3 winds and a pair of the 4th wind</p>
            <Image src="/assets/tiles/50px/man/man6.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-east.png" />

            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />

            <Image src="/assets/tiles/50px/wind/wind-north.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">BigFourWind (大四喜) - 13 pt</Header>
            <p>Melds of all 4 winds</p>

            <Image src="/assets/tiles/50px/man/man8.png" />
            <Image src="/assets/tiles/50px/man/man8.png" />

            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-east.png" />
            <Image src="/assets/tiles/50px/wind/wind-east.png" />

            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />

            <Image src="/assets/tiles/50px/wind/wind-north.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">HiddenTreasure - 13 pt</Header>
            <p>Four pong and all concealed and win by Self-Drawn</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />

            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">AllTerminals (全腰九) - 13 pt</Header>
            <p>Hand containing Pongs/Kongs of Ones and Nines only</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin9.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />
            <Image src="/assets/tiles/50px/pin/pin9.png" />

            <Image src="/assets/tiles/50px/man/man1.png" />
            <Image src="/assets/tiles/50px/man/man1.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">MixedTerminals (半腰九) - 7 pt</Header>
            <p>Hand containing Pongs/Kongs of Ones and Nines and honor tiles</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo9.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
          </List.Item>

          <List.Item>
            <Header size="medium">AllHonors (全番子) - 10 pt</Header>
            <p>All honor tiles</p>
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />
            <Image src="/assets/tiles/50px/wind/wind-west.png" />

            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-white.png" />
      
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />
            <Image src="/assets/tiles/50px/dragon/dragon-chun.png" />

            <Image src="/assets/tiles/50px/wind/wind-north.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />
            <Image src="/assets/tiles/50px/wind/wind-north.png" />

            <Image src="/assets/tiles/50px/wind/wind-south.png" />
            <Image src="/assets/tiles/50px/wind/wind-south.png" />

          </List.Item>

          <List.Item>
            <Header size="medium">AllKongs - 13 pt</Header>
            <p>Hand containing 4 Kongs</p>
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo1.png" />

            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />
            <Image src="/assets/tiles/50px/bamboo/bamboo5.png" />

            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />
            <Image src="/assets/tiles/50px/pin/pin1.png" />

            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />
            <Image src="/assets/tiles/50px/pin/pin8.png" />

            <Image src="/assets/tiles/50px/man/man7.png" />
            <Image src="/assets/tiles/50px/man/man7.png" />
          </List.Item>

        </List>
      </Segment>
    </Container>
  );
};

export default RulesHands;
