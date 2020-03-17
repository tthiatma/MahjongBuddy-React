import axios, { AxiosResponse } from 'axios';
import { ITile } from '../models/tile';

axios.defaults.baseURL = 'http://localhost:5000/api';

const responseBody = (Response: AxiosResponse) => Response.data;

const sleep = (ms: number) => (response: AxiosResponse) => 
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms));

const request = {
    get: (url:string) => axios.get(url).then(sleep(1000)).then(responseBody),
    post: (url:string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url:string, body: {}) => axios.put(url,body).then(sleep(1000)).then(responseBody),
    del: (url:string) => axios.delete(url).then(sleep(1000)).then(responseBody) 
}

const Tiles ={
    list: ():Promise<ITile[]> => request.get('/tiles'),
    details: (id:string) => request.get(`/tiles/${id}`),
    create: (tile: ITile) => request.post('tiles', tile),
    update: (tile: ITile) => request.put(`/tiles/${tile.id}`, tile),
    delete: (id:string) => request.del(`/tiles/${id}`)
}

export default {
    Tiles
}