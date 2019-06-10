export const config = {
    apiUrl: '/rest'
};

export function handleResponse<T>(response: Response): Promise<string | T> {
    return new Promise((resolve, reject) => {
        if (response.ok) {
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                response.text().then(text => resolve(text));
            }
        } else {
            response.text().then(text => reject(text));
        }
    });
}

export function handleResponseData<T>(response: any): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        if (response.ok) {
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                response.text().then(text => resolve(text));
            }
        } else {
            response.text().then(text => reject(text));
        }
    });
}

export function handleError(error) {
    return Promise.reject(error && error.message);
}

export function authHeader() {
    // return authorization header with jwt token
    let user = JSON.parse(localStorage.getItem('user'));

    if (user && user.token) {
        return { 'Authorization': 'Bearer ' + user.token };
    } else {
        return {};
    }
}