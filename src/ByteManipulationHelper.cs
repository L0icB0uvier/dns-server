using System.Buffers.Binary;

namespace dns_server;

public static class ByteManipulationHelper
{
    public static void AddUShortToList(ushort value, List<byte> list)
    {
        byte[] bytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        list.AddRange(bytes);
    }
    
    public static void AddUIntToList(uint value, List<byte> list)
    {
        byte[] bytes = new byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        list.AddRange(bytes);
    }
}