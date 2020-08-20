import axios, { AxiosResponse } from "axios";
import { ITile } from "../models/tile";
import { history } from "../..";
import { IGame } from "../models/game";
import { IUser, IUserFormValues } from "../models/user";
import { toast } from "react-toastify";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

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
  const originalRequest = error.config;

  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }
  const { status, data, config } = error.response;
  if (status === 404) {
    history.push("/notfound");
  }

  if (status === 401 && originalRequest.url.endsWith("refresh")) {
    window.localStorage.removeItem("jwt");
    window.localStorage.removeItem("refreshToken");
    history.push("/");
    toast.info("Your session has expired, please login again");
    return Promise.reject(error);
  }

  if (status === 401 && !originalRequest._retry) {
    originalRequest._retry = true;

    return axios
      .post("user/refresh", {
        token: window.localStorage.getItem("jwt"),
        refreshToken: window.localStorage.getItem("refreshToken"),
      })
      .then((res) => {
        window.localStorage.setItem("jwt", res.data.token);
        window.localStorage.setItem("refreshToken", res.data.refreshToken);
        axios.defaults.headers.common[
          "Authorization"
        ] = `Bearer ${res.data.token}`;
        return axios(originalRequest);
      });
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

const request = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
};

const Tiles = {
  list: (): Promise<ITile[]> => request.get("/tiles"),
  detail: (id: string) => request.get(`/tiles/${id}`),
  create: (tile: ITile) => request.post("tiles", tile),
  update: (tile: ITile) => request.put(`/tiles/${tile.id}`, tile),
  delete: (id: string) => request.del(`/tiles/${id}`),
};

const Games = {
  list: (): Promise<IGame[]> => request.get("/games"),
  detail: (id: string) => request.get(`/games/${id}`),
  create: (game: IGame) => request.post("games", game),
  update: (game: IGame) => request.put(`/games/${game.id}`, game),
  delete: (id: string) => request.del(`/games/${id}`),
  latestRound: (id: string) => request.get(`/games/${id}/latestround`),
};

const Rounds = {
  detail: (id: string, gameId: string) =>
    request.post(`/rounds/Details`, { id: id, gameId: gameId }),
};

const User = {
  current: (): Promise<IUser> => request.get("/user"),
  login: (user: IUserFormValues): Promise<IUser> =>
    request.post(`/user/login/`, user),
  register: (user: IUserFormValues): Promise<IUser> =>
    request.post(`/user/register/`, user),
  fblogin: (accessToken: string) =>
    request.post(`/user/facebook`, { accessToken }),
  refreshToken: (token: string, refreshToken: string) => {
      return axios.post(`/user/refresh`, {token, refreshToken})
      .then(res => {
        window.localStorage.setItem("jwt", res.data.token);
        window.localStorage.setItem("refreshToken", res.data.refreshToken);
        axios.defaults.headers.common[
          "Authorization"
        ] = `Bearer ${res.data.token}`;
        return res.data.token;  
      })
  },
};

export default {
  Tiles,
  Games,
  User,
  Rounds,
};
