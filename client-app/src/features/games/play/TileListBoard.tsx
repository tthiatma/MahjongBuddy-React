import React, { useContext, useEffect, useRef } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Transition, Image, Container } from "semantic-ui-react";
import useSound from "use-sound";
import tileThrowSfx from "../../../app/common/sounds/tileThrow.mp3";
import { RootStoreContext } from "../../../app/stores/rootStore";

interface IProps {
  graveyardTiles: IRoundTile[];
  activeTile: IRoundTile;
  activeTileAnimation: string;
}

const TileListBoard: React.FC<IProps> = ({
  graveyardTiles,
  activeTile,
  activeTileAnimation,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { gameSound } = rootStore.gameStore;
  const [playTileThrownSfx] = useSound(tileThrowSfx);
  const prevActiveTile = useRef<IRoundTile>();
  useEffect(() => {
    if (activeTile?.id && prevActiveTile.current?.id !== activeTile?.id && gameSound) {
       playTileThrownSfx();
    }
    prevActiveTile.current = activeTile;
  },[playTileThrownSfx, activeTile, gameSound]);
  
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
        {graveyardTiles &&
          graveyardTiles
            .sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter)
            .map((rt) => (
              <div style={{ paddingRight: "2px" }} key={rt.id}>
                <img src={rt.tile.imageSmall} alt="tile" />
              </div>
            ))}

        {activeTile && (
          <Transition
            key={activeTile.id}
            transitionOnMount={true}
            animation={activeTileAnimation}
            duration={1000}
          >
            <Image src={activeTile.tile.image} alt="tile" />
          </Transition>
        )}
      </div>
    </Container>
  );
};

export default observer(TileListBoard);
