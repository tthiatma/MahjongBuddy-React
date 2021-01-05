import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { WindDirection } from "../../../app/models/windEnum";
import { Icon, Popup } from "semantic-ui-react";
import { IRoundOtherPlayer, IRoundPlayer } from "../../../app/models/player";

interface IProps {
  player: IRoundPlayer | IRoundOtherPlayer;
}

const PlayerStatus: React.FC<IProps> = ({ player }) => {
  return (
    <Fragment>
      {player && (
        <Fragment>
          {player.isInitialDealer && <Popup content="I'm the very first dealer of the game" trigger={<Icon name="star" />}/>}{" "}
          {player.isDealer && <Popup content="I threw the dice this round" trigger={<Icon name="cube" />}/>} {player.displayName} | Wind[
          {WindDirection[player.wind]}] | Flower[{player.wind + 1}] |{" "}
          <span
            style={{
              color:
                player.points > 0
                  ? "#75c775"
                  : player.points < 0
                  ? "red"
                  : "black",
            }}
          >
            <strong>{player.points > 0 && "+"} {player.points} pts</strong>
          </span>
        </Fragment>
      )}
    </Fragment>
  );
};

export default observer(PlayerStatus);
