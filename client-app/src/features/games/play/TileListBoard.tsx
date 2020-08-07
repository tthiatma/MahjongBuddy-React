import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Transition, Image, Container } from "semantic-ui-react";

interface IProps {
  graveyardTiles: IRoundTile[];
  activeTile: IRoundTile;
  activeTileAnimation: string;
}

const TileListBoard: React.FC<IProps> = ({ graveyardTiles, activeTile, activeTileAnimation }) => {
  return (
    <Container>
      <div
        style={{
          display: "flex",
          minHeight: "300px",
          flexWrap: "wrap",
          alignItems: "flex-start",
          alignContent: "flex-start",
        }}
      >
        {graveyardTiles
          .sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter)
          .map((rt) => (
            <div style={{ paddingRight: "2px" }} key={rt.id}>
              <img src={rt.tile.imageSmall} alt="tile" />
            </div>
          ))}

        {activeTile && (
          <Transition transitionOnMount={true} animation={activeTileAnimation} duration={1000}>
            <Image src={activeTile.tile.image} alt="tile" />
          </Transition>
        )}
      </div>
    </Container>
  );
};

export default observer(TileListBoard);
