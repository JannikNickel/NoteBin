import "../css/user.css";
import "../css/toolbar.css";
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from "react-router-dom";
import { ChevronDoubleRightIcon, ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import ToastContainer from "../components/ToastContainer";
import { showErrorToast } from "../utils/toast-utils";
import { apiRequest, Note, NoteListRequest, NoteListResponse, User } from "../api";
import { deleteAuthData, getUser } from "../utils/storage";
import { formatTimeAgo } from "../utils/time-utils";
import { getLanguageDisplayName } from "../language";

const PAGE_SIZE: number = 25;

const UserPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [notes, setNotes] = useState<Note[]>([]);
    const [totalNotes, setTotalNotes] = useState<number>(0);
    const [currPage, setCurrPage] = useState<number>(1);
    const navigate = useNavigate();

    const handleLogout = async () => {
        const response = await apiRequest<{}, {}>(`/api/auth`, {}, {
            method: "DELETE"
        }, true);
        if (response.ok) {
            deleteAuthData();
            setIsAuthenticated(false);
            navigate("/");
        } else {
            showErrorToast(response.error.message);
        }
    };

    const formatDate = (utcMilliseconds: number) => {
        const date = new Date(utcMilliseconds);
        return date.toISOString().split('T')[0];
    };

    const totalPages = () => Math.ceil(totalNotes / PAGE_SIZE);

    const changePage = (offset: number) => {
        const page = currPage + offset;
        setCurrPage(Math.min(Math.max(page, 1), totalPages()));
    };

    const queryNotes = async (page: number) => {
        if (!id) {
            return;
        }
        const request: NoteListRequest = {
            offset: (page - 1) * PAGE_SIZE,
            amount: PAGE_SIZE,
            owner: id
        };
        const response = await apiRequest<NoteListRequest, NoteListResponse>(`/api/note/list`, request, {
            method: "GET"
        });
        if (response.ok) {
            setNotes(response.value.notes);
            setTotalNotes(response.value.total);
        } else {
            showErrorToast(response.error.message);
        }
    };

    const fetchUser = async () => {
        setLoading(true);
        setError(null);

        const response = await apiRequest<{}, User>(`/api/user/${id}`, {}, {
            method: "GET"
        });
        if (response.ok) {
            setUser(response.value);
        } else {
            setError(response.error.message);
        }
        setLoading(false);
    };

    const validateUser = async () => {
        const response = await apiRequest<{}, {}>("/api/auth", {}, { method: "GET" }, true);
        setIsAuthenticated(response.ok);
    };

    const handleNoteClick = (noteId: string) => {
        navigate(`/note/${noteId}`);
    };

    useEffect(() => {
        fetchUser();
    }, [id]);

    useEffect(() => {
        if (id && id === getUser()) {
            validateUser();
        }
    }, []);

    useEffect(() => {
        queryNotes(currPage);
    }, [id, currPage]);

    if (loading) {
        return <p className="p-2">Loading...</p>
    }
    if (error) {
        return <p className="p-2">Error: {error}</p>
    }

    return (
        <>
            <div className="user-note-container">
                <div className="note-container">
                    {notes.map((note, index) => (
                        <div key={index} className="note-item">
                            <button className="note-header secondary select-none" onClick={() => handleNoteClick(note.id)}>
                                <div>
                                    [{note.name || "UNTITLED"}]
                                </div>
                                <div>
                                    {getLanguageDisplayName(note.syntax)}
                                </div>
                                <div>
                                    {formatTimeAgo(note.creationTime)}
                                </div>
                            </button>
                            <div className="note-content">
                                {note.content}
                            </div>
                        </div>
                    ))}
                </div>
                <div className="toolbar toolbar-left toolbar-absolute">
                    <input
                        className="toolbar-element secondary min-w-60"
                        value={""}
                        placeholder="Search notes..."
                        maxLength={32} />
                    <button className="toolbar-element secondary" onClick={() => changePage(-1)}>
                        <ChevronLeftIcon className="h-4 w-4" />
                    </button>
                    <div className="toolbar-element secondary select-none">
                        {currPage} / {totalPages()}
                    </div>
                    <button className="toolbar-element secondary" onClick={() => changePage(1)}>
                        <ChevronRightIcon className="h-4 w-4" />
                    </button>
                    <button className="toolbar-element secondary" onClick={() => changePage(100000000)}>
                        <ChevronDoubleRightIcon className="h-4 w-4" />
                    </button>
                </div>
                {user &&
                    <div className="toolbar toolbar-absolute">
                        {isAuthenticated &&
                            <button
                                className="toolbar-element primary"
                                onClick={handleLogout}>
                                    LOGOUT
                            </button>
                        }
                        <div className="toolbar-element secondary pointer-events-none">
                            JOINED: [{user.creationTime && formatDate(user.creationTime)}]
                        </div>
                        <div className="toolbar-element secondary pointer-events-none">
                            {user?.username}
                        </div>
                    </div>
                }
            </div>
            <ToastContainer />
        </>
    )
};

export default UserPage;
