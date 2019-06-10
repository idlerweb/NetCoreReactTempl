import React from 'react';
import { bindActionCreators, Dispatch } from "redux";
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import AuthActions from '../actions/auth';
import AlertActions from '../actions/alert';
import { ThunkAction } from 'redux-thunk';
import { IAuthAction } from '../store/Interfaces/IAuth';
import IStoreState from '../store/Interfaces/IStoreState';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';

interface ILoginPageState {
    loggingIn?: boolean;
    alert?: any;
    logout?: () => void;
    login?: (email: string, password: string) => ThunkAction<void, IStoreState, null, IAuthAction | RouterAction | IAlertAction>;
    clear?: () => void;
}

interface State {
    email: string;
    password: string;
    submitted: boolean;
    emailIsValid: boolean;
}

type Props = IStoreState & ILoginPageState;

class LoginPage extends React.Component<Props, State> {
    constructor(props) {
        super(props);

        this.props.logout();
        this.props.clear();

        this.state = {
            email: '',
            password: '',
            submitted: false,
            emailIsValid: true
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(e) {
        const { name, value } = e.target;
        if (name == "email") {
            this.setState({
                email: value,
                emailIsValid: value == '' || value.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)
            });
        }
        else if (name == "password")
            this.setState({ password: value });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ submitted: true });
        const { email, password } = this.state;
        if (email && password) {
            this.props.login(email, password);
        }
    }

    render() {
        const { loggingIn, alert } = this.props;
        const { email, password, submitted, emailIsValid } = this.state;
        return (
            <div className="row jumbotron mt-4 justify-content-center">
                <div className="col-md-6 col-md-offset-3">
                    <div className="row justify-content-center">
                        <h2>Login</h2>
                    </div>
                    <form name="form" className="mb-md-3" onSubmit={this.handleSubmit}>
                        <div className="form-group">
                            <label className="col-form-label col-form-label-lg" htmlFor="email">Email</label>
                            <input type="text" className={'form-control form-control-lg' + ((submitted && !email) || !emailIsValid ? ' is-invalid' : '')} name="email" value={email} onChange={this.handleChange} disabled={loggingIn ? true : false} />
                            <div className="invalid-feedback">Email not valid</div>


                        </div>
                        <div className={'form-group' + (submitted && !password ? ' is-invalid' : '')}>
                            <label className="col-form-label col-form-label-lg" htmlFor="password">Password</label>
                            <input type="password" className={'form-control form-control-lg' + (submitted && !password ? ' is-invalid' : '')} name="password" value={password} onChange={this.handleChange} disabled={loggingIn ? true : false} />
                            <div className="invalid-feedback">Password is required</div>
                        </div>
                        <blockquote className="blockquote mt-4 text-center">
                            <div className="form-group">
                                {!loggingIn && <button className="btn btn-primary btn-lg">Login</button>}
                                {loggingIn &&
                                    <button className="btn btn-primary"><div className="spinner-grow text-light mx-md-4" role="status">
                                        <span className="sr-only">Loading...</span>
                                    </div></button>}
                                &emsp;or&emsp;<Link to="/register" className="btn btn-secondary btn-lg">Register</Link>
                            </div>
                        </blockquote>
                    </form>
                    {alert && alert.message &&
                        <div className={`alert ${alert.type}`}>{alert.message}</div>
                    }
                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { loggingIn } = state.authentication;
    return {
        loggingIn,
        alert: state.alert
    };
}
function mapDispatchToProps(dispatch: Dispatch): ILoginPageState {
    return {
        logout: bindActionCreators(AuthActions.logout, dispatch),
        login: bindActionCreators(AuthActions.login, dispatch),
        clear: bindActionCreators(AlertActions.clear, dispatch)
    };
}


const connectedLoginPage = connect<{}, {}, ILoginPageState>(mapStateToProps, mapDispatchToProps)(LoginPage);
export { connectedLoginPage as LoginPage };