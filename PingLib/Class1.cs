using System.Net.NetworkInformation;    

namespace PingLib
{
    public static class PingClass
    {
        public static bool PingHost(string nameOrAddress)
        {
            try
            {
                using (Ping pinger = new Ping())
                {
                    int timeout = 1000; // Timeout in milliseconds (1 second)
                    PingReply reply = pinger.Send(nameOrAddress,timeout);

                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}

