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
import { Data } from '../services/dto';
import { Link } from 'react-router-dom';

interface IListDataPageState {
    alert?: any;
    loading?: boolean;
    update?: boolean;
    clear?: () => void;
    list?: Data[];
    count?: number;
    sortList?: (list: Data[], field: string, sortAsc: boolean, count: number) => ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction>
    getList?: (top: number, skip: number, search: string) => ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction>
    create?: (order: Data) => ThunkAction<void, IStoreState, null, IDataAction | RouterAction | IAlertAction>
}

interface State {
    search: string;
}

type Props = IStoreState & IListDataPageState;

class ListDataPage extends React.Component<Props, State> {

    constructor(props) {
        super(props);

        this.props.clear();
        this.state = { search: null }
        this.handleSort = this.handleSort.bind(this);
        this.searchSubmit = this.searchSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        this.props.getList(10, 0, null);
    }

    handleChange(e) {
        e.preventDefault();
        const { name, value } = e.target;
        if (name == "search")
            this.setState({ search: value });
        
    }

    searchSubmit(e) {
        e.preventDefault();
        this.props.getList(10, 0, this.state.search);
    }

    sortAsc: boolean;
    handleSort(field: string) {
        this.sortAsc = !this.sortAsc
        const { list } = this.props;
        this.props.sortList(list, field, this.sortAsc, this.props.count);
    }

    render() {
        const { alert, list, loading } = this.props;
        const { search } = this.state;
        return (
            <div className="row jumbotron mt-4 justify-content-center">
                <div className="col-md-6 col-md-offset-3">
                    <div className="row justify-content-center">
                        <h2>Data</h2>
                    </div>
                    {loading && <div className="d-flex mt-2 justify-content-center">
                        <div className="spinner-border" style={{ width: '4rem', height: '4rem' }} role="status">
                            <span className="sr-only">Loading...</span>
                        </div>
                    </div>}
                    {alert && alert.message &&
                        <div className={`alert ${alert.type} mt-2`}>{alert.message}</div>
                    }
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <button className="btn btn-outline-secondary" type="button" onClick={this.searchSubmit}>Search</button>
                        </div>
                        <input type="text" className="form-control" onChange={this.handleChange} name="search" value={search} />
                    </div>
                    <table className="table hover mt-2">
                        <thead>
                            <tr>
                                <th scope="col" onClick={() => this.handleSort(`id`)} style={{ cursor: 'pointer' }}>#(sorted)</th>
                                <th scope="col">field1</th>
                                <th scope="col">field2</th>
                                <th scope="col">field3</th>
                                <th scope="col">field4</th>
                                <th scope="col">field5</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {list.map(p =>
                                <tr key={p.id}>
                                    <th scope="row">{p.id}</th>
                                    <td>{p.field1}</td>
                                    <td>{p.field2}</td>
                                    <td>{p.field3 && `true`}{!p.field3 && `false`}</td>
                                    <td>{p.field4}</td>
                                    <td>{p.field5}</td>  
                                    <td>
                                        <Link to={`/print/${p.id}`}>Print</Link>
                                    </td>
                                </tr>)
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: IStoreState): IListDataPageState {
    return {
        alert: state.alert,
        list: state.data.list instanceof Array ? state.data.list : [],
        count: state.data.count,
        loading: state.data.loading
    };
}
function mapDispatchToProps(dispatch: Dispatch): IListDataPageState {
    return {
        sortList: bindActionCreators(DataActions.sortList, dispatch),
        getList: bindActionCreators(DataActions.getList, dispatch),
        clear: bindActionCreators(AlertActions.clear, dispatch)
    };
}


const connectedListDataPage = connect<{}, {}, IListDataPageState>(mapStateToProps, mapDispatchToProps)(ListDataPage);
export { connectedListDataPage as ListDataPage };