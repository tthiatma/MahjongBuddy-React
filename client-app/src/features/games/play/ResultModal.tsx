import React, { useContext, Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { IRoundResult } from "../../../app/models/round";
import { Modal, Header, Button, Icon } from "semantic-ui-react";
import { sortTiles } from "../../../app/common/util/util";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { TileStatus } from "../../../app/models/tileStatus";
import { ExtraPoint } from "../../../app/models/extraPointEnum";
import { PlayResult } from "../../../app/models/playResultEnum";

const ResultModal: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { startRound, endGame } = rootStore.hubStore;
  const { getMainUser, gameIsOver } = rootStore.gameStore;
  const {
    round,
    showResult,
    closeResultModal,
    boardActiveTile,
  } = rootStore.roundStore;

  let winner: IRoundResult | null = null;
  let losers: IRoundResult[] | null = null;
  let tiePlayers: IRoundResult[] | null = null;
  let winnerTiles: IRoundTile[] | null = null;

  const roundResults = round!.roundResults;
  const isHost = getMainUser?.isHost;
  const activeBoardTile = boardActiveTile;

  if (roundResults) {
    winner = roundResults?.find((r) => r.playResult === PlayResult.Win)!;
    losers = roundResults!.filter((r) => r.playResult === PlayResult.Lost);
    tiePlayers = roundResults!.filter((r) => r.playResult === PlayResult.Tie);
    winnerTiles = winner?.playerTiles.sort(sortTiles);
  }

  return (
    <Modal closeIcon open={showResult} onClose={closeResultModal} size="small">
      <Header icon="bullhorn" content="Result" />

      {roundResults !== null && roundResults.length > 0 ? (
        <Modal.Content>
          <h3>
            Winner : {winner?.displayName}: {winner?.points} pts
            <ul>
              {winner?.roundResultHands.map((h, i) => (
                <li key={i}>
                  {h.name} : {h.point}
                </li>
              ))}
              {winner?.roundResultExtraPoints.map((e, i) => (
                <li key={i}>
                  {e.name} : {e.point}
                </li>
              ))}
            </ul>
          </h3>
          <div className="flexTilesContainer">
            {winnerTiles &&
              winnerTiles.map((rt) => (
                <div
                  key={rt.id}
                  style={{
                    backgroundImage: `url(${rt.tile.imageSmall}`,
                  }}
                  className={
                    rt.status === TileStatus.UserJustPicked
                      ? "flexTilesSmall justPickedTile"
                      : "flexTilesSmall"
                  }
                />
              ))}
            {!winner!.roundResultExtraPoints.some(
              (ep) => ep.extraPoint === ExtraPoint.SelfPick
            ) &&
              activeBoardTile && (
                <div
                  style={{
                    backgroundImage: `url(${activeBoardTile!.tile.imageSmall}`,
                  }}
                  className="flexTilesSmall justPickedTile"
                />
              )}
          </div>
          <h3>
            {losers && (
              <ul>
                {losers.map((l, i) => (
                  <Fragment key={l.userName}>
                    <li>
                      Loser : {l.displayName}: {l.points}
                    </li>
                    {l.playerTiles!.sort(sortTiles).map((rt) => (
                      <img key={rt.id} src={rt.tile.imageSmall} alt="tile" />
                    ))}
                  </Fragment>
                ))}
              </ul>
            )}
            {tiePlayers && (
              <ul>
                {tiePlayers.map((p, i) => (
                  <Fragment key={p.userName}>
                    <li>{p.displayName}</li>
                    {p.playerTiles.sort(sortTiles).map((rt) => (
                      <img key={rt.id} src={rt.tile.imageSmall} alt="tile" />
                    ))}
                    <br />
                    <br />
                  </Fragment>
                ))}
              </ul>
            )}
          </h3>
        </Modal.Content>
      ) : (
        <Modal.Content>
          <h3>
            booooo it's a tie. nobody win
            {tiePlayers && (
              <ul>
                {tiePlayers.map((p, i) => (
                  <Fragment key={p.userName}>
                    <li>{p.displayName}</li>
                    {p.playerTiles.sort(sortTiles).map((rt) => (
                      <img key={rt.id} src={rt.tile.imageSmall} alt="tile" />
                    ))}
                    <br />
                    <br />
                  </Fragment>
                ))}
              </ul>
            )}
          </h3>
        </Modal.Content>
      )}
      {!gameIsOver && isHost && (
        <Modal.Actions>
          <Button color="red" onClick={endGame} inverted>
            <Icon name="handshake" /> End Game
          </Button>
          <Button color="blue" onClick={startRound} inverted>
              <Icon name="play" /> Next Round
            </Button>
        </Modal.Actions>
      )}
    </Modal>
  );
};
export default observer(ResultModal);
