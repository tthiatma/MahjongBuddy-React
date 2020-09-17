import { observable, computed, action, runInAction } from "mobx";
import { IResetPasswordFormValues, IUser, IUserFormValues } from "../models/user";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";

export default class UserStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable user: IUser | null = null;
  @observable loading: boolean = false;

  @computed get isLoggedIn() {
    return !!this.user;
  }

  @action forgotPassword = async(email: string) => {
    try {
      await agent.User.forgotPassword(email);
      this.rootStore.modalStore.closeModal();
      history.push(`/user/forgotPasswordSuccess?email=${email}`);
    } catch (error) {
      throw error;
    }
  }

  @action resetPassword = async(values: IResetPasswordFormValues) => {
    try {
      await agent.User.resetPassword(values);
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

  @action login = async (values: IUserFormValues) => {
    try {
      const user = await agent.User.login(values);
      runInAction(() => {
        this.user = user;
      });
      this.rootStore.commonStore.setToken(user.token);
      this.rootStore.commonStore.setRefreshToken(user.refreshToken);
      this.rootStore.modalStore.closeModal();
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
    } catch (error) {
      console.log(error);
    }
  };

  @action logout = () => {
    this.rootStore.commonStore.setToken(null);
    this.rootStore.commonStore.setRefreshToken(null);
    this.user = null;
    history.push("/");
  };

  @action fbLogin = async (response: any) => {
    this.loading = true;
    try {
      const user = await agent.User.fblogin(response.accessToken);

      runInAction(() => {
        this.user = user;
        this.rootStore.commonStore.setToken(user.token);
        this.rootStore.commonStore.setRefreshToken(user.refreshToken);
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
}
