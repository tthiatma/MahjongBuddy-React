import React, { useContext } from "react";
import { Segment, Label, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { runInAction } from "mobx";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";

interface TestParams {
  roundId: string;
  id: string;
}

const TestPage: React.FC<RouteComponentProps<TestParams>> = () => {
  const rootStore = useContext(RootStoreContext);
  const { isMyTurn, pickCounter: counter, canPick } = rootStore.roundStore;
  // const [counter, setCounter] = useState(5);
  // const [myTurn, setMyTurn] = useState(false);
  // const [canPick, setCanPick] = useState(false);

  const toggleIsMyTurn = () => {
    runInAction(() => {
      rootStore.roundStore.isMyTurn = !isMyTurn;
    });
  };

  // const toggleMyTurn = () => {
  //   setMyTurn(!myTurn);
  // };

  // useEffect(() => {
  //   if (myTurn) {
  //     counter > 0 && setTimeout(() => setCounter(counter - 1), 1000);
  //     if (counter === 0) setCanPick(true);
  //   } else {
  //     setCounter(3);
  //     setCanPick(false);
  //   }
  // }, [counter, myTurn]);

  return (
    <Segment inverted textAlign="center" vertical>
      {/* <div>
        <Label>{counter}</Label>
        <Label>{myTurn.toString()}</Label>
        <Button onClick={toggleMyTurn}>Go</Button>
        <Button disabled={!canPick}>{canPick.toString()}</Button>
      </div> */}
      <div>
        <Label>{counter}</Label>
        <Label>{isMyTurn.toString()}</Label>
        <Button onClick={toggleIsMyTurn}>Go</Button>
        <Button disabled={!canPick}>{canPick.toString()}</Button>
      </div>
    </Segment>
  );
};

export default observer(TestPage);
