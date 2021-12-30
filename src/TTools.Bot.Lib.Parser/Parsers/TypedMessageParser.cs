using System.Collections.Immutable;
using TTools.Bot.Lib.Parser.Models;

namespace TTools.Bot.Lib.Parser.Parsers;

public static class TypedMessageParser
{
    public static IIrcMessage ParseMessageToTyped(ReadOnlyMemory<char> rawMessage)
    {
        var parsed = RawMessageParser.ParseRawMessage(rawMessage);

        if (parsed.Command.Span.SequenceEqual("PRIVMSG"))
            return ParsePrivateMessage(parsed);
        if (parsed.Command.Span.SequenceEqual("PING"))
            return new PingMessage(parsed, parsed.Parameters);

        return parsed;
    }

    public static PrivateMessage ParsePrivateMessage(RawIrcMessage message)
    {
        // Username can be extracted from the host prefix (username!username@username.tmi.twitch.tv)
        var username = message.Prefix[..message.Prefix.Span.IndexOf('!')];

        // TODO: handle # being pushed out a char (don't know what this means)
        var separatorIndex = message.Parameters.Span.IndexOf(':');
        if (separatorIndex == -1)
            separatorIndex = message.Parameters.Span.LastIndexOf(" ");

        var channel = message.Parameters[1..(separatorIndex - 1)];
        var content = message.Parameters[(separatorIndex + 1)..];

        var parsedTags = ParseTagsToDictionary(message.Tags);
        return new PrivateMessage(message, username, channel, content, parsedTags);
    }

    private static IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> ParseTagsToDictionary(ReadOnlyMemory<char> tags)
    {
        if (tags.Length == 0)
        {
            return
                new Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>(0)
                    .ToImmutableDictionary();
        }

        var parsedTags = new Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>();
        var remainingTags = tags;

        // Go through the memory using the index of the next semicolon
        do
        {
            var endOfTagIndex = remainingTags.Span.IndexOf(';');

            if (endOfTagIndex == -1)
                endOfTagIndex = remainingTags.Length;

            var currentTag = remainingTags[..endOfTagIndex];
            var tagKey = currentTag[..currentTag.Span.IndexOf('=')];
            var tagValue = currentTag[(tagKey.Length + 1)..];

            parsedTags.Add(tagKey, tagValue);

            // Final index will be larger than the Span's length, this clamps it
            var realEndIndex = Math.Min(remainingTags.Length, endOfTagIndex + 1);

            // Remove the parsed tag from the working string
            remainingTags = remainingTags[realEndIndex..];
        } while (remainingTags.Length > 0);

        return parsedTags.ToImmutableDictionary();
    }
}
