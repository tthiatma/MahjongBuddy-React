import React, { useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile } from "../../../app/models/tile";
import { IRoundResult } from "../../../app/models/round";
import { Modal, Header, Button, Icon } from "semantic-ui-react";
import { sortTiles } from "../../../app/common/util/util";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction } from "mobx";
import { TileStatus } from "../../../app/models/tileStatus";

interface IProps {
  roundResults: IRoundResult[] | null;
  roundTiles: IRoundTile[] | null;
}

const ResultModal: React.FC<IProps> = ({
  roundResults,
  roundTiles,
}) => {
        const rootStore = useContext(RootStoreContext);
        const {startRound} = rootStore.hubStore;
        const {showResult} = rootStore.roundStore;

        let winner: IRoundResult | null = null;
        let losers: IRoundResult[] | null = null;
        let winnerTiles: IRoundTile[] | null = null;
        let boardTile: IRoundTile | null = null;

        const closeModal = () => {
           runInAction(() => {
               rootStore.roundStore.showResult = false;
           }) 
        }
        if (roundResults) {
          winner = roundResults?.find((r) => r.isWinner === true)!;
          losers = roundResults!.filter((r) => r.isWinner === false);
          winnerTiles = roundTiles!.filter((t) => t.owner === winner?.userName)!
            .sort(sortTiles);
          boardTile = roundTiles!.find((t) => t.status === TileStatus.BoardActive)!;
        }

        return (
          <Modal
            open={showResult}
            onClose={closeModal}
            size="small"
          >
            <Header icon="bullhorn" content="Result" />

            {roundResults !== null && roundResults.length > 0   ? 
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
                                className={rt.status === TileStatus.UserJustPicked ? "flexTiles justPickedTile" : "flexTiles"}  
                              />
                            ))}
                            {losers!.length === 1 && boardTile && 
                            <div
                            style={{
                              backgroundImage: `url(${boardTile.tile.imageSmall}`,
                            }}
                            className="flexTiles justPickedTile"
                             />}
                        </div>
                        <h3>
                          {losers && (
                            <ul>
                              {losers.map((l, i) => (
                                <li key={i}>
                                  {l.userName}: {l.pointsResult}
                                </li>
                              ))}
                            </ul>
                          )}
                        </h3>
                      </Modal.Content>          
            : 
            <Modal.Content>
              <h3>booooo it's a tie. nobody win</h3>
            </Modal.Content>
            }
            <Modal.Actions>
              <Button
                color="green"
                onClick={closeModal}
                inverted
              >
                <Icon name="checkmark" /> Got it
              </Button>
              <Button
                color="blue"
                onClick={startRound}
                inverted
              >
                <Icon name="play" /> Next Round
              </Button>
            </Modal.Actions>
          </Modal>
        );
      };
export default observer(ResultModal);
