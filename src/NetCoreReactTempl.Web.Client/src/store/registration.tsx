import UserConstants from '../constants/user';
import IRegister from './Interfaces/IRegister';

export function registration(state = {}, action): IRegister {
    switch (action.type) {
        case UserConstants.REGISTER_REQUEST:
            return { registering: true };
        case UserConstants.REGISTER_SUCCESS:
            return { registered: true };
        case UserConstants.REGISTER_FAILURE:
            return {};
        default:
            return state
    }
}