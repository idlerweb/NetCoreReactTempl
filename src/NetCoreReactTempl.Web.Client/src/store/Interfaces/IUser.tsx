import UserConstants from "../../constants/user";
import { AuthInfo } from "../../services/dto";

export interface IUser {
    email: string;
    name: string;
    password: string;
    loggingIn?: boolean;
}

export interface IUserAction {
    type: UserConstants,
    user?: string | AuthInfo,
    error?: any
}