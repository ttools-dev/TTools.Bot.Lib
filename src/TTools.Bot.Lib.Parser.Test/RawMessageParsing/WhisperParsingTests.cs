using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class WhisperParsingTests
    {
        private const string TaggedWhisperMessage =
            "@badges=;color=#00FF7F;display-name=jammehcow;emotes=;message-id=2;thread-id=82674227_726077279;" +
            "turbo=0;user-id=12345;user-type= :jammehcow!jammehcow@jammehcow.tmi.twitch.tv " +
            "WHISPER someuser123 :test 1 2 3";
        private const string UntaggedWhisperMessage =
            ":jammehcow!jammehcow@jammehcow.tmi.twitch.tv WHISPER someuser123 :test 1 2 3";

        [Test]
        public void DoesExtractTagsWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedWhisperMessage.ToCharArray());
            parsedResult.Tags.ToString()
                .Should()
                .Be("badges=;color=#00FF7F;display-name=jammehcow;emotes=;message-id=2;thread-id=82674227_726077279;" +
                    "turbo=0;user-id=12345;user-type=",
                    "because we thought that the message had tags");
        }

        [Test]
        public void DoesExtractEmptyTagsWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UntaggedWhisperMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that the message didn't have tags");
        }

        [Test]
        public void DoesExtractPrefixWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedWhisperMessage.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("jammehcow!jammehcow@jammehcow.tmi.twitch.tv",
                    "because we thought that the message had a prefix");
        }

        [Test]
        public void DoesExtractCommandWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedWhisperMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("WHISPER", "because we thought that the message had a command");
        }

        [Test]
        public void DoesExtractParametersWhenPresent()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(TaggedWhisperMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("someuser123 :test 1 2 3", "because we thought that the message had a parameters");
        }
    }
}
