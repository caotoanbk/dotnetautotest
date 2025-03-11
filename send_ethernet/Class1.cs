using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace send_ethernet
{
    public static class EthernetCommand
    {
        private static TcpClient _client;
        private static NetworkStream _stream;
        private static Timer _reconnectTimer;

        public static async Task<bool> Connect(string ipAddress, int port, int timeoutMilliseconds = 5000)
        {
            _client = new TcpClient();
            var cts = new CancellationTokenSource(timeoutMilliseconds);

            try
            {
                await _client.ConnectAsync(ipAddress, port).WaitAsync(cts.Token);
                _stream = _client.GetStream();
                StartReconnectTimer(ipAddress, port);
                return true;
            }
            catch (OperationCanceledException)
            {
                // Connection attempt timed out
                _client.Close();
                _client = null;
                return false;
            }
            catch
            {
                // Other connection errors
                _client.Close();
                _client = null;
                return false;
            }
        }

        public static bool IsConnected()
        {
            return _client != null && _client.Connected;
        }

        public static bool SendPacket(List<byte> packet, List<byte> expectedResponse)
        {
            if (!EnsureConnected())
            {
                throw new InvalidOperationException("Not connected to any server.");
            }

            PrintHex("Sending packet: ", packet);
            _stream.Write(packet.ToArray(), 0, packet.Count);

            byte[] responseBuffer = new byte[1024];
            int bytesRead = _stream.Read(responseBuffer, 0, responseBuffer.Length);

            List<byte> response = responseBuffer.Take(bytesRead).ToList();
            PrintHex("Received response: ", response);

            if (expectedResponse == null || expectedResponse.Count == 0)
            {
                return true;
            }

            return response.ContainsSequence(expectedResponse);
        }

        private static bool EnsureConnected()
        {
            if (_client == null || !_client.Connected)
            {
                try
                {
                    // Attempt to reconnect
                    _client = new TcpClient();
                    _client.Connect("160.48.199.98", 20000); // Replace with actual IP and port
                    _stream = _client.GetStream();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static void Disconnect()
        {
            _reconnectTimer?.Dispose();
            _stream?.Close();
            _client?.Close();
            _client = null;
            _stream = null;
        }

        private static void StartReconnectTimer(string ipAddress, int port)
        {
            _reconnectTimer = new Timer(async state =>
            {
                if (_client == null || !_client.Connected)
                {
                    try
                    {
                        await Connect(ipAddress, port);
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

        private static void PrintHex(string prefix, List<byte> data)
        {
            Console.WriteLine(prefix + BitConverter.ToString(data.ToArray()).Replace("-", " "));
        }
    }
}

