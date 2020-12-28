import React, { useContext, Fragment } from "react";
import { Container, Segment, Divider, List, Image, Icon } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../app/stores/rootStore";
import { NavLink } from "react-router-dom";

const Footer: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { showFooter } = rootStore.commonStore;

  return (
    <Fragment>
      {showFooter && (
        <Segment className="mastfooter" vertical>
          <Container textAlign="center">
            <Divider fitted />
            <List horizontal inverted divided link size="small">
              <List.Item as={NavLink} to="/termsandconditions">
                <List.Content verticalAlign="bottom">
                  Terms and Conditions
                </List.Content>
              </List.Item>
              <List.Item as={NavLink} to="/privacypolicy">
                <List.Content verticalAlign="bottom">
                  Privacy Policy
                </List.Content>
              </List.Item>
              <List.Item as={NavLink} to="/changelog">
                <List.Content verticalAlign="bottom">
                  Change Log
                </List.Content>
              </List.Item>
              <List.Item as="a" target="_blank" href="https://www.facebook.com/MahjongBuddyFB">
                <List.Content verticalAlign="top">
                  <Icon name="facebook" size="large" color="blue" />
                </List.Content>
              </List.Item>
              <List.Item
                as="a"
                href="https://ko-fi.com/P5P030YCN"
                target="_blank"
              >
                <Image src="/assets/ko-fi_support.png" alt="ko-fi support" />
              </List.Item>
            </List>
          </Container>
        </Segment>
      )}
    </Fragment>
  );
};

export default observer(Footer);
