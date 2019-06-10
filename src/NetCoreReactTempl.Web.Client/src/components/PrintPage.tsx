import React from 'react';
import { bindActionCreators, Dispatch } from "redux";
import { connect } from 'react-redux';
import AlertActions from '../actions/alert';
import IStoreState from '../store/Interfaces/IStoreState';
import { ThunkAction } from 'redux-thunk';
import { IDataAction } from '../store/Interfaces/IData';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';
import DataActions from '../actions/data';
import { Data, RadioButton } from '../services/Dto';
import "react-datepicker/dist/react-datepicker.css";

interface IPrintPageState {
    alert?: any;
    clear?: () => void;
    loading?: boolean;
    getData?: (id: number) => ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction>;
    data?: Data;
    match?: any;
}

type Props = IStoreState & IPrintPageState;

class PrintPage extends React.Component<Props> {
    createRef: HTMLDivElement;
    constructor(props) {
        super(props);

        this.props.clear();
    }

    componentDidMount() {
        const { match: { params } } = this.props;
        this.props.getData(params.id);
    }

    render() {
        const { alert, loading, data } = this.props;
        return (<div className="jumbotron">
            {loading && <div className="d-flex mt-5 justify-content-center">
                <div className="spinner-border" style={{ width: '4rem', height: '4rem' }} role="status">
                    <span className="sr-only">Loading...</span>
                </div>
            </div>}
            {!loading && data && <div className="col-6 col-offset-3"> 
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="inputGroup-field1">field1</span>
                    </div>
                    <input type="text" className="form-control" name="field1" value={data.fields.field1} readOnly aria-describedby="inputGroup-field1" />
                </div>
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="inputGroup-field2">field2</span>
                    </div>
                    <input type="text" className="form-control" name="field2" value={data.fields.field2 instanceof Date ? data.fields.field2.toISOString() : data.fields.field2} readOnly aria-describedby="inputGroup-field2" />
                </div>
                <div className="form-group form-check mb-3">
                    <input type="checkbox" className="form-check-input" name="field3" checked={data.fields.field3 == 'true'} readOnly id="field3" />
                    <label className="form-check-label" htmlFor="field3">field3 </label>
                </div>
                <div className="input-group mb-3">
                    <div className="form-check">
                        <input type="radio" className="form-control" name="field4" checked={data.fields.field4 == RadioButton.radio1} value={RadioButton.radio1} readOnly id="field41" />
                        <label className="form-check-label" htmlFor="field41">field4 option 1</label>
                    </div>
                    <div className="form-check">
                        <input type="radio" className="form-control" name="field4" checked={data.fields.field4 == RadioButton.radio2} value={RadioButton.radio2} readOnly id="field42" />
                        <label className="form-check-label" htmlFor="field42">field4 option 2</label>
                    </div>
                </div>
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <button className="btn btn-outline-secondary dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">field5</button>
                    </div>
                    <input type="text" className="form-control" value={data.fields.field5} readOnly />
                </div>
                {alert && alert.message &&
                    <div className={`alert ${alert.type} mt-3`}>{alert.message}</div>
                }
            </div>}
        </div>);
    }
}

function mapStateToProps(state: IStoreState): IPrintPageState {
    return {
        alert: state.alert,
        loading: state.data.loading,
        data: state.data.data ? state.data.data : undefined
    };
}
function mapDispatchToProps(dispatch: Dispatch): IPrintPageState {
    return {
        getData: bindActionCreators(DataActions.get, dispatch),
        clear: bindActionCreators(AlertActions.clear, dispatch)
    };
}


const connectedPrintPage = connect<{}, {}, IPrintPageState>(mapStateToProps, mapDispatchToProps)(PrintPage);
export { connectedPrintPage as PrintPage };