import AuthConstants from '../constants/auth';
import { IAuth, IAuthAction } from './Interfaces/IAuth';

let user = JSON.parse(localStorage.getItem('user'));
const initialState = user ? { loggedIn: true, user } : {};

export function authentication(state = initialState, action: IAuthAction): IAuth {
    switch (action.type) {
        case AuthConstants.LOGIN_REQUEST:
            return {
                loggingIn: true
            };
        case AuthConstants.LOGIN_SUCCESS:
            return {
                loggedIn: true,
                user: action.response.data
            };
        case AuthConstants.LOGIN_FAILURE:
            return {};
        case AuthConstants.LOGOUT:
            return {};
        default:
            return state
    }
}