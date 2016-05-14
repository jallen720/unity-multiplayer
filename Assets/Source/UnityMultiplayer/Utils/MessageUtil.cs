using System.Collections.Generic;

public static class MessageUtil {
    private const byte MESSAGE_PROTOCOL_VERSION = 1;

    // MESSAGE_PROTOCOL_VERSION   1
    // message type (enum : byte) 1
    // ----------------------------
    // total                      2
    public const int MESSAGE_DATA_START_INDEX = 2;

    public static void InitMessage(List<byte> message, byte messageType) {
        message.Clear();
        message.Add(MESSAGE_PROTOCOL_VERSION);
        message.Add(messageType);
    }

    public static void ValidateMessage(byte[] message, List<byte> validTypes) {
        const int MESSAGE_PROTOCOL_VERSION_INDEX = 0;

        byte messageProtocolVersion = message[MESSAGE_PROTOCOL_VERSION_INDEX];
        byte messageType = GetMessageType(message);

        if (messageProtocolVersion != MESSAGE_PROTOCOL_VERSION) {
            DebugUtil.Log(string.Format(
                "message protocol version [{0}] is invalid",
                messageProtocolVersion
            ));
        }

        if (!validTypes.Contains(messageType)) {
            DebugUtil.Log(string.Format("message type [{0}] is invalid", messageType));
        }
    }

    public static byte GetMessageType(byte[] message) {
        const int MESSAGE_TYPE_INDEX = 1;

        return message[MESSAGE_TYPE_INDEX];
    }
}