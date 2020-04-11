import React from 'react'
import { Segment, Item, Header, Button, Image } from 'semantic-ui-react'
import { ITile } from '../../../app/models/tile';
import { observer } from 'mobx-react-lite';

const  TileListItem: React.FC = () => {
    return (
      <Item.Group>
        <Item>
          <Item.Content>
          <Image
            src="/assets/tiles/50px/face-down.png"
            alt="logo"            
          />
          </Item.Content>
        </Item>
      </Item.Group>
    );
}

export default observer(TileListItem)