import React, { Component } from 'react'
import { connect } from 'react-redux';
import { actionCreators } from '../store/ServerStatusStore';

class ServerStatus extends Component {
    componentWillMount() {
        // Call API to fetch from signalr.
    }

    render() {
        return (
            <div>

            </div>
        );
    }
}


export default connect(dispatch => bindActionCreators(actionCreators, dispatch)
)(ServerStatus)