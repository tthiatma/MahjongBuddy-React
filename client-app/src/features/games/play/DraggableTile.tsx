import React from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import {
  Draggable,
  DroppableProvided,
  DroppableStateSnapshot,
} from "react-beautiful-dnd";
import { TileStatus } from "../../../app/models/tileStatus";
import useSound from 'use-sound';
import tileSelectSfx from '../../../app/common/sounds/tileSelect.mp3';

interface IProps {
  containerStyleName: string;
  tiles: IRoundTile[];
  droppableProvided: DroppableProvided;
  droppableSnapshot: DroppableStateSnapshot;
  doThrowTile: (tileId: string) => void;
}

const DraggableTile: React.FC<IProps> = ({
  tiles,
  droppableProvided,
  droppableSnapshot,
  containerStyleName,
  doThrowTile
}) => {
  const getListStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "lightblue" : "",
  });
  const [play, { stop }] = useSound(tileSelectSfx);

  return (
    <span
      {...droppableProvided.droppableProps}
      ref={droppableProvided.innerRef}
      style={getListStyle(droppableSnapshot.isDraggingOver)}
    >
      {tiles.map((rt: IRoundTile, index) => (
        <Draggable key={rt.id} draggableId={rt.id} index={index}>
          {(provided, snapshot) => (
            <span
              onDoubleClick={() => doThrowTile(rt.id)}
              ref={provided.innerRef}
              {...provided.draggableProps}
              {...provided.dragHandleProps}
              className={containerStyleName}
            >
              <div 
                onMouseEnter={() => play()}
                style={{
                  backgroundImage: `url(${rt.tile.image}`,
                }}
                className="flexTiles activeTiles"
                {...(rt.status === TileStatus.UserJustPicked && {
                  className: "flexTiles justPickedTile",
                })}
              />
            </span>
          )}
        </Draggable>
      ))}
      {droppableProvided.placeholder}
    </span>
  );
};
export default observer(DraggableTile);
