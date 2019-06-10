export class BaseDto {
    id: number;
}

export class AuthInfo extends BaseDto {
    email: string;
    password: string;
    isVerify?: boolean;
    token?: string;
}

export class Fields {
    field1: string;
    field2: Date;
    field3: boolean | string;
    field4: RadioButton;
    field5: DropDown;
}


export class Data extends BaseDto {
    fields: Fields;
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
