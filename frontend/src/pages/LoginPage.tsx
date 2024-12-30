import "../css/login.css";
import React, { useState } from "react";

const LoginPage: React.FC = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        //TODO
    };

    return (
        <div className="flex justify-center items-center h-screen">
            <div className="form-container w-80 p-5 bg-secondary-default rounded-lg">
                <form onSubmit={handleSubmit}>
                    <p className="w-full mb-8 text-3xl">[LOGIN]</p>
                    <div className="form-group">
                        <label htmlFor="username">USERNAME:</label>
                        <input
                            className="secondary"
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">PASSWORD:</label>
                        <input
                            className="secondary"
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required />
                    </div>
                    <div className="form-group">
                        <button className="primary" type="submit">LOGIN</button>
                    </div>
                    <div className="separator" />
                    <button className="text-button secondary center" type="button">[CREATE ACCOUNT]</button>
                </form>
            </div>
        </div>
    );
};

export default LoginPage;
