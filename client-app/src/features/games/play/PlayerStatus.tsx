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
            {player.userName} | {WindDirection[player.wind]} | {player.points} pts
        </Fragment>
    )
  };

  export default observer(PlayerStatus);
