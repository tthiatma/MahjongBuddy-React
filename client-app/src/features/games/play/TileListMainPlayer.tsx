import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Droppable, Draggable } from "react-beautiful-dnd";
import { IRoundPlayer } from "../../../app/models/round";

interface IProps{
  mainPlayer: IRoundPlayer | null;
  containerStyleName: string;
  tileStyleName: string;
  mainPlayerActiveTiles: IRoundTile[] | null;
  mainPlayerGraveYardTiles: IRoundTile[] | null;
  mainPlayerJustPickedTile: IRoundTile[] | null;
}

const mainPlayerTiles = {
  // display: 'flex',
};

const mainPlayerGraveyard = {
  // display: 'flex',
  marginRight: '10px',
}

const getListStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? 'lightblue' : '',
  // display: 'flex',
});

const TileListMainPlayer: React.FC<IProps> = ({ mainPlayer, containerStyleName, mainPlayerActiveTiles,mainPlayerGraveYardTiles, mainPlayerJustPickedTile }) => {
  return (
    <div id='mainPlayerTiles' style={mainPlayerTiles}>
      <span style={mainPlayerGraveyard} id="userGraveyard">
        {mainPlayerGraveYardTiles && mainPlayerGraveYardTiles
            .map((rt) => (
              <div key={rt.id} className='tileHorizontalContainer'>
                <div                  
                  style={{
                    backgroundImage: `url(${rt.tile.image}`,
                  }}
                  className="flexTiles"
                />
              </div>
            ))}
      </span>
      <Droppable
      isDropDisabled={!mainPlayer!.isMyTurn || !mainPlayer!.mustThrow}
      droppableId="tile"
       direction="horizontal">
        {(provided, snapshot) => (
          <span
            ref={provided.innerRef}
            style={getListStyle(snapshot.isDraggingOver)}
            {...provided.droppableProps}
          >
            <span>
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
                              backgroundImage: `url(${rt.tile.image}`,
                            }}
                            className="flexTiles"
                          />
                        </div>
                      )}
                    </Draggable>
                  ))}
              {provided.placeholder}
            </span>
            <span id="userJustPicked">
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
                              backgroundImage: `url(${rt.tile.image}`,
                            }}
                            className="flexTiles justPickedTile"
                          />
                        </div>
                      )}
                    </Draggable>
                  ))}
              {provided.placeholder}
            </span>
          </span>
        )}
      </Droppable>
    </div>
  );
};
export default observer(TileListMainPlayer);
