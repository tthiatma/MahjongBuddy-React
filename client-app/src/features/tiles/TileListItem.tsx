import React from 'react'
import { Segment, Item, Header, Button, Image } from 'semantic-ui-react'
import { ITile } from '../../app/models/tile';
import { observer } from 'mobx-react-lite';

const tileImageStyle = {
  position: 'absolute',
  bottom: '5%',
  left: '5%',
  width: '100%',
  height: 'auto',
  color: 'white'
};

const  TileListItem: React.FC<{tile: ITile}> = ({tile}) => {
    return (
      <Item.Group>
        <Item>
          <Item.Content>
            <p>{tile.title}</p>
          </Item.Content>
        </Item>
      </Item.Group>
    );
}

export default observer(TileListItem)