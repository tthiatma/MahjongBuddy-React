import React, { useContext, Fragment } from "react";
import { Menu, Container, Dropdown, Image } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { NavLink, Link } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import JoinGameForm from "../user/JoinGameForm";

const NavBar: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { user, logout } = rootStore.userStore;
  const { openModal } = rootStore.modalStore;
  const { showNavBar } = rootStore.commonStore;
  return (
    <Fragment>
      {showNavBar && (
        <Menu fixed="top" inverted>
          <Container>
            <Menu.Item header as={NavLink} exact to="/">
              <img
                src="/assets/logo.png"
                alt="logo"
                style={{ marginRight: 10 }}
              />
              MahjongBuddy
            </Menu.Item>
            {user && (
              <Fragment>
                <Menu.Item
                  name="Join Game"
                  onClick={() => openModal(<JoinGameForm />)}
                />
                <Menu.Item as={NavLink} name="Create Game" to="/createGame" />
              </Fragment>
            )}
            <Menu.Item as={NavLink} name="Rules" to="/rules" />
            {/* <Menu.Item as={NavLink} name="About" to="/about" /> */}

            {user && (
              <Menu.Item position="right">
                <Image
                  avatar
                  spaced="right"
                  src={user.image || "/assets/user.png"}
                />
                <Dropdown pointing="top left" text={user.displayName}>
                  <Dropdown.Menu>
                    <Dropdown.Item
                      as={Link}
                      to={`/profile/${user.userName}`}
                      text="My profile"
                      icon="user"
                    />
                    <Dropdown.Item
                      onClick={logout}
                      text="Logout"
                      icon="power"
                    />
                  </Dropdown.Menu>
                </Dropdown>
              </Menu.Item>
            )}
          </Container>
        </Menu>
      )}
    </Fragment>
  );
};

export default observer(NavBar);
