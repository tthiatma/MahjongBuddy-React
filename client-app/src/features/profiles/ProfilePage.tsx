import React, { Fragment, useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import { LoadingComponent } from "../../app/layout/LoadingComponent";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";

interface RouteParams {
  username: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const ProfilePage: React.FC<IProps> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadingProfile,
    profile,
    loadProfile,
    isCurrentUser,
    loading,
    setActiveTab,
  } = rootStore.profileStore;

  useEffect(() => {
    loadProfile(match.params.username);
  }, [loadProfile, match.params.username]);

  if (loadingProfile) return <LoadingComponent content="Loading profile..." />;

  return (
    <Fragment>
      <Grid>
        <Grid.Column width={16}>
          <ProfileHeader
            profile={profile!}
            isCurrentUser={isCurrentUser}
            loading={loading}
          />
          <ProfileContent setActiveTab={setActiveTab} />
        </Grid.Column>
      </Grid>
    </Fragment>
  );
};

export default observer(ProfilePage);
