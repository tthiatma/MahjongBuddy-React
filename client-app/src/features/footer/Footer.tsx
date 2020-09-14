import React, { useContext, Fragment } from "react";
import {
  Container,
  Segment,
  Divider,
  List,
} from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../app/stores/rootStore";

const Footer: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { showFooter } = rootStore.commonStore;

  return (
    <Fragment>
      {showFooter && (
        <Segment style={{ padding: "0em 0em" }} className="mastfooter" vertical>
          <Container textAlign="center">
            <Divider inverted section />
            <List horizontal inverted divided link size="small">
              <List.Item as="a" href="#">
                Site Map
              </List.Item>
              <List.Item as="a" href="#">
                Contact Us
              </List.Item>
              <List.Item as="a" href="#">
                Terms and Conditions
              </List.Item>
              <List.Item as="a" href="/privacypolicy">
                Privacy Policy
              </List.Item>
            </List>
          </Container>
        </Segment>
      )}
    </Fragment>
  );
};

export default observer(Footer);
