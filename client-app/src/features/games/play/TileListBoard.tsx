import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Droppable } from "react-beautiful-dnd";

interface IProps {
  graveyardTiles: IRoundTile[];
  activeTile: IRoundTile;
}

const getStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? 'lightblue' : 'lightgrey',
 display: 'flex',
 // padding: grid,
 overflow: 'auto',
 transitionDuration: `0.001s`
});

const TileListBoard: React.FC<IProps> = ({ graveyardTiles, activeTile }) => {
  return (
    <div>
      <div style={{ display: "flex", minHeight:"350px", flexWrap:'wrap', alignItems:'flex-start', alignContent: 'flex-start' }}>
        {graveyardTiles
          .sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter)
          .map((rt, index) => (
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
