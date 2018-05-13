import React from 'react';
import { connect } from 'react-redux';
import ServerStatus from './ServerStatus'
const Home = props => (
    <div>
        <h1>Hello, world!</h1>
        <ServerStatus />
    </div>
);

export default connect()(Home);
