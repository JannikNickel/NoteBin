import "../css/user.css";
import "../css/toolbar.css";
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from "react-router-dom";
import ToastContainer from "../components/ToastContainer";
import { showErrorToast } from "../utils/toast-utils";
import { apiRequest, User } from "../api";
import { deleteAuthData, getUser } from "../utils/storage";

const UserPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
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

    useEffect(() => {
        const fetchData = async () => {
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

        fetchData();
    }, [id]);

    useEffect(() => {
        const validateUser = async () => {
            const response = await apiRequest<{}, {}>("/api/auth", {}, { method: "GET" }, true);
            setIsAuthenticated(response.ok);
        };
        
        if (id && id === getUser()) {
            validateUser();
        }
    }, []);

    if (loading) {
        return <p className="p-2">Loading...</p>
    }
    if (error) {
        return <p className="p-2">Error: {error}</p>
    }

    return (
        <>
            <div className="flex justify-center items-center p-0"> 
                {/* TODO create list of notes */}
                {user &&
                    <div className="toolbar">
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
