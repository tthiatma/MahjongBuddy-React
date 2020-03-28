export interface IUser{
    userName: string;
    displayName: string;
    token: string;
    image?: string;
    isHost: boolean;
}

export interface IUserFormValues{
    email: string;
    password: string;
    displayName?: string;
    userName?: string
}