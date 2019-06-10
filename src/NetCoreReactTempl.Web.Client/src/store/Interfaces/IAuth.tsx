import AuthConstants from "../../constants/auth";
import { AuthInfo } from "../../services/dto";
import { IRestResponseBase } from "../../services/restService";

export interface IAuth {
    loggingIn?: boolean;
    loggedIn?: boolean;
    user?: string | AuthInfo;
}

export interface IAuthAction {
    type: AuthConstants,
    response?: IRestResponseBase<AuthInfo>
}