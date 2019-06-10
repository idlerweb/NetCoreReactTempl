import UserConstants from '../constants/user';
import AlertActions from './alert';
import IStoreState from '../store/Interfaces/IStoreState';
import { IUserAction } from '../store/Interfaces/IUser';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';
import { ThunkAction } from 'redux-thunk';
import { RestService } from '../services/restService';
import { AuthInfo } from '../services/dto';

export default class UserActions {

    static register(user: AuthInfo): ThunkAction<void, IStoreState, null, IUserAction | RouterAction | IAlertAction> {
        return dispatch => {
            dispatch({ type: UserConstants.REGISTER_REQUEST });

            let service = new RestService<AuthInfo>()
            let data: AuthInfo = {
                id: 0,
                email: user.email,
                password: user.password,
            };

            service.post(new AuthInfo(), data)
                .then(
                    result => {
                        dispatch({ type: UserConstants.REGISTER_SUCCESS, result });
                        dispatch(AlertActions.success("Registration success"));
                    },
                    error => {
                        dispatch({ type: UserConstants.REGISTER_FAILURE, error });
                        dispatch(AlertActions.error(error));
                    }
                );
        };
    }
}