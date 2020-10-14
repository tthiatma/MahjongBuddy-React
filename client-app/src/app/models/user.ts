export interface IUser{
    id: string;
    userName: string;
    displayName: string;
    token: string;
    refreshToken: string;
    image?: string;
    isHost: boolean;
}

export interface IResetPasswordFormValues{
    email: string;
    token: string;
    newPassword: string;
}

export interface IUserFormValues{
    email: string;
    password: string;
    displayName?: string;
    userName?: string
}