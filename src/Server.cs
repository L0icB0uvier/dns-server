using System.Net;
using System.Net.Sockets;
using dns_server.DnsMessage;

namespace dns_server;

public static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Logs from your program will appear here!");

        // Resolve UDP address
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 2053;
        IPEndPoint udpEndPoint = new IPEndPoint(ipAddress, port);

        // Create UDP socket
        UdpClient udpClient = new UdpClient(udpEndPoint);

        while (true)
        {
            Console.WriteLine("Request Starts");
            // Receive data
            IPEndPoint sourceEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedData = udpClient.Receive(ref sourceEndPoint);

            string receivedBytes = BitConverter.ToString(receivedData);
            Console.WriteLine($"Received {receivedData.Length} bytes from {sourceEndPoint}: {receivedBytes}");

            var receivedMessage = new DnsMessage.DnsMessage();
            receivedMessage.Decode(receivedData);

            var responseMessage = CreateResponseMessage(receivedMessage);

            byte[] response = responseMessage.Encode();
            string responseBytes = BitConverter.ToString(response);
            Console.WriteLine($"Sending {responseBytes.Length} bytes to {sourceEndPoint}: {responseBytes}");

            // Send response
            udpClient.Send(response, response.Length, sourceEndPoint);
            Console.WriteLine("Request Ends");
        }
    }

    private static DnsMessage.DnsMessage CreateResponseMessage(DnsMessage.DnsMessage receivedMessage)
    {
        return new DnsMessage.DnsMessage(
            new Header(receivedMessage.Header.Id,
                true,
                receivedMessage.Header.OpCode,
                false,
                false,
                receivedMessage.Header.RecursionDesired,
                false,
                0,
                receivedMessage.Header.OpCode == 0 ? Convert.ToByte(0) : Convert.ToByte(4),
                1,
                1),
            new List<Question>(){new Question(receivedMessage.Questions[0].Name.DomainName, 1, 1)},
            new List<Answer>() {new Answer(receivedMessage.Questions[0].Name.DomainName)},
            new Authority(),
            new Additional());
    }
}