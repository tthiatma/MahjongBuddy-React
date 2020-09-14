import React, { Fragment, useContext } from "react";
import RulesContent from "./RulesContent";
import { RootStoreContext } from "../../app/stores/rootStore";


const RulesPage = () => {
  const rootStore = useContext(RootStoreContext);

  const { setActiveTab } = rootStore.ruleStore;
    return (
    <Fragment>
        <RulesContent setActiveTab={setActiveTab} />
    </Fragment>
  );
};

export default RulesPage;
