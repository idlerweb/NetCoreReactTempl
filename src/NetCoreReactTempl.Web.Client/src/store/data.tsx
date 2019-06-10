import DataConstants from '../constants/data';
import { IData, IDataAction } from './Interfaces/IData';

export function data(state = {}, action: IDataAction): IData {
    switch (action.type) {
        case DataConstants.CREATEDATA_REQUEST:
        case DataConstants.GETDATA_REQUEST:
            return {
                loading: true
            };
        case DataConstants.SORTDATA:
            return {
                loading: false,
                list: action.response.list instanceof Array ? action.response.list : []
            };
        case DataConstants.GETDATA_SUCCESS:
            return {
                loading: false,
                list: action.response.list instanceof Array ? action.response.list.sort((a, b) => b.id - a.id) : [],
                data: action.response.data ? action.response.data : undefined,
                count: action.response.count
            };
        case DataConstants.CREATEDATA_SUCCESS:
            return {
                loading: false,
                data: action.response.data ? action.response.data : undefined
            };
        case DataConstants.CREATEDATA_FAILURE:
        case DataConstants.GETDATA_FAILURE:
            return {};
        default:
            return state;
    }
}