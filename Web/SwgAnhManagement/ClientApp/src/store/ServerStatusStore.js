const requestServerStatusType = 'REQUEST_SERVER_STATUS';
const reciveServerStatusType = 'RECIVE_SERVER_STATUS';
const initalState = { isLoading: false, status: 'Offline', color: 'danger' };

// Fetch data
export const actionCreators = {
    requestServerStatus: () => async (dispatch, getState) => {
        dispatch({ type: requestServerStatusType });
    }
};

// Set states etc
export const reducer = (state, action) => {
    state = state || initalState;
    if (action.type === requestServerStatusType) {
        return {
            ...state,
            isLoading: true
        };
    }
    return state;
};