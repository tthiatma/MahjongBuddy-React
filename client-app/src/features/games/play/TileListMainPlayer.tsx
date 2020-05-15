import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Droppable, Draggable } from "react-beautiful-dnd";

interface IProps{
  containerStyleName: string;
  tileStyleName: string;
  mainPlayerActiveTiles: IRoundTile[] | null;
  mainPlayerGraveYardTiles: IRoundTile[] | null;
  mainPlayerJustPickedTile: IRoundTile[] | null;
}

const mainPlayerTiles = {
  display: 'flex',
};

const mainPlayerGraveyard = {
  display: 'flex',
  marginRight: '5px'
}

const getListStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? 'lightblue' : 'lightgrey',
  display: 'flex'
});

const TileListMainPlayer: React.FC<IProps> = ({ containerStyleName, mainPlayerActiveTiles,mainPlayerGraveYardTiles, mainPlayerJustPickedTile }) => {
  return (
    <div style={mainPlayerTiles}>
      <div style={mainPlayerGraveyard} id="userGraveyard">
        {mainPlayerGraveYardTiles && mainPlayerGraveYardTiles
            .map((rt) => (
              <div
                key={rt.id}
                style={{
                  backgroundImage: `url(${rt.tile.imageSmall}`,
                }}
                className="flexTiles"
              />
            ))}
      </div>
      <Droppable droppableId="tile" direction="horizontal">
        {(provided, snapshot) => (
          <div
            ref={provided.innerRef}
            style={getListStyle(snapshot.isDraggingOver)}
            {...provided.droppableProps}
          >
            <div>
            {mainPlayerActiveTiles && mainPlayerActiveTiles
            .map((rt, index) => (                  
                    <Draggable draggableId={rt.id} index={index} key={rt.id}>
                      {(provided) => (
                        <div
                          ref={provided.innerRef}
                          {...provided.draggableProps}
                          {...provided.dragHandleProps}
                          className={containerStyleName}
                        >
                          <div
                            style={{
                              backgroundImage: `url(${rt.tile.imageSmall}`,
                            }}
                            className="flexTiles"
                          />
                        </div>
                      )}
                    </Draggable>
                  ))}
              {provided.placeholder}
            </div>
            <div id="userGraveyard">
              {mainPlayerJustPickedTile && mainPlayerJustPickedTile
              .map((rt, index) => (
                    <Draggable draggableId={rt.id} index={index} key={rt.id}>
                      {(provided) => (
                        <div
                          ref={provided.innerRef}
                          {...provided.draggableProps}
                          {...provided.dragHandleProps}
                          className={containerStyleName}
                        >
                          <div
                            style={{
                              backgroundImage: `url(${rt.tile.imageSmall}`,
                            }}
                            className="flexTiles"
                          />
                        </div>
                      )}
                    </Draggable>
                  ))}
              {provided.placeholder}
            </div>
          </div>
        )}
      </Droppable>
    </div>
  );
};
export default observer(TileListMainPlayer);
