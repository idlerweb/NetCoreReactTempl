import React from 'react';
import { bindActionCreators, Dispatch } from "redux";
import { connect } from 'react-redux';
import $ from 'jquery';
import AlertActions from '../actions/alert';
import IStoreState from '../store/Interfaces/IStoreState';
import { ThunkAction } from 'redux-thunk';
import { IDataAction, IData } from '../store/Interfaces/IData';
import { RouterAction } from 'connected-react-router';
import IAlertAction from '../store/Interfaces/IAlertAction';
import DataActions from '../actions/data';
import { Data, RadioButton, DropDown, Fields } from '../services/Dto';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

interface IDataPageState {
    alert?: any;
    clear?: () => void;
    loading?: boolean;
    create?: (data: Data) => ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction>;
}

interface State extends Fields {
    submitted: boolean;
}

type Props = IStoreState & IDataPageState;

class DataPage extends React.Component<Props, State> {
    createRef: HTMLDivElement;
    constructor(props) {
        super(props);

        this.props.clear();
        this.state = {
            field1: "",
            field2: new Date(),
            field3: false,
            field4: null,
            field5: null,
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleData = this.handleData.bind(this);
        this.handleCreate = this.handleCreate.bind(this);
        this.handleChangeDate = this.handleChangeDate.bind(this)
    }

    handleChange(e) {
        const { name, value, checked } = e.target;
        switch (name) {
            case "field1":
                this.setState({
                    field1: value
                });
                break;
            case "field2":
                this.setState({
                    field2: value
                });
                break;
            case "field3":
                this.setState({
                    field3: checked
                });
                break;
            case "field4":
                this.setState({
                    field4: value
                });
                break;
            case "field5":
                this.setState({
                    field5: value
                });
                break;
            default:
                break;
        }
    }

    handleChangeDate(date) {
        this.setState({
            field2: date
        });
    }

    handleData(e) {
        e.preventDefault();
        const { field1, field4, field5 } = this.state;
        if (field1 && field4 && field5) {
            ($(this.createRef) as any).modal('hide');
            this.props.create({ id: 0, fields: { ...this.state } });
        }
    }

    handleCreate(e) {
        e.preventDefault();
        this.setState({ submitted: true })
        const { field1, field4, field5 } = this.state;
        if (field1 && field4 && field5) {
            ($(this.createRef) as any).modal('show');
        }
    }

    render() {
        const { submitted, field1, field2, field3, field4, field5 } = this.state;
        const { alert, loading } = this.props;
        return (<div className="jumbotron">
            {loading && <div className="d-flex mt-5 justify-content-center">
                <div className="spinner-border" style={{ width: '4rem', height: '4rem' }} role="status">
                    <span className="sr-only">Loading...</span>
                </div>
            </div>}
            {!loading && <div className="col-6 col-offset-3"> <div className="input-group mb-3">
                
            </div>
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="inputGroup-field1">field1</span>
                    </div>
                    <input type="text" className={'form-control' + (submitted && !field1 ? ' is-invalid' : '')} name="field1" value={field1} onChange={this.handleChange} aria-describedby="inputGroup-field1" />

                    <div className="invalid-feedback">field1 not valid</div>
                </div>
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="inputGroup-field2">field2</span>
                    </div>
                    <DatePicker className="form-control" selected={field2} onChange={this.handleChangeDate} aria-describedby="inputGroup-field2" />
                </div>
                <div className="form-group form-check mb-3">
                    <input type="checkbox" className="form-check-input" name="field3" checked={field3 as boolean} onChange={this.handleChange} id="field3" />
                    <label className="form-check-label" htmlFor="field3">field3 </label>
                </div>
                <div className="input-group mb-3">
                    <div className="form-check">
                        <input type="radio" className={'form-check-input' + (submitted && field4 == null ? ' is-invalid' : '')} name="field4" checked={field4 == RadioButton.radio1} value={RadioButton.radio1} onChange={this.handleChange} id="field41" />
                        <label className="form-check-label" htmlFor="field41">field4 option 1</label>
                    </div>
                    <div className="form-check">
                        <input type="radio" className={'form-check-input' + (submitted && field4 == null ? ' is-invalid' : '')} name="field4" checked={field4 == RadioButton.radio2} value={RadioButton.radio2} onChange={this.handleChange} id="field42"/>
                        <label className="form-check-label" htmlFor="field42">field4 option 2</label>
                    </div>
                    <div className="invalid-feedback">field4 not valid</div>
                </div>
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <button className="btn btn-outline-secondary dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">field5</button>
                        <div className="dropdown-menu">
                            <a className="dropdown-item" href="#" onClick={() => this.setState({ field5: DropDown.option1 })}>Option 1</a>
                            <a className="dropdown-item" href="#" onClick={() => this.setState({ field5: DropDown.option2 })}>Option 2</a>
                            <a className="dropdown-item" href="#" onClick={() => this.setState({ field5: DropDown.option3 })}>Option 3</a>
                        </div>
                    </div>
                    <input type="text" className={'form-control' + (submitted && field5 == null ? ' is-invalid' : '')} value={field5} disabled />
                    <div className="invalid-feedback">field5 not valid</div>
                </div>
                <button type="button" className="btn btn-primary" onClick={this.handleCreate}>Create</button>
                <div className="modal fade" ref={(modal) => { this.createRef = modal; }} tabIndex={-1} role="dialog" aria-hidden="true">
                        <div className="modal-dialog modal-dialog-centered" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title" id="exampleModalCenterTitle">Сonfirmation</h5>
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                Are you sure you want to create Data?
                        </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" data-dismiss="modal">No</button>
                                <button type="button" className="btn btn-primary" onClick={this.handleData}>Yes</button>
                            </div>
                        </div>
                    </div>
                </div>
                {alert && alert.message &&
                    <div className={`alert ${alert.type} mt-3`}>{alert.message}</div>
                }
            </div>}
        </div>);
    }
}

function mapStateToProps(state: IStoreState): IDataPageState {
    return {
        alert: state.alert,
        loading: state.data.loading
    };
}
function mapDispatchToProps(dispatch: Dispatch): IDataPageState {
    return {
        create: bindActionCreators(DataActions.create, dispatch),
        clear: bindActionCreators(AlertActions.clear, dispatch)
    };
}


const connectedDataPage = connect<{}, {}, IDataPageState>(mapStateToProps, mapDispatchToProps)(DataPage);
export { connectedDataPage as DataPage };