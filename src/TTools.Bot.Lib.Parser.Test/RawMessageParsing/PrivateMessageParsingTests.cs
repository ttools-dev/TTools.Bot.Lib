using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Models;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class PrivateMessageParsingTests
    {
        private const string TaggedMessage =
            "@badge-info=subscriber/6;badges=broadcaster/1,subscriber/0,turbo/1;color=#89B827;display-name=jammehcow;" +
            "emotes=;first-msg=1;flags=;id=7e5c273d-803b-4406-b61e-a3be1280a9d2;mod=0;room-id=82674227;subscriber=1;" +
            "tmi-sent-ts=1630993395915;turbo=1;user-id=82674227;user-type= :" +
            "jammehcow!jammehcow@jammehcow.tmi.twitch.tv PRIVMSG #jammehcow :Example Message";

        private const string UntaggedMessage =
            ":jammehcow!jammehcow@jammehcow.tmi.twitch.tv PRIVMSG #jammehcow :Example Message";

        // TODO: check all tags, not just the example
        private static readonly Dictionary<string, char[]> Tags = new()
        {
            {"badge-info", "subscriber/6".ToCharArray()},
            {"badges", "broadcaster/1,subscriber/0,turbo/1".ToCharArray()},
            {"color", "#89B827".ToCharArray()},
            {"display-name", "jammehcow".ToCharArray()},
            {"emotes", "".ToCharArray()},
            {"first-msg", "1".ToCharArray()},
            {"flags", "".ToCharArray()},
            {"id", "7e5c273d-803b-4406-b61e-a3be1280a9d2".ToCharArray()},
            {"mod", "0".ToCharArray()},
            {"room-id", "82674227".ToCharArray()},
            {"subscriber", "1".ToCharArray()},
            {"tmi-sent-ts", "1630993395915".ToCharArray()},
            {"turbo", "1".ToCharArray()},
            {"user-id", "82674227".ToCharArray()},
            {"user-type", "".ToCharArray()}
        };

        [Test]
        public void DoesExtractTagsWhenPresent()
        {
            // TODO: fix
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedMessage.ToCharArray());
            parsedResult.Tags.ToString()
                .Should()
                .Be("badge-info=subscriber/6;badges=broadcaster/1,subscriber/0,turbo/1;color=#89B827;display-name=jammehcow;" +
                    "emotes=;first-msg=1;flags=;id=7e5c273d-803b-4406-b61e-a3be1280a9d2;mod=0;room-id=82674227;subscriber=1;" +
                    "tmi-sent-ts=1630993395915;turbo=1;user-id=82674227;user-type=",
                    "because we thought that the message had tags");
        }

        [Test]
        public void DoesExtractEmptyTagsWhenNotPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UntaggedMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that the message had no tags");
        }

        [Test]
        public void DoesExtractPrefixWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .NotBe(ReadOnlyMemory<char>.Empty,
                    "because we thought that the message had a prefix");
        }

        [Test]
        public void DoesExtractCommandWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UntaggedMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("PRIVMSG", "because we thought that the message had a command");
        }

        [Test]
        public void DoesIdentifyPrivateMessageType()
        {
            var parsedResult = TypedMessageParser.ParseMessageToTyped(TaggedMessage.ToCharArray());
            parsedResult
                .Should()
                .BeOfType<PrivateMessage>();
        }

        [Test]
        public void DoesParseTagsWhenPresent()
        {
            var parsedResult = (PrivateMessage)TypedMessageParser.ParseMessageToTyped(TaggedMessage.ToCharArray());
            parsedResult
                .ParsedTags
                .Should()
                .OnlyContain(pair =>
                    Tags.ContainsKey(pair.Key.ToString()) &&
                    pair.Value.ToArray().SequenceEqual(Tags[pair.Key.ToString()]));
        }
    }
}
