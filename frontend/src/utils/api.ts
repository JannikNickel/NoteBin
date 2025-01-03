import { getReasonPhrase } from "http-status-codes";
import { Err, Ok, Result } from "../utils/result";
import { getAuthToken } from "../utils/storage";

export interface NoteCreateRequest {
    name?: string;
    fork?: string;
    syntax: string;
    content: string;
}

export interface NoteCreateResponse {
    id: string;
}

export interface Note {
    id: string;
    name: string;
    owner?: string;
    fork?: string;
    creationTime: number;
    syntax: string;
    content?: string;
}

export interface AuthRequest {
    username: string;
    password: string;
}

export interface AuthResponse {
    token: string;
}

export interface UserRequest {
    username: string;
}

export interface User {
    username: string;
    creationTime: number;
}

export interface NoteListRequest {
    offset: number;
    amount: number;
    owner?: string;
    filter?: string;
}

export interface NoteListResponse {
    notes: Note[];
    total: number;
};

export interface RequestError {
    statusCode?: number;
    message: string;
};

interface ErrorResponse {
    error: string;
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

    const isEmptyReq = !requestBody || Object.keys(requestBody).length === 0;
    const isGet = options.method === "GET";
    if (isGet && !isEmptyReq) {
        const queryParams = new URLSearchParams();
        Object.entries(requestBody).forEach(([key, value]) => {
            if (value !== undefined) {
                queryParams.append(key, value as string);
            }
        });
        path += `?${queryParams.toString()}`;
    }
    
    let body = !isGet && !isEmptyReq ? { body: JSON.stringify(requestBody) } : {};
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
        return Err({ message: "An unknown error occurred!" } as RequestError);
    }
}
