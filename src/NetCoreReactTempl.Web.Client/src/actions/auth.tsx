import AuthConstants from '../constants/auth';
import AlertActions from './alert';
import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';
import { IAuthAction } from '../store/Interfaces/IAuth';
import IStoreState from '../store/Interfaces/IStoreState';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';
import { RestService } from '../services/restService';
import { AuthInfo } from '../services/dto';
import AuthService from '../services/auth';

export default class AuthActions {

    static login(email, password): ThunkAction<void, IStoreState, null, IAuthAction | RouterAction | IAlertAction> {
        return dispatch => {
            dispatch({ type: AuthConstants.LOGIN_REQUEST });

            let service = new RestService<AuthInfo>()
            let data: AuthInfo = {
                id: 0,
                email,
                password
            };

            service.put(new AuthInfo(), data)
                .then(
                    result => {
                        dispatch({ type: AuthConstants.LOGIN_SUCCESS, response: result });
                        if (result && result.data.token) {
                            localStorage.setItem('user', JSON.stringify(result.data));
                        }
                        dispatch(push('/data'));
                    },
                    error => {
                        dispatch({ type: AuthConstants.LOGIN_FAILURE, error });
                        dispatch(AlertActions.error(JSON.parse(error).errors.join(", ")));
                    }
                );
        };
    }

    static logout() {
        AuthService.logout();
        return { type: AuthConstants.LOGOUT };
    }
}