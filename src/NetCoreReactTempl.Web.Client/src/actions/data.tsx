import AlertActions from './alert';
import DataConstants from '../constants/data';
import IAlertAction from '../store/Interfaces/IAlertAction';
import { RouterAction, push } from 'connected-react-router';
import { IDataAction } from '../store/Interfaces/IData';
import IStoreState from '../store/Interfaces/IStoreState';
import { ThunkAction } from 'redux-thunk';
import { Data } from '../services/dto';
import { RestService, IRestResponseBase } from '../services/restService';

export default class DataActions {

    static getList(top: number = 10, skip: number = 0, search: string): ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction> {
        return dispatch => {
            dispatch({ type: DataConstants.GETDATA_REQUEST });

            let service = new RestService<Data>()

            service.getList(new Data(), top, skip, search)
                .then(
                    result => {
                        dispatch({ type: DataConstants.GETDATA_SUCCESS, response: result });
                    },
                    error => {
                        dispatch({ type: DataConstants.GETDATA_FAILURE, error });
                        dispatch(AlertActions.error(error));
                    }
                );
        };
    }

    static create(data: Data): ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction> {
        return dispatch => {
            dispatch({ type: DataConstants.CREATEDATA_REQUEST });

            let service = new RestService<Data>()

            service.post(new Data(), data)
                .then(
                    result => {
                        dispatch({ type: DataConstants.CREATEDATA_SUCCESS, response: result });
                        dispatch(AlertActions.success('Create success'));
                    },
                    error => {
                        dispatch({ type: DataConstants.CREATEDATA_FAILURE, error });
                        dispatch(AlertActions.error(error));
                    }
                );
        };
    }

    static get(id: number): ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction> {
        return dispatch => {
            dispatch({ type: DataConstants.GETDATA_REQUEST });

            let service = new RestService<Data>()

            service.get(new Data(), id)
                .then(
                    result => {
                        dispatch({ type: DataConstants.GETDATA_SUCCESS, response: result });
                    },
                    error => {
                        dispatch({ type: DataConstants.GETDATA_FAILURE, error });
                        dispatch(AlertActions.error(error));
                    }
                );
        };
    }

    static sortList(list: Data[], field: string, sortAsc: boolean, count: number): ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction> {
        return dispatch => {
            if (sortAsc)
                var listSort = { list: list.sort((a, b) => a[field] - b[field]).map(po => po) };
            else
                var listSort = { list: list.sort((a, b) => b[field] - a[field]).map(po => po) };

            dispatch({
                type: DataConstants.SORTDATA, response: {
                    data: undefined,
                    list: listSort.list,
                    count
                }
            });
        };
    }
}