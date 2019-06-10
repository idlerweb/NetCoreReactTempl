import PlaceholderConstants from '../constants/placeholder';
import IPlaceholder from './Interfaces/IPlaceholder';

export function placeholder(state = {}, action): IPlaceholder {
    switch (action.type) {
        case PlaceholderConstants.SHOW:
            return { show: true };
        case PlaceholderConstants.HIDE:
            return { show: false };
        default:
            return state
    }
}