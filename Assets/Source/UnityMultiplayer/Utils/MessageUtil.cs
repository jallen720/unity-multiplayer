using System.Collections.Generic;

public static class MessageUtil {
    private const byte MESSAGE_PROTOCOL_VERSION = 1;
    private const byte MESSAGE_TYPE = (byte)'U';
    public const int MESSAGE_DATA_START_INDEX = 2;

    public static void InitMessage(List<byte> message) {
        message.Clear();
        message.Add(MESSAGE_PROTOCOL_VERSION);
        message.Add(MESSAGE_TYPE);
    }

    public static void ValidateMessage(byte[] message) {
        const int MESSAGE_PROTOCOL_VERSION_INDEX = 0;
        const int MESSAGE_TYPE_INDEX = 1;

        byte messageProtocolVersion = message[MESSAGE_PROTOCOL_VERSION_INDEX];
        char messageType = (char)message[MESSAGE_TYPE_INDEX];

        if (messageProtocolVersion != MESSAGE_PROTOCOL_VERSION) {
            DebugUtil.Log(string.Format(
                "message protocol version [{0}] is invalid",
                messageProtocolVersion
            ));
        }

        if (messageType != MESSAGE_TYPE) {
            DebugUtil.Log(string.Format(
                "message type [{0}] is invalid",
                messageType
            ));
        }
    }
}