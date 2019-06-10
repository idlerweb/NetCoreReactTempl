import { applyMiddleware, combineReducers, createStore } from 'redux';
import thunk from 'redux-thunk';
import { routerMiddleware } from 'react-router-redux';
import { alert } from './alert';
import { authentication } from './authentication';
import { registration } from './registration';
import { placeholder } from './placeholder';
import { connectRouter } from 'connected-react-router';
import { data } from './data';

export default function configureStore(history, initialState) {

    const middleware = [
        thunk,
        routerMiddleware(history)
    ];

    const rootReducer = combineReducers({
        alert: alert,
        authentication: authentication,
        registration: registration,
        placeholder: placeholder,
        data: data,
        router: connectRouter(history)
    });

    return createStore(
        rootReducer,
        initialState,
        applyMiddleware(...middleware)
    );
}
