import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './Login';
import Chat from './Chat';

function App() {
    const isAuthenticated = !!localStorage.getItem('token'); // Verifica se o usuário está autenticado

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route
                    path="/chat"
                    element={isAuthenticated ? <Chat /> : <Navigate to="/login" />}
                />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}

export default App;