import axios, { AxiosResponse } from "axios";
import { history } from "../..";
import { IGame } from "../models/game";
import {
  IResetPasswordFormValues,
  IUser,
  IUserFormValues,
} from "../models/user";
import { toast } from "react-toastify";
import { IProfile, IPhoto } from "../models/profile";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.withCredentials = true;

axios.interceptors.request.use(
  (config) => {
    const token = window.localStorage.getItem("jwt");
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axios.interceptors.response.use(undefined, (error) => {
  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }
  const { status, data, config, headers } = error.response;
  if (status === 404) {
    history.push("/notfound");
  }

  if (status === 401 && headers['www-authenticate'] === 'Bearer error="invalid_token", error_description="The token is expired"') {
    window.localStorage.removeItem("jwt");
    history.push("/");
    toast.info("Your session has expired, please login again");
  }

  if (
    status === 400 &&
    config.method === "get" &&
    data.errors.hasOwnProperty("id")
  ) {
    history.push("/notfound");
  }
  if (status === 500) {
    toast.error("Server error - check the terminal for more info!");
  }
  throw error.response;
});

const responseBody = (Response: AxiosResponse) => Response.data;

// const sleep = (ms: number) => (response: AxiosResponse) =>
//     new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms));

const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
  postForm: (url: string, file: Blob) => {
    let formData = new FormData();
    formData.append("File", file);
    return axios
      .post(url, formData, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(responseBody);
  },
};

const Games = {
  list: (): Promise<IGame[]> => requests.get("/games"),
  detail: (id: string) => requests.get(`/games/${id}`),
  create: (game: IGame) => requests.post("games", game),
  update: (game: IGame) => requests.put(`/games/${game.id}`, game),
  delete: (id: string) => requests.del(`/games/${id}`),
  end:(id: string) => requests.post(`/games/${id}/end`, {}),
  latestRound: (id: string) => requests.get(`/games/${id}/latestround`),
};

const Rounds = {
  detail: (id: string, gameId: string, userName: string) =>
    requests.post(`/rounds/Details`, {
      id: id,
      gameId: gameId,
      userName: userName,
    }),
};

const User = {
  current: (): Promise<IUser> => requests.get("/user"),
  login: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/login/`, user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/register/`, user),
  fblogin: (accessToken: string) =>
    requests.post(`/user/facebook`, { accessToken }),
  refreshToken: (): Promise<IUser> => requests.post(`/user/refreshToken`, {}),
  verifyEmail: (token: string, email: string): Promise<void> =>
    requests.post(`/user/verifyEmail`, { token, email }),
  resendVerifyEmailConfirm: (email: string): Promise<void> =>
    requests.get(`/user/resendEmailVerification?email=${email}`),
  forgotPassword: (reset: IResetPasswordFormValues): Promise<void> => {
    return requests.post(`/user/forgotPassword`, reset);
  },
  resetPassword: (reset: IResetPasswordFormValues): Promise<void> =>
    requests.post(`/user/resetPassword`, reset),
  resendForgotPassword: (email: string): Promise<void> =>
    requests.get(`/user/resendForgotPasswordVerification?email=${email}`),
};

const Profiles = {
  get: (userName: string): Promise<IProfile> =>
    requests.get(`/profiles/${userName}`),
  uploadPhoto: (photo: Blob): Promise<IPhoto> =>
    requests.postForm(`/photos`, photo),
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.del(`/photos/${id}`),
  updateProfile: (profile: Partial<IProfile>) =>
    requests.put(`/profiles`, profile),
};

export default {
  Games,
  User,
  Rounds,
  Profiles,
};
