import { observable, computed, action, runInAction } from "mobx";
import { IResetPasswordFormValues, IUser, IUserFormValues } from "../models/user";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";

export default class UserStore {
  refreshTokenTimeout: any;
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable user: IUser | null = null;
  @observable loading: boolean = false;

  @computed get isLoggedIn() {
    return !!this.user;
  }

  @action gotoForgotPassword = () => {
    history.push("/user/forgotPassword");
    this.rootStore.modalStore.closeModal();
  }

  @action forgotPassword = async(values: IResetPasswordFormValues) => {
    try {
      await agent.User.forgotPassword(values);
      this.rootStore.modalStore.closeModal();
      history.push(`/user/forgotPasswordSuccess?email=${values.email}`);
    } catch (error) {
      throw error;
    }
  }

  @action resetPassword = async(values: IResetPasswordFormValues) => {
    try {
      await agent.User.resetPassword(values);
      history.push(`/user/resetPasswordSuccess`);
    } catch (error) {
      throw error;
    }
  }

  @action register = async (values: IUserFormValues) => {
    try {
      await agent.User.register(values);
      this.rootStore.modalStore.closeModal();
      history.push(`/user/registerSuccess?email=${values.email}`);
    } catch (error) {
      throw error;
    }
  };

  @action refreshToken = async () => {
    this.stopRefreshTokenTimer();
    try {
      const user = await agent.User.refreshToken();
      runInAction(() => {
        this.user = user;
      })
      this.rootStore.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
    } catch (error) {
      console.log(error);
    }
  }

  @action login = async (values: IUserFormValues) => {
    try {
      const user = await agent.User.login(values);
      runInAction(() => {
        this.user = user;
      });
      this.rootStore.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      this.rootStore.modalStore.closeModal();
      history.push(`/`);
    } catch (error) {
      throw error;
    }
  };

  @action getUser = async () => {
    try {
      const user = await agent.User.current();
      runInAction(() => {
        this.user = user;
      });
      this.rootStore.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
    } catch (error) {
      console.log(error);
    }
  };

  @action logout = () => {
    this.rootStore.commonStore.setToken(null);
    this.user = null;
    history.push("/");
    this.stopRefreshTokenTimer();
  };

  @action fbLogin = async (response: any) => {
    this.loading = true;
    try {
      const user = await agent.User.fblogin(response.accessToken);
      runInAction(() => {
        this.user = user;
        this.rootStore.commonStore.setToken(user.token);
        this.startRefreshTokenTimer(user);
        this.rootStore.modalStore.closeModal();
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      })
      throw error;
    }
  };

  private startRefreshTokenTimer(user: IUser) {
    const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
