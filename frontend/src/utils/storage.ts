const USER: string = "user";
const SESSION: string = "session";

export const setUser = (user: string): void => {
    localStorage.setItem(USER, user);
};

export const getUser = (): string | null => {
    return localStorage.getItem(USER);
};

export const setAuthToken = (token: string): void => {
    localStorage.setItem(SESSION, token);
};

export const getAuthToken = (): string | null => {
    return localStorage.getItem(SESSION);
};
