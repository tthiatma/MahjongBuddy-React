import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";
import { TileStatus } from "../../../app/models/tileStatus";
import { Droppable, Draggable, DragDropContext } from "react-beautiful-dnd";
import { Container, Ref, Table } from "semantic-ui-react";

interface IProps {
  containerStyleName: string;
  tileStyleName: string;
  roundTiles: IRoundTile[];
}

const getListStyle = (isDraggingOver: boolean) => ({
   background: isDraggingOver ? 'lightblue' : 'lightgrey',
  display: 'flex',
  // padding: grid,
  overflow: 'auto',
});

const TileListOtherPlayer: React.FC<IProps> = ({
  containerStyleName,
  tileStyleName,
  roundTiles,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { selectedTile } = rootStore.roundStore;
  return (
      <Droppable droppableId="tile" direction="horizontal">
        {(provided, snapshot) => (
          // <Ref innerRef={provided.innerRef}>
          <div
            ref={provided.innerRef}
            style={getListStyle(snapshot.isDraggingOver)}
            {...provided.droppableProps}
          >
            {roundTiles &&
              roundTiles
                .filter((t) => t.status === TileStatus.UserActive)
                .map((rt, index) => (
                  <Draggable draggableId={rt.id} index={index} key={rt.id}>
                    {(provided) => (
                      // <Ref innerRef={provided.innerRef}>
                      <div
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}
                      >
                        <div
                          style={{
                            backgroundImage: `url(${rt.tile.imageSmall}`,
                          }}
                          className="flexTiles"
                        />
                      </div>
                      //</Ref>
                    )}
                  </Draggable>
                ))}
            {provided.placeholder}
          </div>
          // </Ref>
        )}
      </Droppable>
  );
};
export default observer(TileListOtherPlayer);
