import React from "react";
import { Tab } from "semantic-ui-react";
import RulesHands from "./RulesHands";
import RulesExtraPoints from "./RulesExtraPoints";
import RulesBasic from "./RulesBasic";

const panes = [
  { menuItem: "Basic", render: () => <RulesBasic /> },
  { menuItem: "Hands", render: () => <RulesHands /> },
  { menuItem: "Extra Points", render: () => <RulesExtraPoints /> },
];

interface IProps {
  setActiveTab: (activeIndex: any) => void;
}

const RulesContent: React.FC<IProps> = ({ setActiveTab }) => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="left"
      panes={panes}
      onTabChange={(e, data) => setActiveTab(data.activeIndex)}
    />
  );
};

export default RulesContent;
