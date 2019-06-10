import { AuthInfo, Data } from "./dto";

export function mapRestUrl<T>(type: T): string {
    switch (true) {
        case (type instanceof AuthInfo):
            return "/api/auth";
        case (type instanceof Data):
            return "/api/data";
        default:
            return ""
    }
}