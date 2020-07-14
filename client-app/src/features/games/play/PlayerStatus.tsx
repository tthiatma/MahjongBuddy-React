import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundPlayer } from "../../../app/models/round";
import { WindDirection } from "../../../app/models/windEnum";

interface IProps {
    player: IRoundPlayer;  }

const PlayerStatus: React.FC<IProps> = ({
    player
  }) => {
    return(
        <Fragment>
            {player.userName} | Wind[{WindDirection[player.wind]}] | Flower[{player.wind + 1}] | <span style={{color: (player.points > 0) ? 'green' : (player.points < 0) ? 'red' : 'black'}}>{player.points} pts</span>
        </Fragment>
    )
  };

  export default observer(PlayerStatus);
