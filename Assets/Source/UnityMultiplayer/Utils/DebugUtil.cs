using System.Linq;
using UnityEngine;

public static class DebugUtil {
    private const string SEPARATOR = "//////////////////////////////////////////////////////////\n";
    private const string LINE_PREFIX = "// ";

    private const string MESSAGE_TEMPLATE =
        "\n" +
        SEPARATOR +
        LINE_PREFIX + "\n" +
        "{0}" +
        LINE_PREFIX + "\n" +
        SEPARATOR;

    private const string MESSAGE_LINE_TEMPLATE = LINE_PREFIX + "{0}\n";

    public static void Log(string message) {
        Log(Lines(message));
    }

    public static string[] Lines(string str) {
        return str.Split(new char[] { '\n' });
    }

    public static void Log(string[] messageLines) {
        Debug.Log(FormattedMessage(messageLines));
    }

    private static string FormattedMessage(string[] messageLines) {
        return string.Format(MESSAGE_TEMPLATE, messageLines.Aggregate("", MessageLineAggregator));
    }

    private static string MessageLineAggregator(string message, string messageLine) {
        return message + FormatMessageLine(messageLine);
    }

    private static string FormatMessageLine(string messageLine) {
        return string.Format(MESSAGE_LINE_TEMPLATE, messageLine);
    }
}