import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Box } from '@mui/material';

function Login() {
    const [email, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            // Simula uma requisição de login
            const response = await fetch('http://localhost:5074/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('token', data.token); // Armazena o token JWT
                navigate('/chat'); // Redireciona para a tela de chat
            } else {
                setError('Credenciais inválidas');
            }
        } catch (err) {
            setError('Erro ao conectar ao servidor');
        }
    };

    return (
        <Container maxWidth="sm">
            <Box sx={{ marginTop: 8, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <Typography variant="h4" gutterBottom>
                    Login
                </Typography>
                <TextField
                    fullWidth
                    label="Usuário"
                    variant="outlined"
                    margin="normal"
                    value={email}
                    onChange={(e) => setUsername(e.target.value)}
                />
                <TextField
                    fullWidth
                    label="Senha"
                    type="password"
                    variant="outlined"
                    margin="normal"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                {error && <Typography color="error">{error}</Typography>}
                <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    onClick={handleLogin}
                    sx={{ marginTop: 2 }}
                >
                    Entrar
                </Button>
            </Box>
        </Container>
    );
}

export default Login;