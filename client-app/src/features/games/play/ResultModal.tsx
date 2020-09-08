import React, { useContext, Fragment } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { IRoundResult, IRoundPlayer } from "../../../app/models/round";
import { Modal, Header, Button, Icon } from "semantic-ui-react";
import { sortTiles } from "../../../app/common/util/util";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { TileStatus } from "../../../app/models/tileStatus";
import { ExtraPoint } from "../../../app/models/extraPointEnum";

interface IProps {
  roundResults: IRoundResult[] | null;
  roundTiles: IRoundTile[] | null;
  isHost: boolean;
  roundPlayers: IRoundPlayer[] | null;
}

const ResultModal: React.FC<IProps> = ({
  roundResults,
  roundTiles,
  isHost,
  roundPlayers,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { startRound } = rootStore.hubStore;
  const { showResult, closeModal } = rootStore.roundStore;

  let winner: IRoundResult | null = null;
  let losers: IRoundResult[] | null = null;
  let tiePlayers: IRoundPlayer[] | undefined = undefined;
  let winnerTiles: IRoundTile[] | null = null;
  let boardTile: IRoundTile | null = null;

  if (roundResults) {
    winner = roundResults?.find((r) => r.isWinner === true)!;
    losers = roundResults!.filter((r) => r.isWinner === false);
    if (!winner) tiePlayers = roundPlayers!;

    if (losers && losers.length === 1) {
      tiePlayers = roundPlayers?.filter(
        (p) =>
          p.userName !== winner?.userName && p.userName !== losers![0].userName
      );
    }
    winnerTiles = roundTiles!
      .filter((t) => t.owner === winner?.userName)!
      .sort(sortTiles);

    boardTile = roundTiles!.find((t) => t.status === TileStatus.BoardActive)!;
  }

  return (
    <Modal open={showResult} onClose={closeModal} size="small">
      <Header icon="bullhorn" content="Result" />

      {roundResults !== null && roundResults.length > 0 ? (
        <Modal.Content>
          <h3>
            Winner : {winner?.userName}: {winner?.pointsResult} pts
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
            {!winner!.roundResultExtraPoints.some((ep) => ep.extraPoint === ExtraPoint.SelfPick) && boardTile && (
              <div
                style={{
                  backgroundImage: `url(${boardTile.tile.imageSmall}`,
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
                      Loser : {l.userName}: {l.pointsResult}
                    </li>
                    {roundTiles!
                      .filter((t) => t.owner === l.userName)!
                      .sort(sortTiles)
                      .map((rt) => (
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
                    <li>{p.userName}</li>
                    {roundTiles!
                      .filter((t) => t.owner === p.userName)!
                      .sort(sortTiles)
                      .map((rt) => (
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
                    <li>{p.userName}</li>
                    {roundTiles!
                      .filter((t) => t.owner === p.userName)!
                      .sort(sortTiles)
                      .map((rt) => (
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
      <Modal.Actions>
        <Button color="green" onClick={closeModal} inverted>
          <Icon name="checkmark" /> Got it
        </Button>
        {isHost && (
          <Button color="blue" onClick={startRound} inverted>
            <Icon name="play" /> Next Round
          </Button>
        )}
      </Modal.Actions>
    </Modal>
  );
};
export default observer(ResultModal);
