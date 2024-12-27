import { Err, Ok, Result } from "./utils/result";

export interface NoteCreateRequest {
    syntax: string,
    content: string
}

export interface NoteCreateResponse {
    id: string
}

interface ErrorResponse {
    error: string
}

async function getErrorMessage(response: Response): Promise<string> {
    let empty = response.headers.get("Content-Length") !== "0";
    const errorInfo: ErrorResponse | null = empty ? await response.json() : null;
    return errorInfo?.error || `[${response.status}] ${response.statusText}`;
}

export async function apiRequest<TReq, TRes>(path: string, requestBody: TReq, options: RequestInit = {}): Promise<Result<TRes, string>> {
    const defHeaders = {
        "Content-Type": "application/json"
    };
    const mergedOptions: RequestInit = {
        headers: {
            ...defHeaders,
            ...(options.headers || {})
        },
        body: JSON.stringify(requestBody),
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
