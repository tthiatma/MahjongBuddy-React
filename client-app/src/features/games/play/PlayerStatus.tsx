import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { WindDirection } from "../../../app/models/windEnum";
import { Icon } from "semantic-ui-react";
import { IRoundOtherPlayer, IRoundPlayer } from "../../../app/models/player";

interface IProps {
  player: IRoundPlayer | IRoundOtherPlayer;
}

const PlayerStatus: React.FC<IProps> = ({ player }) => {
  return (
    <Fragment>
      {player.isInitialDealer && <Icon name="star" />}{" "}
      {player.isDealer && <Icon name="cube" />} {player.displayName} | Wind[
      {WindDirection[player.wind]}] | Flower[{player.wind + 1}] |{" "}
      <span
        style={{
          color:
            player.points > 0 ? "#75c775" : player.points < 0 ? "red" : "black",
        }}
      >
        {player.points > 0 && "+"} {player.points} pts
      </span>
    </Fragment>
  );
};

export default observer(PlayerStatus);
