import React, { useContext, Fragment, useState } from "react";
import { observer } from "mobx-react-lite";
import { IRoundResult } from "../../../app/models/round";
import { Modal, Header, Button, Icon, Confirm } from "semantic-ui-react";
import { sortTiles } from "../../../app/common/util/util";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { TileStatus } from "../../../app/models/tileStatus";
import { ExtraPoint } from "../../../app/models/extraPointEnum";
import { PlayResult } from "../../../app/models/playResultEnum";

const ResultModal: React.FC = () => {
  const [confirmOpen, setConfirmOpen] = useState(false);  
  const handleCancel = () => setConfirmOpen(false);
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
    losers = roundResults?.filter((r) => r.playResult === PlayResult.Lost || r.playResult === PlayResult.LostWithPenalty);
    tiePlayers = roundResults!.filter((r) => r.playResult === PlayResult.Tie);
    groupedLosers = groupLosersByName(losers);
  }

  return (
    <Fragment>
      <Modal closeIcon open={showResult} onClose={closeResultModal} size="small">
      <Header icon="bullhorn" content="Result" />

      <Modal.Content>
        {round?.isTied && <h3>booooo it's a tie. nobody win</h3>}

        {winners && (
          <Fragment>
            <h3>
              {winners.map((w) => (
                <Fragment key={`winner-${w.userName}`}>
                  {`${w.displayName} ${PlayResult[w.playResult]} ${w.points} pts`}
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
                    {w.playerTiles.slice().sort(sortTiles).map((rt) => (
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
                  {lostList.map((l,i) => (
                    <li key={`lost-${i}-${l.userName}`}>
                      {`${l.displayName} ${PlayResult[l.playResult]} ${l.points} pts`}
                    </li>
                  ))}
                  {lostList[0].playerTiles!.slice().sort(sortTiles).map((rt) => (
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
                  {p.playerTiles.slice().sort(sortTiles).map((rt) => (
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
          <Button color="red" onClick={ () => setConfirmOpen(true)} inverted>
            <Icon name="handshake" /> End Game
          </Button>
          <Button color="blue" onClick={startRound} inverted>
            <Icon name="play" /> Next Round
          </Button>
        </Modal.Actions>
      )}
    </Modal>
      <Confirm
          open={confirmOpen}
          content='Are you sure you want to end the game?'
          onCancel={handleCancel}
          onConfirm={endGame}
        />
    </Fragment>
    
  );
};
export default observer(ResultModal);
