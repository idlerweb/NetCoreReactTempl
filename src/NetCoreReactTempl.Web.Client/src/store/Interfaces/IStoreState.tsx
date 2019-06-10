import { IAuth } from "./IAuth";
import IRegister from "./IRegister";
import IPlaceholder from "./IPlaceholder";
import { IData } from "./IData";

export default interface IStoreState {
    authentication?: IAuth;
    alert?: {};
    registration?: IRegister;
    placeholder?: IPlaceholder;
    data?: IData;
    router?: {};
};