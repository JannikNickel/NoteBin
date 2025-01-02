import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import UploadPage from "./pages/UploadPage";
import NotePage from "./pages/NotePage";
import UserPage from "./pages/UserPage";
import LoginPage from "./pages/LoginPage";

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<UploadPage />} />
                <Route path="/note/:id" element={<NotePage />} />
                <Route path="/user/:id" element={<UserPage />} />
                <Route path="/login" element={<LoginPage isSignup={false} />} />
                <Route path="/signup" element={<LoginPage isSignup={true} />} />
            </Routes>
        </Router>
    )
};

export default App;
