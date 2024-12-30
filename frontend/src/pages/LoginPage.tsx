import "../css/login.css";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

interface LoginPageProps {
    isSignup: boolean;
}

const LoginPage: React.FC<LoginPageProps> = ({ isSignup }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        //TODO
    };

    const handleSignupSwitch = () => {
        navigate(isSignup ? "/login" : "/signup");
    };

    return (
        <div className="flex justify-center items-center h-screen">
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
                            onChange={(e) => setUsername(e.target.value)}
                            required />
                    </div>
                    <div className="form-group">
                        <label className="select-none" htmlFor="password">PASSWORD:</label>
                        <input
                            className="secondary"
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required />
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
                        <button className="primary" type="submit">{isSignup ? "CREATE ACCOUNT" : "LOGIN"}</button>
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
    );
};

export default LoginPage;
