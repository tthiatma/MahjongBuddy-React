import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Droppable } from "react-beautiful-dnd";

interface IProps {
  roundTiles: IRoundTile[];
}

const getStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? 'lightblue' : 'lightgrey',
 display: 'flex',
 // padding: grid,
 overflow: 'auto',
 transitionDuration: `0.001s`
});

const TileListBoard: React.FC<IProps> = ({ roundTiles }) => {
  return (
    <Droppable droppableId="test">
      {(provided, snapshot) => (
        <div
          ref={provided.innerRef}
          style={getStyle(snapshot.isDraggingOver)}
          {...provided.droppableProps}
        >
          {roundTiles
            .sort((a, b) => a.boardGraveyardCounter - b.boardGraveyardCounter)
            .map((rt, index) => (
                <div key={rt.id}>
                <span>{rt.boardGraveyardCounter}</span>
                <img src={rt.tile.imageSmall} alt="tile" />
              </div>
            ))}
          {provided.placeholder}
        </div>
      )}
    </Droppable>
  );
};

export default observer(TileListBoard);
