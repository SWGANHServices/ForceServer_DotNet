import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { actionCreators } from '../store/ServerStatusStore';

class ServerStatus extends Component {
    componentWillMount() {
        // Call API to fetch from signalr.
        this.props.requestServerStatus();
    }

    render() {
        return (
            <div>
                <section className="serverstatus">
                    <div className={this.props.status === 'Online' ? "serverstatus__status alert alert-success": "serverstatus__status alert alert-danger"} role="alert">
                        <p>Server Status {this.props.status}</p>
                    </div>
                </section>
            </div>
        );
    }
}



export default connect(
    state => state.serverStatus,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(ServerStatus);
