import React, { useContext, Fragment } from "react";
import { Container, Segment, Divider, List, Image } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../app/stores/rootStore";

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
              <List.Item as="a" href="/termsandconditions">
                <List.Content verticalAlign="middle">
                  Terms and Conditions
                </List.Content>
              </List.Item>
              <List.Item as="a" href="/privacypolicy">
                Privacy Policy
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
