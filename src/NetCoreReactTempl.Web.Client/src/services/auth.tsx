import { handleResponse, handleError, config } from './baseService';
import { AuthInfo } from './dto';

const url = config.apiUrl + '/api/auth';

export default class AuthService {

    static login(email: string, password: string): Promise<string | AuthInfo> {
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        };

        return fetch(url, requestOptions)
            .then(r => handleResponse<string | AuthInfo>(r), handleError)
            .then((user: AuthInfo) => {
                if (user && user.token) {
                    localStorage.setItem('user', JSON.stringify(user));
                }
                return user;
            });
    }

    static logout() {
        localStorage.removeItem('user');
    }
}