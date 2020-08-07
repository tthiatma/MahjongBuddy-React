import React, { useState, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Transition, Image, Container } from "semantic-ui-react";

interface IProps {
  graveyardTiles: IRoundTile[];
  activeTile: IRoundTile;
  activeTileAnimation: string;
}

const TileListBoard: React.FC<IProps> = ({ graveyardTiles, activeTile, activeTileAnimation }) => {
  const [visible, setVisible] = useState<boolean>(false);

  const sleep = (ms: number) => {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  useEffect(() => {
    setVisible(false);
    sleep(1).then(() => {
      setVisible(true);
    })
  }, [activeTile]);

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
          <Transition visible={visible} animation={activeTileAnimation} duration={1000}>
            <Image src={activeTile.tile.image} alt="tile" />
          </Transition>
        )}
      </div>
    </Container>
  );
};

export default observer(TileListBoard);
