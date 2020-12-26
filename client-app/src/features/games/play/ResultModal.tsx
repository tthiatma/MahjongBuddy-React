import React, { useContext, Fragment } from "react";
import { observer } from "mobx-react-lite";
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

  let winners: IRoundResult[] | null = null;
  let losers: IRoundResult[] | null = null;
  let tiePlayers: IRoundResult[] | null = null;
  let groupedLosers;

  const roundResults = round!.roundResults;
  const isHost = getMainUser?.isHost;
  const activeBoardTile = boardActiveTile;

  const groupLosersByName = (losers: IRoundResult[]) => {
    return Object.entries(
      losers.reduce((llist, l) => {
        llist[l.displayName] = llist[l.displayName]
          ? [...llist[l.displayName], l]
          : [l];
        return llist;
      }, {} as { [key: string]: IRoundResult[] })
    );
  };

  if (roundResults) {
    winners = roundResults?.filter((r) => r.playResult === PlayResult.Win)!;
    losers = roundResults?.filter((r) => r.playResult === PlayResult.Lost);
    tiePlayers = roundResults!.filter((r) => r.playResult === PlayResult.Tie);
    groupedLosers = groupLosersByName(losers);
  }

  return (
    <Modal closeIcon open={showResult} onClose={closeResultModal} size="small">
      <Header icon="bullhorn" content="Result" />

      <Modal.Content>
        {round?.isTied && <h3>booooo it's a tie. nobody win</h3>}

        {winners && (
          <Fragment>
            <h3>
              {winners.map((w) => (
                <Fragment>
                  Winner : {w.displayName}: {w.points} pts
                  <ul>
                    {w.roundResultHands.map((h, i) => (
                      <li key={i}>
                        {h.name} : {h.point}
                      </li>
                    ))}
                    {w.roundResultExtraPoints.map((e, i) => (
                      <li key={i}>
                        {e.name} : {e.point}
                      </li>
                    ))}
                  </ul>
                  <div className="flexTilesContainer">
                    {w.playerTiles.sort(sortTiles).map((rt) => (
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
                    {!w.roundResultExtraPoints.some(
                      (ep) => ep.extraPoint === ExtraPoint.SelfPick
                    ) &&
                      activeBoardTile && (
                        <div
                          style={{
                            backgroundImage: `url(${
                              activeBoardTile!.tile.imageSmall
                            }`,
                          }}
                          className="flexTilesSmall justPickedTile"
                        />
                      )}
                  </div>
                </Fragment>
              ))}
            </h3>
          </Fragment>
        )}
        {groupedLosers && (
          <h3>
            {groupedLosers.map(([lname, lostList]) => (
              <Fragment key={lname}>
                <ul>
                  {lostList.map((l) => (
                    <li>
                      Loser : {l.displayName}: {l.points}
                    </li>
                  ))}
                  {lostList[0].playerTiles!.sort(sortTiles).map((rt) => (
                    <img key={rt.id} src={rt.tile.imageSmall} alt="tile" />
                  ))}
                </ul>
              </Fragment>
            ))}
          </h3>
        )}
        {tiePlayers && (
          <h3>
            <ul>
              {tiePlayers.map((p) => (
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
          </h3>
        )}
      </Modal.Content>
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
