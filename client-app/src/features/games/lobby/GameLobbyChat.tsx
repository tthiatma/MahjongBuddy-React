import React, { Fragment, useContext, useEffect } from 'react';
import { Segment, Header, Form, Button, Comment } from 'semantic-ui-react';
import { RootStoreContext } from '../../../app/stores/rootStore';
import {Form as FinalForm, Field} from 'react-final-form';
import { Link } from 'react-router-dom';
import TextAreaInput from '../../../app/common/form/TextAreaInput';
import { observer } from 'mobx-react-lite';

const GameLobbyChat = () => {
  const rootStore = useContext(RootStoreContext);
  const {    
    stopHubConnection,
    createHubConnection,
    addChatMsg,
    game
  } = rootStore.gameStore;

  useEffect(() => {
    createHubConnection(game!.id);
    return () => {
      stopHubConnection();
    }
  }, [createHubConnection, stopHubConnection, game])

  return (
    <Fragment>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Chat about this event</Header>
      </Segment>
      <Segment attached>
        <Comment.Group>
          {game &&
            game.chatMsgs &&
            game.chatMsgs.map(cm => (
              <Comment key={cm.id}>
                <Comment.Avatar src={cm.image || "/assets/user.png"} />
                <Comment.Content>
                  <Comment.Author as={Link} to={`/profile/${cm.userName}`}>
                    {cm.displayName}
                  </Comment.Author>
                  <Comment.Metadata>
                    <div>{cm.createdAt}</div>
                  </Comment.Metadata>
                  <Comment.Text>{cm.body}</Comment.Text>
                </Comment.Content>
              </Comment>
            ))}
          <FinalForm
            onSubmit={addChatMsg}
            render={({ handleSubmit, submitting, form }) => (
              <Form onSubmit={() => handleSubmit()!.then(() => form.reset())}>
                <Field
                  name="body"
                  component={TextAreaInput}
                  rows={2}
                  placeholder="Add your message"
                />
                <Button
                  content="Add Message"
                  labelPosition="left"
                  icon="edit"
                  primary
                  loading={submitting}
                />
              </Form>
            )}
          />
        </Comment.Group>
      </Segment>
    </Fragment>
  );
};

export default observer(GameLobbyChat);
