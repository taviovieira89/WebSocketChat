import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, TextField, Button, List, ListItem, ListItemText, Typography, Paper, Box } from '@mui/material';
import { v4 as uuidv4 } from 'uuid';

function Chat() {
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const [roomId, setRoomId] = useState('');
    const [connected, setConnected] = useState(false);
    const webSocket = useRef(null);
    const navigate = useNavigate();

    useEffect(() => {
        // Gera um ID de sala automaticamente
        const generatedRoomId = uuidv4();
        setRoomId(generatedRoomId);
    }, []);

    const connectToRoom = () => {
        const token = localStorage.getItem('token');
        if (!token) {
            navigate('/login');
            return;
        }

        if (webSocket.current) {
            webSocket.current.close();
        }

        webSocket.current = new WebSocket(`ws://localhost:5074/ws?token=${token}&room=${roomId}`);

        webSocket.current.onopen = () => {
            setConnected(true);
            console.log(`Conectado à sala: ${roomId}`);
        };

        webSocket.current.onmessage = (event) => {
            setMessages((prevMessages) => [...prevMessages, event.data]);
        };

        webSocket.current.onclose = () => {
            setConnected(false);
            console.log(`Desconectado da sala: ${roomId}`);
        };
    };

    const sendMessage = () => {
        if (webSocket.current && webSocket.current.readyState === WebSocket.OPEN && inputValue) {
            webSocket.current.send(inputValue);
            setInputValue('');
        }
    };

    return (
        <Container maxWidth="md">
            <Paper elevation={3} sx={{ p: 3, borderRadius: 2 }}>
                <Typography variant="h4" gutterBottom align="center">
                    Chat em Tempo Real
                </Typography>

                {/* Mostrar ID da sala gerado automaticamente */}
                <Box display="flex" gap={2} mb={2} alignItems="center">
                    <TextField
                        fullWidth
                        label="ID da Sala"
                        variant="outlined"
                        value={roomId}
                        disabled
                    />
                    <Button variant="contained" color="secondary" onClick={connectToRoom} disabled={connected}>
                        {connected ? "Conectado" : "Entrar"}
                    </Button>
                </Box>

                {/* Exibir mensagens e entrada de texto somente se conectado */}
                {connected && (
                    <>
                        <Paper
                            elevation={2}
                            sx={{
                                height: 300,
                                overflowY: 'auto',
                                p: 2,
                                mb: 2,
                                backgroundColor: '#f5f5f5',
                            }}
                        >
                            <List>
                                {messages.map((message, index) => (
                                    <ListItem key={index} sx={{ borderBottom: '1px solid #ddd' }}>
                                        <ListItemText primary={message} />
                                    </ListItem>
                                ))}
                            </List>
                        </Paper>

                        <Box display="flex" gap={2}>
                            <TextField
                                fullWidth
                                variant="outlined"
                                placeholder="Digite sua mensagem..."
                                value={inputValue}
                                onChange={(e) => setInputValue(e.target.value)}
                            />
                            <Button variant="contained" color="primary" onClick={sendMessage}>
                                Enviar
                            </Button>
                        </Box>
                    </>
                )}
            </Paper>
        </Container>
    );
}

export default Chat;
