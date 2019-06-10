import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import IStoreState from '../store/Interfaces/IStoreState';
import { AuthInfo } from '../services/dto';


interface INavMenuState {
    user?: string | AuthInfo;
}

interface State {
    popupVisible: boolean
}

type Props = IStoreState & INavMenuState;

class NavMenu extends React.Component<Props, State> {

    constructor(props) {
        super(props);
    }

    render() {
        const { user } = this.props;

        let menu = null;
        if (user) {
            menu = <ul className="navbar-nav mr-auto">
                <li className="nav-item">
                    <Link className="nav-link" to={'/data'} >
                        <h3 className="font-weight-light">Data</h3>
                    </Link>
                </li>
                <li className="nav-item">
                    <Link className="nav-link" to={'/create'} >
                        <h3 className="font-weight-light">Create</h3>
                    </Link>
                </li>
            </ul>
        }
        return (
            <nav className="navbar navbar-expand-lg">
                <Link className="nav-link" to={'/'} >
                    {user && <h3 className="row font-weight-light mr-md-4">{(user as AuthInfo).email}</h3>}
                </Link>
                {menu}
                <ul className="navbar-nav">
                    <li className="nav-item">
                        <Link className="nav-link" to={'/login'}>
                            <h3 className="font-weight-light">{user ? 'Logout' : 'Login'}</h3>
                        </Link>
                    </li>
                </ul>
            </nav>);
    }
};

function mapStateToProps(state: IStoreState): INavMenuState {
    return {
        user: state.authentication.user
    };
}

const connectedNavMenu = connect<{}, {}, INavMenuState>(mapStateToProps)(NavMenu);
export { connectedNavMenu as NavMenu };