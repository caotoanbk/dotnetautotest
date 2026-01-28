using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
// 1. Cấu hình địa chỉ IP của Máy B
string ipMayB = "10.224.89.245"; // <--- THAY BẰNG IP THẬT CỦA MÁY B
string hubUrl = $"http://{ipMayB}:5000/notificationHub";

// 2. Khởi tạo kết nối
var connection = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .WithAutomaticReconnect()
    .Build();

try 
{
    Console.WriteLine($"Đang kết nối tới Máy B tại: {hubUrl}...");
    await connection.StartAsync();
    Console.WriteLine("Kết nối THÀNH CÔNG!");

    while (true)
    {
        Console.Write("\nNhập nội dung thông báo (hoặc 'exit' để thoát): ");
        string? message = Console.ReadLine();

        if (message?.ToLower() == "exit") break;

        if (!string.IsNullOrEmpty(message))
        {
            // Gọi hàm 'SendToB' đã định nghĩa trên Hub của Máy B
            await connection.InvokeAsync("SendToB", message);
            Console.WriteLine("--> Đã gửi!");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"LỖI: Không thể kết nối. Hãy kiểm tra:");
    Console.WriteLine($"- IP {ipMayB} đã đúng chưa?");
    Console.WriteLine("- Máy B đã chạy Server chưa?");
    Console.WriteLine("- Firewall Máy B đã mở port 5000 chưa?");
    Console.WriteLine($"Chi tiết lỗi: {ex.Message}");
}
finally 
{
    await connection.DisposeAsync();
}
