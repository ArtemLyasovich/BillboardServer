﻿namespace BillboardServer.Consts;

public class Consts
{
    internal const string DEFAULT_MESSAGE = "Message queue is empty. Send a POST request to display your message.";

    internal const string ERROR_EMPTY_MESSAGE = "Message cannot be empty or whitespace.";
    internal const string SUCCESS_MESSAGE_ENQUEUED = "Message enqueued successfully.";

    internal const string APPLICATION_STARTUP_LOGID = "ApplicationStartup";
    internal const string APPLICATION_SHUTDOWN_LOGID = "ApplicationShutdown";
    internal const string REPOSITORY_LOGID = "Repository";
    internal const string CONTROLLER_LOGID = "Controller";
    internal const string MESSAGE_DISPLAY_SERVICE_LOGID = "MessageDisplayService";
}
