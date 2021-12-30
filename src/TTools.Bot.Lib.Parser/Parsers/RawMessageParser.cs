using TTools.Bot.Lib.Parser.Models;

namespace TTools.Bot.Lib.Parser.Parsers;

public static class RawMessageParser
{
    public static RawIrcMessage ParseRawMessage(ReadOnlyMemory<char> rawMessage)
    {
        // FIXME: return ':' for future parsers to handle
        var (tags, postTags) = ParseTags(rawMessage);
        var (possiblePrefix, postPrefix) = SliceNextSpace(postTags);

        // TODO: bounds, may fail on some message types
        if (possiblePrefix.Span[0] != ':')
            return new RawIrcMessage(rawMessage, tags, ReadOnlyMemory<char>.Empty, possiblePrefix, postPrefix);

        var (command, parameters) = SliceNextSpace(postPrefix);
        return new RawIrcMessage(rawMessage, tags, possiblePrefix.TrimStart(":"), command, parameters);
    }

    private static (ReadOnlyMemory<char> tags, ReadOnlyMemory<char> post) ParseTags(ReadOnlyMemory<char> message)
    {
        // TODO: bounds check
        if (message.IsEmpty || message.Span[0] != '@')
            return (ReadOnlyMemory<char>.Empty, message);

        // We search for the first occurrence of ' :' which denotes the end of tags
        for (var messagePos = 0; messagePos < message.Length; messagePos++)
        {
            // FIXME: out of bounds
            if (message.Span[messagePos] != ' ')
                continue;

            if (message.Span[messagePos + 1] != ':')
                continue;

            return (message[1..messagePos], message[(messagePos+1)..]);
        }

        return (message, ReadOnlyMemory<char>.Empty);
    }

    private static (ReadOnlyMemory<char> segment, ReadOnlyMemory<char> post) SliceNextSpace(
        ReadOnlyMemory<char> message)
    {
        for (var messagePos = 0; messagePos < message.Length; messagePos++)
        {
            if (message.Span[messagePos] != ' ')
                continue;

            return (message[..messagePos], message[(messagePos + 1)..]);
        }

        return (message, ReadOnlyMemory<char>.Empty);
    }
}
