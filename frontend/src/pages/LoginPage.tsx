import "../css/login.css";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { EyeIcon, EyeSlashIcon } from "@heroicons/react/24/outline";
import ToastContainer from "../components/ToastContainer";
import { clearToasts, showErrorToast, showSuccessToast } from "../utils/toast-utils";
import { apiRequest, AuthResponse, UserRequest } from "../api";
import { setAuthToken, setUser } from "../utils/storage";

interface LoginPageProps {
    isSignup: boolean;
}

const LoginPage: React.FC<LoginPageProps> = ({ isSignup }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    const singleErrorToast = (message: string) => {
        clearToasts();
        showErrorToast(message);
    };

    const handleLogin = async () => {
        const requestBody: UserRequest = { username, password };
        const response = await apiRequest<UserRequest, AuthResponse>("/api/auth", requestBody, {
            method: "POST"
        });

        if (response.ok) {
            setUser(requestBody.username);
            setAuthToken(response.value.token);
            navigate("/");
        } else {
            if (response.error.statusCode === 401) {
                singleErrorToast("Invalid username or password!");
            } else {
                singleErrorToast(response.error.message);
            }
        }
    };

    const handleSignup = async () => {
        if (password !== confirmPassword) {
            singleErrorToast("Passwords do not match!");
            return;
        }

        const requestBody: UserRequest = { username, password };
        const response = await apiRequest<UserRequest, {}>("/api/user", requestBody, {
            method: "POST"
        });

        if (response.ok) {
            setPassword("");
            showSuccessToast("Account created!");
            navigate("/login");
        } else {
            singleErrorToast(response.error.message);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await (isSignup ? handleSignup() : handleLogin());
    };

    const handleSignupSwitch = () => {
        navigate(isSignup ? "/login" : "/signup");
    };

    return (
        <>
            <div className="flex flex-col justify-center items-center h-screen">
                <ToastContainer className="relative top-2 left-0 right-0" />
                <div className="form-container w-80 p-5 bg-secondary-default rounded-lg">
                    <form onSubmit={handleSubmit}>
                        <p className="w-full mb-8 text-3xl select-none">{isSignup ? "[SIGN UP]" : "[LOGIN]"}</p>
                        <div className="form-group">
                            <label className="select-none" htmlFor="username">USERNAME:</label>
                            <input
                                className="secondary"
                                type="text"
                                id="username"
                                value={username}
                                maxLength={32}
                                onChange={(e) => setUsername(e.target.value)}
                                required />
                        </div>
                        <div className="form-group">
                            <label className="select-none" htmlFor="password">PASSWORD:</label>
                            <div className="relative">
                                <input
                                    className="secondary"
                                    type={showPassword ? "text" : "password"}
                                    id="password"
                                    value={password}
                                    maxLength={32}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required />
                                <button
                                    type="button"
                                    tabIndex={-1}
                                    className="secondary absolute right-3 top-[20%]"
                                    onClick={() => setShowPassword(!showPassword)}>
                                    {showPassword ? <EyeIcon className="h-5 w-5" /> : <EyeSlashIcon className="h-5 w-5" />}
                                </button>
                            </div>
                        </div>
                        {isSignup && password && (
                            <div className="form-group">
                                <label className="select-none" htmlFor="confirmPassword">CONFIRM PASSWORD:</label>
                                <input
                                    className="secondary"
                                    type="password"
                                    id="confirmPassword"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    required />
                            </div>
                        )}
                        <div className="form-group">
                            <button className="form-group-button primary" type="submit">{isSignup ? "CREATE ACCOUNT" : "LOGIN"}</button>
                        </div>
                        <div className="separator" />
                        <button
                            className="text-button secondary center"
                            type="button"
                            onClick={handleSignupSwitch}>
                            {isSignup ? "[LOGIN]" : "[CREATE ACCOUNT]"}
                        </button>
                    </form>
                </div>
            </div>
        </>
    );
};

export default LoginPage;
