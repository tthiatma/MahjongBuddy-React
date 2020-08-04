import { IRoundTile, TileValue, TileSetGroup } from "../../models/tile";
import _ from "lodash";
import { TileStatus } from "../../models/tileStatus";

export const GetPossibleChow = (
  boardActiveTile: IRoundTile,
  playerActiveTiles: IRoundTile[]
): Array<IRoundTile[]> => {
  let chowTilesOptions: Array<IRoundTile[]> = [];

  let sameTypeChowTiles = playerActiveTiles.filter(
    (t) =>
      t.tile.tileType === boardActiveTile.tile.tileType &&
      t.tile.tileValue !== boardActiveTile.tile.tileValue
  );

  // If the board active tile is one
  if (boardActiveTile.tile.tileValue === TileValue.One) {
    const tileTwo = sameTypeChowTiles?.find(
      (t) => t.tile.tileValue === TileValue.Two
    );
    const tileThree = sameTypeChowTiles?.find(
      (t) => t.tile.tileValue === TileValue.Three
    );

    if (tileTwo && tileThree) {
      let twoThreeArray: IRoundTile[] = [tileTwo, tileThree];
      chowTilesOptions.push(twoThreeArray);
    }
    // If the board active tile is nine
  } else if (boardActiveTile.tile.tileValue === TileValue.Nine) {
    const tileSeven = sameTypeChowTiles?.find(
      (t) => t.tile.tileValue === TileValue.Seven
    );
    const tileEight = sameTypeChowTiles?.find(
      (t) => t.tile.tileValue === TileValue.Eight
    );

    if (tileSeven && tileEight) {
      let sevenEightArray: IRoundTile[] = [tileSeven, tileEight];
      chowTilesOptions.push(sevenEightArray);
    }
    // if not one or nine
  } else {
    let possibleTiles = sameTypeChowTiles?.filter(
      (t) =>
        t.tile.tileValue === boardActiveTile!.tile.tileValue - 2 ||
        t.tile.tileValue === boardActiveTile!.tile.tileValue + 2 ||
        t.tile.tileValue === boardActiveTile!.tile.tileValue + 1 ||
        t.tile.tileValue === boardActiveTile!.tile.tileValue - 1
    );

    if (possibleTiles && possibleTiles.length > 1) {
      //remove dups
      possibleTiles = _.uniqWith(
        possibleTiles,
        (a, b) =>
          a.tile.tileType === b.tile.tileType &&
          a.tile.tileValue === b.tile.tileValue
      );

      //now we try to get possible tiles to chow
      for (var i = 0; i < possibleTiles.length; i++) {
        let t = possibleTiles[i];
        let testChowTiles: IRoundTile[] = [];
        testChowTiles.push(t);
        testChowTiles.push(boardActiveTile!);
        testChowTiles.sort((a, b) => a!.tile.tileValue - b!.tile.tileValue);

        let probableTile;
        if ((t.tile.tileValue + boardActiveTile!.tile.tileValue) % 2 !== 0) {
          //then these two tiles is connected
          probableTile = possibleTiles.find(
            (t) => t.tile.tileValue === testChowTiles[1].tile.tileValue + 1
          );
        } else {
          //then these two tiles is not connected
          probableTile = possibleTiles.find(
            (t) => t.tile.tileValue === testChowTiles[0].tile.tileValue + 1
          );
        }
        if (probableTile) {
          chowTilesOptions.push(
            [probableTile, t].sort(
              (a, b) => a!.tile.tileValue - b!.tile.tileValue
            )
          );
        }
      }
    }
  }

  //remove dups
  chowTilesOptions = _.uniqWith(chowTilesOptions, (a, b) => {
    let foundDups = true;
    var firsta = _.find(b, function (t) {
      return t.tile.tileValue === a[0].tile.tileValue;
    });
    var seconda = _.find(b, function (t) {
      return t.tile.tileValue === a[1].tile.tileValue;
    });

    if (!firsta || !seconda) foundDups = false;

    return foundDups;
  });

  return chowTilesOptions;
};

export const GetPossibleKong = (
  isPlayerTurn: boolean,
  playerTiles: IRoundTile[],
  boardActiveTile: IRoundTile
): IRoundTile[] => {
  const mainPlayerAliveTiles = _.filter(
    playerTiles,
    (t) => t.status === TileStatus.UserActive || t.status === TileStatus.UserJustPicked
  );

  let validTileForKongs: IRoundTile[] = [];

  var kongTiles = _.chain(playerTiles)
    .groupBy((asd) => `${asd.tile.tileType}-${asd.tile.tileValue}`)
    .filter((asd) => asd.length > 2)
    .value();

  _.forEach(kongTiles, function (ts) {
    //if player has 3 same tiles and not in graveyard, they can kong matching board active tile anytime even if its not their turn
    if (ts.length === 3) {
      let userActive = _.filter(
        ts,
        (t) => t.status !== TileStatus.UserGraveyard
      );
      if (
        userActive.length === 3 &&
        boardActiveTile &&
        userActive[0].tile.tileType === boardActiveTile.tile.tileType &&
        userActive[0].tile.tileValue === boardActiveTile.tile.tileValue
      )
        validTileForKongs.push(boardActiveTile);
    }

    if (ts.length === 4 && isPlayerTurn) {
      //if its player's turn
      //if player has 3 same tiles that's ponged, player can kong only from their active and just picked tile
      let pongedTile = _.filter(
        ts,
        (t) => t.tileSetGroup === TileSetGroup.Pong
      );
      if (pongedTile.length === 3) {
        //check if user active tile
        let tileIsAlive = _.filter(
          mainPlayerAliveTiles,
          (t) =>
            t.tile.tileValue === pongedTile[0].tile.tileValue &&
            t.tile.tileType === pongedTile[0].tile.tileType
        );

        if (tileIsAlive.length > 0) validTileForKongs.push(tileIsAlive[0]);
      }

      //if tile is not in graveyard, then player can kong when its his turn
      let allTileAlive = _.filter(
        ts,
        (t) => t.status !== TileStatus.UserGraveyard
      );
      if (allTileAlive.length === 4) validTileForKongs.push(allTileAlive[0]);
    }
  });

  return validTileForKongs;
};
