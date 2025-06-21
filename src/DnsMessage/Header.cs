using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Header
{
    public ushort Id { get; set; }
    public bool QueryResponse { get; set; }
    public byte OpCode { get; set; }
    public bool AuthoritativeAnswer { get; set; }
    public bool TruncatedMessage { get; set; }
    public bool RecursionDesired { get; set; }
    public bool RecursionAvailable { get; set; }
    public byte Reserved { get; set; }
    public byte ResponseCode { get; set; }
    public ushort QuestionCount { get; set; }
    public ushort AnswerCount { get; set; }
    public ushort AuthorityCount { get; set; }
    public ushort AdditionalCount { get; set; }

    public Header()
    {
        Id = 0;
        QueryResponse = true;
        OpCode = 0;
        AuthoritativeAnswer = false;
        TruncatedMessage = false;
        RecursionDesired = false;
        RecursionAvailable = false;
        Reserved = 0;
        ResponseCode = 0;
        QuestionCount = 0;
        AnswerCount = 0;
        AuthorityCount = 0;
        AdditionalCount = 0;
    }

    public Header(ushort id,
        bool queryResponse,
        byte opCode,
        bool authoritativeAnswer,
        bool truncatedMessage,
        bool recursionDesired,
        bool recursionAvailable,
        byte reserved,
        byte responseCode,
        ushort questionCount,
        ushort answerCount,
        ushort authorityCount = 0,
        ushort additionalCount = 0)
    {
        Id = id;
        QueryResponse = queryResponse;
        OpCode = opCode;
        AuthoritativeAnswer = authoritativeAnswer;
        TruncatedMessage = truncatedMessage;
        RecursionDesired = recursionDesired;
        RecursionAvailable = recursionAvailable;
        Reserved = reserved;
        ResponseCode = responseCode;
        QuestionCount = questionCount;
        AnswerCount = answerCount;
        AuthorityCount = authorityCount;
        AdditionalCount = additionalCount;
    }
    
    public void Decode(byte[] bytes)
    {
        Id = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(0, 2));
        DecodeFlags(bytes.AsSpan(2, 2).ToArray());
        QuestionCount = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(4, 2));
        AnswerCount = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(6, 2));
        AuthorityCount = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(8, 2));
        AdditionalCount = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(10, 2));
    }

    public void DecodeFlags(byte[] flagsByte)
    {
        QueryResponse = (flagsByte[0] & 0b10000000) != 0; // Bit 7
        OpCode = (byte)((flagsByte[0] >> 3) & 0b00001111); // Bits 3-6
        AuthoritativeAnswer = (flagsByte[0] & 0b00000100) != 0; // Bit 2
        TruncatedMessage = (flagsByte[0] & 0b00000010) != 0; // Bit 1
        RecursionDesired = (flagsByte[0] & 0b00000001) != 0; // Bit 
        RecursionAvailable = (flagsByte[1] & 0b10000000) != 0;
        Reserved = (byte)((flagsByte[1] >> 4) & 0b00000111);
        ResponseCode = (byte)(flagsByte[1] & 0b00001111);
    }

    public byte[] Encode()
    {
        byte[] headerBytes = new byte[12];
        
        BinaryPrimitives.WriteUInt16BigEndian(headerBytes.AsSpan(0), Id);

        var flags = EncodeFlags();
        flags.CopyTo(headerBytes, 2);
        
        BinaryPrimitives.WriteUInt16BigEndian(headerBytes.AsSpan(4), QuestionCount);
        BinaryPrimitives.WriteUInt16BigEndian(headerBytes.AsSpan(6), AnswerCount);
        BinaryPrimitives.WriteUInt16BigEndian(headerBytes.AsSpan(8), AuthorityCount);
        BinaryPrimitives.WriteUInt16BigEndian(headerBytes.AsSpan(10), AdditionalCount);
        
        return headerBytes;
    }

    private byte[] EncodeFlags()
    {
        byte[] result = new byte[2];
        
        result[0] |= (1 << 7);
    
        // OpCode (bits 6-3, 4 bits)
        result[0] |= (byte)((OpCode & 0x0F) << 3);
        
        /*if (AuthoritativeAnswer)
            result[0] |= (1 << 2);
        
        if (TruncatedMessage)
            result[0] |= (1 << 1);*/
    
        // RecursionDesired (bit 0, rightmost)
        if (RecursionDesired)
            result[0] |= 1;
        
        /*if(RecursionAvailable)
            result[1] |= (1 << 7);
        
        result[1] |= (byte)((Reserved & 0b111) << 4);*/
        
        result[1] |= (byte)(ResponseCode & 0b1111);
        
        return result;
    }
}