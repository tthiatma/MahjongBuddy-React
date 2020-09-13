import React, { Fragment, useContext } from "react";
import { Container } from "semantic-ui-react";
import NavBar from "../nav/NavBar";
import RulesContent from "./RulesContent";
import { RootStoreContext } from "../../app/stores/rootStore";


const RulesPage = () => {
  const rootStore = useContext(RootStoreContext);

  const { setActiveTab } = rootStore.ruleStore;
    return (
    <Fragment>
      <NavBar />
      <Container style={{ paddingTop: "5em" }}>
        <RulesContent setActiveTab={setActiveTab} />
      </Container>
    </Fragment>
  );
};

export default RulesPage;
