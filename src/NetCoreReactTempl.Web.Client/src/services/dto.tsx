export class BaseDto {
    id: number;
}

export class AuthInfo extends BaseDto {
    email: string;
    password: string;
    isVerify?: boolean;
    token?: string;
}

export class Data extends BaseDto {
    field1: string;
    field2: Date;
    field3: boolean;
    field4: RadioButton
    field5: DropDown
}

export enum RadioButton {
    radio1 = 1,
    radio2 = 2
}

export enum DropDown {
    option1 = 1,
    option2 = 2,
    option3 = 3
}
