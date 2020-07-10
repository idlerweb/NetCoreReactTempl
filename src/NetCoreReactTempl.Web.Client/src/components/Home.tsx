import React from 'react';
import { connect } from 'react-redux';

const Home = () => (
    <div className="jumbotron mt-4">
        <div className="justify-content-center">
            <div className="row">
                <h2>Net Core 3.1 + React + Redux Template</h2>
            </div>
            <div className="row">
                <p className="h5">version 0.2.0</p>
            </div>
        </div>
    </div>
);

export default connect()(Home);