import AlertConstants from '../constants/alert';
import IAlertAction from '../store/Interfaces/IAlertAction';

export default class AlertActions {
    static success(message): IAlertAction {
        return { type: AlertConstants.SUCCESS, message };
    }
    static error(message): IAlertAction {
        return { type: AlertConstants.ERROR, message };
    }
    static clear(): IAlertAction {
        return { type: AlertConstants.CLEAR };
    }
}