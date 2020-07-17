import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundPlayer } from "../../../app/models/round";
import { WindDirection } from "../../../app/models/windEnum";
import { Icon } from "semantic-ui-react";

interface IProps {
    player: IRoundPlayer;  }

const PlayerStatus: React.FC<IProps> = ({
    player
  }) => {
    return(
        <Fragment>
           {player.isInitialDealer && <Icon name="star" />} {player.isDealer && <Icon name="cube" />} {player.userName} | Wind[{WindDirection[player.wind]}] | Flower[{player.wind + 1}] | <span style={{color: (player.points > 0) ? '#75c775' : (player.points < 0) ? 'red' : 'black'}}>{player.points > 0 && '+'} {player.points} pts</span>
        </Fragment>
    )
  };

  export default observer(PlayerStatus);
