using System.Collections.Immutable;
using TTools.Bot.Lib.Parser.Models;

namespace TTools.Bot.Lib.Parser.Parsers;

public static class IrcMessageParser
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
        var username = message.Prefix[..message.Prefix.Span.IndexOf('!')];
        var spannedParameters = message.Parameters.Span;

        // TODO: handle # being pushed out a char
        // TODO: maybe FF, caused by recent messages api
        var separatorIndex = spannedParameters.IndexOf(':');
        if (separatorIndex == -1)
            separatorIndex = spannedParameters.LastIndexOf(" ");
        var channel = message.Parameters[1..(separatorIndex - 1)];
        // TODO: add test
        var content = message.Parameters[(separatorIndex + 1)..].Trim('');

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

        do
        {
            var endOfTagIndex = remainingTags.Span.IndexOf(';');

            // BUGBUG: empty string at end could cause -1 index?
            if (endOfTagIndex == -1)
                endOfTagIndex = remainingTags.Length;

            var currentTag = remainingTags[..endOfTagIndex];
            var tagKey = currentTag[..currentTag.Span.IndexOf('=')];
            var tagValue = currentTag[(tagKey.Length + 1)..];

            // FIXME: Maybe don't use strings?
            parsedTags.Add(tagKey, tagValue);

            var realEndIndex = Math.Min(remainingTags.Length, endOfTagIndex + 1);
            remainingTags = remainingTags[realEndIndex..];
        } while (remainingTags.Length > 0);

        return parsedTags.ToImmutableDictionary();
    }
}
