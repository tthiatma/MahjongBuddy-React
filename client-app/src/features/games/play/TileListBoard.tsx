import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";

interface IProps {
  graveyardTiles: IRoundTile[];
  activeTile: IRoundTile;
}


const TileListBoard: React.FC<IProps> = ({ graveyardTiles, activeTile }) => {
  return (
    <div>
      <div style={{ display: "flex", minHeight:"350px", flexWrap:'wrap', alignItems:'flex-start', alignContent: 'flex-start' }}>
        {graveyardTiles
          .sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter)
          .map((rt) => (
            <div key={rt.id}>
              <img src={rt.tile.imageSmall} alt="tile" />
            </div>
          ))}
      </div>
      <div style={{ display: "flex", justifyContent:'center' }}>
        {activeTile && (
          <div>
            <img src={activeTile.tile.imageSmall} alt="tile" />
          </div>
        )}
      </div>
    </div>
  );
};

export default observer(TileListBoard);
