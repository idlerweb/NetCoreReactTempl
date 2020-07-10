import { handleError, config, handleResponseData, authHeader } from './baseService';
import { mapRestUrl } from './mapRestUrl';

export interface IRestServiceBase<T> {
    getList: (type: T, top: number, skip: number, search: string) => Promise<IRestResponseBase<T>>;
    get: (type: T, id: string | number) => Promise<IRestResponseBase<T>>;
    post: (type: T, data: T) => Promise<IRestResponseBase<T>>;
    put: (type: T, data: T) => Promise<IRestResponseBase<T>>;
    delete: (type: T, id: number) => Promise<IRestResponseBase<T>>;
}

export interface IRestResponseBase<T> {
    data: T;
    list: T[];
    count: number;
}

export class RestService<T> implements IRestServiceBase<T> {
    public getList(type: T, top: number = 10, skip: number = 0, search: string): Promise<IRestResponseBase<T>> {
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        let url = config.apiUrl + mapRestUrl<T>(type) + `?top=${top}&skip=${skip}`;

        if (search) {
            url += `&search=${search}`;
        }

        return fetch(url, requestOptions).then(r => handleResponseData<IRestResponseBase<T>>(r), handleError);
    }
    public get(type: T, id: number): Promise<IRestResponseBase<T>> {
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        let url = config.apiUrl + mapRestUrl<T>(type);

        if (id)
            url = url + '/' + id;

        return fetch(url, requestOptions).then(r => handleResponseData<IRestResponseBase<T>>(r), handleError);
    }
    public post(type: T, data: T): Promise<IRestResponseBase<T>> {
 
        const requestOptions = {
            method: 'POST',
            headers: {
                ...authHeader(),
                'Content-Type': 'application/json'                               
            },
            body: JSON.stringify(data)
        };

        let url = config.apiUrl + mapRestUrl<T>(type);

        return fetch(url, requestOptions).then(r => handleResponseData<IRestResponseBase<T>>(r), handleError);
    };
    public put(type: T, data: T): Promise<IRestResponseBase<T>> {

        const requestOptions = {
            method: 'PUT',
            headers: {
                ...authHeader(),
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        };

        let url = config.apiUrl + mapRestUrl<T>(type);

        return fetch(url, requestOptions).then(r => handleResponseData<IRestResponseBase<T>>(r), handleError);
    };
    public delete(type: T, id: number): Promise<IRestResponseBase<T>> {

        const requestOptions = {
            method: 'DELETE',
            headers: {
                ...authHeader(),
                'Content-Type': 'application/json'
            },
        };

        let url = mapRestUrl<T>(type);

        return fetch(url + '/' + id, requestOptions).then(r => handleResponseData<IRestResponseBase<T>>(r), handleError);
    };
};