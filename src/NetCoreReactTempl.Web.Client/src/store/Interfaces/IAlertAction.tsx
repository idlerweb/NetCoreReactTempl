import AlertConstants from "../../constants/alert";

export default interface IAlertAction {
    type: AlertConstants,
    message?: string
}