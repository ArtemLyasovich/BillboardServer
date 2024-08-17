# BillboardServer

BillboardServer is a server application designed to manage and display messages on a billboard. It accepts messages via an API, stores them in a queue, and handles their display on the billboard. The application also includes logging for key operations to facilitate monitoring and debugging.

## Key Components

### 1. MessageQueueService
- **Description**: Manages the queue of messages to be displayed on the billboard.
- **Functions**:
  - Adds new messages to the queue.
  - Retrieves messages from the queue for display.

### 2. MessageDisplayService
- **Description**: Responsible for displaying the current message on the billboard.
- **Functions**:
  - Periodically updates the message on the billboard by fetching the next one from the queue.
  - Displays a default message if the queue is empty.

### 3. BillboardRepository
- **Description**: Handles database interactions.
- **Functions**:
  - Loads and saves messages from/to the database.
  - Saves remaining messages in the queue when the server is stopping.

### 4. BillboardController
- **Description**: Provides the API for interacting with the server.
- **Functions**:
  - `GET /billboard`: Retrieves the currently displayed message.
  - `POST /billboard`: Adds a new message to the queue.

## Installation and Running

### 1. Clone the repository
```bash
git clone https://github.com/ArtemLyasovich/BillboardServer.git
cd BillboardServer
```

### 2. Install dependencies
Use the [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet) to install the required packages:
```bash
dotnet restore
```

### 3. Configure the database
Set up the database connection in appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=your_host;Database=your_db;Username=your_user;Password=your_password"
}
```

### 4. Run the application
Start the application using the following command:

```bash
dotnet run
```

## Usage

### Get the Current Message

To retrieve the current message from the billboard, send a GET request to:

```bash
GET /Billboard
```
Sample response:

```json
"Welcome to our billboard!"
```
### Add a New Message
To add a new message to the billboard, send a POST request to:

```bash
POST /billboard
```
Request body:

```json
"Your new message here"
```
Sample successful response:

```json
"Message successfully enqueued."
```

## Logging

The application uses [Serilog](https://serilog.net/) for logging to both the console and a file. Logs provide insights into:

- Server startup and shutdown
- Message queuing and dequeuing
- Database interactions

Logs are saved in the `Logs` directory with daily rotated files named:

```bash
Logs/myapp-{Date}.txt
```

## Server Shutdown

When shutting down the server (e.g., by pressing `Ctrl + C`), any remaining messages in the queue are saved to the database. This ensures that messages are preserved and not lost during the next startup.

## Contact Information

For any questions or support, please contact [artemlysovich@gmail.com](mailto:artemlysovich@gmail.com).
