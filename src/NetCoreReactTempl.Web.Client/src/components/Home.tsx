import React from 'react';
import { connect } from 'react-redux';

const Home = () => (
    <div className="jumbotron mt-4">
        <div className="justify-content-center">
            <div className="row">
                <h2>PW</h2>
            </div>
            <div className="row">
                <p className="h5">Transfer your PW</p>
            </div>
        </div>
    </div>
);

export default connect()(Home);