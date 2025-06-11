using System.Net;
using System.Net.Sockets;
using System.Text;
using dns_server.DnsMessage;

// You can use print statements as follows for debugging, they'll be visible when running tests.
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
    
    var message = new DnsMessage(receivedData);
    // Create an empty response
    byte[] response = message.Encode();
    string responseBytes = BitConverter.ToString(response);
    Console.WriteLine($"Sending {responseBytes.Length} bytes to {sourceEndPoint}: {responseBytes}");
    
    // Send response
    udpClient.Send(response, response.Length, sourceEndPoint);
    Console.WriteLine("Request Ends");

}