import { Err, Ok, Result } from "./utils/result";
import { getReasonPhrase } from "http-status-codes";

export interface NoteCreateRequest {
    name: string,
    syntax: string,
    content: string
}

export interface NoteCreateResponse {
    id: string
}

export interface Note {
    syntax: string,
    content: string
}

interface ErrorResponse {
    error: string
}

async function getErrorMessage(response: Response): Promise<string> {
    let empty = response.headers.get("Content-Length") !== "0";
    const errorInfo: ErrorResponse | null = empty ? await response.json() : null;
    const statusText = getReasonPhrase(response.status);
    return errorInfo?.error || `[${response.status}] ${statusText}`;
}

export async function apiRequest<TReq extends Object, TRes>(path: string, requestBody: TReq, options: RequestInit = {}): Promise<Result<TRes, string>> {
    const defHeaders = {
        "Content-Type": "application/json"
    };

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
            return Err(err);
        }

        const result: TRes = await response.json();
        return Ok(result);
    } catch (err) {
        console.error("apiRequest<TReq, TRes> error: ", err);
        return Err("An unknown error occured!");
    }
}
