import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import UploadPage from "./pages/UploadPage";
import NotePage from "./pages/NotePage";

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<UploadPage />} />
                <Route path="/note/:id" element={<NotePage />} />
            </Routes>
        </Router>
    )
};

export default App;
