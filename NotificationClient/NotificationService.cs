using Microsoft.AspNetCore.SignalR.Client;

namespace MyUtils;

public class NotificationService
{
    // Dùng static để tái sử dụng kết nối (tránh tạo nhiều kết nối gây lag)
    private static HubConnection? _connection;

    public static async Task SendPopupAsync(string targetIp, string message)
    {
        if (_connection == null)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://{targetIp}:5000/notificationHub")
                .WithAutomaticReconnect()
                .Build();
        }

        if (_connection.State == HubConnectionState.Disconnected)
        {
            await _connection.StartAsync();
        }

        await _connection.InvokeAsync("SendToB", message);
    }
}
