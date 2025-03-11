using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace send_ethernet
{
    public static class EthernetCommand
    {
        private static TcpClient _client;
        private static NetworkStream _stream;
        private static Timer _reconnectTimer;

        public static void Connect(string ipAddress, int port)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, port);
            _stream = _client.GetStream();
            StartReconnectTimer(ipAddress, port);
        }

        public static bool IsConnected()
        {
            return _client != null && _client.Connected;
        }

        public static bool SendPacket(List<byte> packet, List<byte> expectedResponse)
        {
            EnsureConnected();

            _stream.Write(packet.ToArray(), 0, packet.Count);

            byte[] responseBuffer = new byte[1024];
            int bytesRead = _stream.Read(responseBuffer, 0, responseBuffer.Length);

            List<byte> response = responseBuffer.Take(bytesRead).ToList();

            return response.ContainsSequence(expectedResponse);
        }

        private static void EnsureConnected()
        {
            if (_client == null || !_client.Connected)
            {
                throw new InvalidOperationException("Not connected to any server.");
            }
        }

        private static void StartReconnectTimer(string ipAddress, int port)
        {
            _reconnectTimer = new Timer(state =>
            {
                if (_client == null || !_client.Connected)
                {
                    try
                    {
                        _client = new TcpClient();
                        _client.Connect(ipAddress, port);
                        _stream = _client.GetStream();
                    }
                    catch
                    {
                        // Handle reconnection failure (e.g., log the error)
                    }
                }
            }, null, 0, 10000); // Attempt to reconnect every 10 seconds
        }

        private static bool ContainsSequence(this List<byte> source, List<byte> sequence)
        {
            for (int i = 0; i <= source.Count - sequence.Count; i++)
            {
                if (source.Skip(i).Take(sequence.Count).SequenceEqual(sequence))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

