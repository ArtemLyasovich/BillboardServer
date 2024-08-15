using BillboardServer.Models;

namespace BillboardServer.Extensions;

public static class MessageExtensions
{
    public static void NormalizeDates(this IEnumerable<Message> messages)
    {
        foreach (var message in messages)
        {
            message.CreatedAt = message.CreatedAt.Kind switch
            {
                DateTimeKind.Unspecified => DateTime.SpecifyKind(message.CreatedAt, DateTimeKind.Utc),
                DateTimeKind.Local => message.CreatedAt.ToUniversalTime(),
                _ => message.CreatedAt
            };
        }
    }
}