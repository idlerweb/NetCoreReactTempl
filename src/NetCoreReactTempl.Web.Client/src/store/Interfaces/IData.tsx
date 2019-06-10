import DataConstants from "../../constants/data";
import { Data } from "../../services/dto";
import { IRestResponseBase } from "../../services/restService";

export interface IData {
    data?: Data;
    list?: Data[];
    count?: number;
    loading?: boolean;
}

export interface IDataAction {
    type: DataConstants,
    response?: IRestResponseBase<Data>
}