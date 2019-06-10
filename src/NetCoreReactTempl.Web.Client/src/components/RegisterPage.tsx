import React from 'react';
import { bindActionCreators, Dispatch } from "redux";
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import UserActions from '../actions/user';
import IStoreState from '../store/Interfaces/IStoreState';
import { IUser } from '../store/Interfaces/IUser';
import AlertActions from '../actions/alert';
import { IUserAction } from '../store/Interfaces/IUser';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';
import { ThunkAction } from 'redux-thunk';
import { AuthInfo } from '../services/dto';

interface IRegisterPageState {
    registering?: boolean;
    registered?: boolean;
    alert?: any;
    register?: (user: AuthInfo) => ThunkAction<void, IStoreState, null, IUserAction | RouterAction | IAlertAction>;
    clear?: () => void;
}

interface State {
    user: AuthInfo;
    repytPassword: string;
    submitted: boolean;
    emailIsValid: boolean;
    repytPasswordIsValid: boolean;
}

type Props = IStoreState & IRegisterPageState;

class RegisterPage extends React.Component<Props, State> {

    constructor(props) {
        super(props);

        this.props.clear();

        this.state = {
            user: {
                id: null,
                email: '',
                password: '',
            },
            submitted: false,
            repytPassword: '',
            emailIsValid: true,
            repytPasswordIsValid: true
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const { name, value } = event.target;
        const { user } = this.state;
        switch (name) {
            case "email":
                this.setState({
                    user: {
                        ...user,
                        [name]: value
                    },
                    emailIsValid: value.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)
                });
                break;
            case "name":
                this.setState({
                    user: {
                        ...user,
                        [name]: value
                    }
                });
                break;
            case "password":
                this.setState({
                    user: {
                        ...user,
                        [name]: value
                    }
                });
                break;
            case "repytPassword":
                this.setState({
                    repytPassword: value,
                    repytPasswordIsValid: value == user.password
                });

        }
    }

    handleSubmit(event) {
        event.preventDefault();

        this.setState({ submitted: true });
        const { user, repytPassword } = this.state;
        if (user.email && user.password && repytPassword) {
            const { register } = this.props;
            register(user);
        }
    }

    render() {
        const { registering, alert, registered } = this.props;
        const { user, submitted, emailIsValid, repytPassword, repytPasswordIsValid } = this.state;
        return (
            <div className="row jumbotron mt-4 justify-content-center">
                <div className="col-md-6 col-md-offset-3">
                    <div className="row justify-content-center">
                        <h2>Register</h2>
                    </div>
                    {!registered && <form name="form" onSubmit={this.handleSubmit}>
                        <div className="form-group">
                            <label className="col-form-label col-form-label-lg" htmlFor="email">Email</label>
                            <input type="text" className={'form-control form-control-lg' + ((submitted && !user.email) || !emailIsValid ? ' is-invalid' : '')} name="email" value={user.email} onChange={this.handleChange} />
                            <div className="invalid-feedback">Email is required</div>
                        </div>
                        <div className="form-group">
                            <label className="col-form-label col-form-label-lg" htmlFor="password">Password</label>
                            <input type="password" className={'form-control form-control-lg' + (submitted && !user.password ? ' is-invalid' : '')} name="password" value={user.password} onChange={this.handleChange} disabled={registering ? true : false} />
                            <div className="invalid-feedback">Password is required</div>
                        </div>
                        <div className="form-group">
                            <label className="col-form-label col-form-label-lg" htmlFor="repytPassword">Repyt Password</label>
                            <input type="password" className={'form-control form-control-lg' + ((submitted && !repytPassword) || !repytPasswordIsValid ? ' is-invalid' : '')} name="repytPassword" value={repytPassword} onChange={this.handleChange} disabled={registering ? true : false} />
                            <div className="invalid-feedback">Password not valid</div>
                        </div>
                        <div className="row mt-4 justify-content-center">
                            <div className="form-group">
                                {!registering && <button className="btn btn-primary btn-lg">Registry</button>}
                                {registering &&
                                    <button className="btn btn-primary btn-lg"><div className="spinner-grow text-light mx-md-4" role="status">
                                        <span className="sr-only">Loading...</span>
                                    </div></button>}
                                &emsp;or&emsp;<Link to="/login" className="btn btn-link"><h4 className="font-weight-light">Cancel</h4>
                                </Link>
                            </div>
                        </div>
                    </form>}
                    {alert && alert.message &&
                        <div className={`alert ${alert.type}`}>{alert.message}</div>
                    }
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: IStoreState): IRegisterPageState {
    return {
        registering: state.registration.registering,
        registered: state.registration.registered,
        alert: state.alert
    };
}

function mapDispatchToProps(dispatch: Dispatch): IRegisterPageState {
    return {
        register: bindActionCreators(UserActions.register, dispatch),
        clear: bindActionCreators(AlertActions.clear, dispatch)
    };
}

const connectedRegisterPage = connect<{}, {}, IRegisterPageState>(mapStateToProps, mapDispatchToProps)(RegisterPage);
export { connectedRegisterPage as RegisterPage };