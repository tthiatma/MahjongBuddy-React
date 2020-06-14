import React from "react";
import { Segment, Label } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { observable, reaction, runInAction } from "mobx";

const counter = observable({ count: 0 })

// invoke once of and dispose reaction: reacts to observable value.
 reaction(
    () => counter.count,
    (count, reaction) => {
        console.log("reaction 3: invoked. counter.count = " + count)
        reaction.dispose()
    }
)

runInAction(() => {counter.count = 1})

// prints:
// reaction 3: invoked. counter.count = 1

runInAction(() => {counter.count = 2})
// prints:
// (There are no logging, because of reaction disposed. But, counter continue reaction)

console.log(counter.count)


const TestPage = () => {
//   const rootStore = useContext(RootStoreContext);

// prints:
// 2

  return (
    <Segment inverted textAlign="center" vertical>
        <Label>Test</Label>
    </Segment>
  );
};

export default observer(TestPage);
