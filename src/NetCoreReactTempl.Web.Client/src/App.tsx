import * as React from 'react';
import { Route, Switch, Redirect } from 'react-router';
import { Layout } from './components/Layout';
import Home from './components/Home';
import { LoginPage } from './components/LoginPage';
import { RegisterPage } from './components/RegisterPage';
import { ListDataPage } from './components/ListDataPage';
import { DataPage } from './components/DataPage';
import { PrintPage } from './components/PrintPage';

const PrintLayout = ({ children }) => (
    <div>
        {children}
    </div>
);  

const PrintLayoutRoute = ({ component: Component, ...rest }) => {
    return (
        <Route {...rest} render={matchProps => (
            <PrintLayout>
                <Component {...matchProps} />
            </PrintLayout>
        )} />
    )
};

const LayoutRoute = ({ component: Component, ...rest }) => {
    return (
        <Route {...rest} render={matchProps => (
            <Layout>
                <Component {...matchProps} />
            </Layout>
        )} />
    )
};

const PrivateRoute = ({ component: Component, ...rest }) => (
    <Route {...rest} render={props => (
        localStorage.getItem('user')
            ? <Layout>
                <Component {...props} />
            </Layout>
            : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
    )} />
)

export default () => (    
    <Switch>
        <LayoutRoute exact path='/' component={Home} />
        <LayoutRoute exact path='/login' component={LoginPage} />
        <LayoutRoute exact path='/register' component={RegisterPage} />
        <PrivateRoute exact path='/data' component={ListDataPage} />
        <PrivateRoute exact path='/create' component={DataPage} />
        <PrintLayoutRoute exact path='/print/:id' component={PrintPage} />
    </Switch>
);