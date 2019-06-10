import React from 'react';
import { NavMenu } from './NavMenu';
import IStoreState from '../store/Interfaces/IStoreState';
import { connect } from 'react-redux';

export class Layout extends React.Component {

    render() {
        return <div className="container-fluid">
            <NavMenu />
            <div>
                {this.props.children}
            </div>
        </div>
    }
};