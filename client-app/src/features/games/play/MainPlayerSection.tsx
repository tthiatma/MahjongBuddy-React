import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { Droppable, Draggable } from "react-beautiful-dnd";
import { IRoundPlayer } from "../../../app/models/round";
import { Grid } from "semantic-ui-react";
import PlayerAction from "./PlayerAction";
import PlayerStatus from "./PlayerStatus";

interface IProps {
  mainPlayer: IRoundPlayer | null;
  containerStyleName: string;
  tileStyleName: string;
  mainPlayerActiveTiles: IRoundTile[] | null;
  mainPlayerGraveYardTiles: IRoundTile[] | null;
  mainPlayerJustPickedTile: IRoundTile[] | null;
}

const getListStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? "lightblue" : "",
  // display: 'flex',
});

const MainPlayerSection: React.FC<IProps> = ({
  mainPlayer,
  containerStyleName,
  mainPlayerActiveTiles,
  mainPlayerGraveYardTiles,
  mainPlayerJustPickedTile,
}) => {
  return (
    <Grid id="mainPlayerTiles">
      <Grid.Column width={3}>
        <Grid.Row centered style={{ padding: "1px" }}>
          {mainPlayerGraveYardTiles &&
            mainPlayerGraveYardTiles.map((rt) => (
              <img key={rt.id} alt={rt.tile.title} src={rt.tile.imageSmall} />
            ))}
        </Grid.Row>
      </Grid.Column>
      <Grid.Column width={10}>
        <Grid.Row>
          <Droppable
            isDropDisabled={!mainPlayer!.isMyTurn || !mainPlayer!.mustThrow}
            droppableId="tile"
            direction="horizontal"
          >
            {(provided, snapshot) => (
              <span
                ref={provided.innerRef}
                style={getListStyle(snapshot.isDraggingOver)}
                {...provided.droppableProps}
              >
                <span>
                  {mainPlayerActiveTiles &&
                    mainPlayerActiveTiles.map((rt, index) => (
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
                  {mainPlayerJustPickedTile &&
                    mainPlayerJustPickedTile.map((rt, index) => (
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
        </Grid.Row>
        <Grid.Row>
          <div
            className="playerStatusContainer"
            {...(mainPlayer!.isMyTurn && {
              className: "playerTurn playerStatusContainer",
            })}
          >
            {mainPlayer && (
              <span
                style={{
                  width: "100%",
                  textAlign: "center",
                  lineHeight: "40px",
                }}
              >
                <PlayerStatus player={mainPlayer} />
              </span>
            )}
          </div>
        </Grid.Row>
      </Grid.Column>
      <Grid.Column width={3}>
        <PlayerAction />
      </Grid.Column>
    </Grid>
  );
};
export default observer(MainPlayerSection);
