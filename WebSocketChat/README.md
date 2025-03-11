# WebSocketChat Project

## Overview
WebSocketChat is a real-time chat application that utilizes WebSocket technology to enable instant messaging between users. The application is built using ASP.NET Core and follows a modular architecture, allowing for easy maintenance and scalability.

## Features
- Real-time messaging using WebSockets
- User management and authentication
- SQLite database for data persistence
- CORS support for cross-origin requests

## Project Structure
```
WebSocketChat
├── Infra
│   └── DependencyInjection.cs
├── Dockerfile
├── WebSocketChat.csproj
└── README.md
```

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- Docker (for containerization)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd WebSocketChat
   ```

2. **Build the application:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

### Docker Setup

To build and run the application using Docker, follow these steps:

1. **Build the Docker image:**
   ```bash
   docker build -t websocketchat .
   ```

2. **Run the Docker container:**
   ```bash
   docker run -d -p 80:80 websocketchat
   ```

### Usage
Once the application is running, you can access it via `http://localhost` in your web browser. You can connect to the WebSocket endpoint at `/ws` to start sending and receiving messages.

## Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.