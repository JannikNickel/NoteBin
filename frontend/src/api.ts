import { Err, Ok, Result } from "./utils/result";
import { getReasonPhrase } from "http-status-codes";
import { getAuthToken } from "./utils/storage";

export interface NoteCreateRequest {
    name: string,
    syntax: string,
    content: string
}

export interface NoteCreateResponse {
    id: string
}

export interface Note {
    name: string,
    owner: string | null,
    syntax: string,
    content: string
}

export interface UserRequest {
    username: string,
    password: string
}

export interface AuthResponse {
    token: string
}

export interface RequestError {
    statusCode: number | null,
    message: string
};

interface ErrorResponse {
    error: string
}

async function getErrorMessage(response: Response): Promise<string> {
    let empty = response.headers.get("Content-Length") !== "0";
    const errorInfo: ErrorResponse | null = empty ? await response.json() : null;
    const statusText = getReasonPhrase(response.status);
    return errorInfo?.error || `[${response.status}] ${statusText}`;
}

export async function apiRequest<TReq extends Object, TRes extends Object>(path: string, requestBody: TReq, options: RequestInit = {}, auth: boolean = false): Promise<Result<TRes, RequestError>> {
    const defHeaders: Record<string, string> = {
        "Content-Type": "application/json"
    };

    if (auth) {
        const token = getAuthToken();
        if (token) {
            defHeaders["Authorization"] = `Bearer ${token}`;
        }
    }

    const body = (requestBody && Object.keys(requestBody).length > 0) ? { body: JSON.stringify(requestBody) } : {}
    const mergedOptions: RequestInit = {
        headers: {
            ...defHeaders,
            ...(options.headers || {})
        },
        ...body,
        ...options
    };

    try {
        const response = await fetch(path, mergedOptions);
        
        if (!response.ok) {
            const err = await getErrorMessage(response);
            return Err({ statusCode: response.status, message: err } as RequestError);
        }

        if (response.headers.get("Content-Length") === "0") {
            return Ok({} as TRes);
        }

        const result: TRes = await response.json();
        return Ok(result);
    } catch (err) {
        console.error("apiRequest<TReq, TRes> error: ", err);
        return Err({ statusCode: null, message: "An unknown error occurred!" } as RequestError);
    }
}
