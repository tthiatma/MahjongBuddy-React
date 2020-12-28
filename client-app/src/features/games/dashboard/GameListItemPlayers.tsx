import React from 'react';
import { List, Image, Popup } from 'semantic-ui-react';
import { IGamePlayer } from '../../../app/models/player';

interface IProps {
    players: IGamePlayer[]
}

const GameListItemPlayers : React.FC<IProps> = ({players}) => {  
    return (
      <List horizontal>
          {players.map(player => (
            <List.Item key={player.userName}>
              <Popup
                header={player.displayName}
                trigger={
                  <Image
                    size="mini"
                    circular
                    src={player.image || '/assets/user.png'}
                  />
                }
              />
            </List.Item>
            ))}
      </List>
    )
}

export default GameListItemPlayers;