import React, { useContext } from "react";
import { observer } from "mobx-react-lite";
import { Modal, Tab } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";
import RulesHands from "../../rules/RulesHands";
import RulesExtraPoints from "../../rules/RulesExtraPoints";

const RulesModal: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { showRules, closeRulesModal } = rootStore.roundStore;

  const panes = [
    {
      menuItem: { key: "hands", content: "Hands" },
      render: () => (
        <Tab.Pane>
          <RulesHands />
        </Tab.Pane>
      ),
    },
    {
      menuItem: { key: "extra_points", content: "Extra Points" },
      render: () => (
        <Tab.Pane>
          <RulesExtraPoints />
        </Tab.Pane>
      ),
    },
  ];

  return (
    <Modal closeIcon open={showRules} onClose={closeRulesModal} size="small">
      <Modal.Content scrolling>
        <Tab panes={panes} />
      </Modal.Content>
    </Modal>
  );
};
export default observer(RulesModal);
