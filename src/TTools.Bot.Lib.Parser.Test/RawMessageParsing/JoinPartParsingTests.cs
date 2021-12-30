using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class JoinPartParsingTests
    {
        private const string JoinChannelMessage = ":jammehcow!jammehcow@jammehcow.tmi.twitch.tv JOIN #jammehcow";
        private const string PartChannelMessage = ":jammehcow!jammehcow@jammehcow.tmi.twitch.tv PART #jammehcow";

        #region JOIN

        [Test]
        public void DoesExtractEmptyTagsForJoinMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(JoinChannelMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractPrefixForJoinMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(JoinChannelMessage.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("jammehcow!jammehcow@jammehcow.tmi.twitch.tv", "because we thought that we provided a prefix");
        }

        [Test]
        public void DoesExtractCommandForJoinMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(JoinChannelMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("JOIN", "because we thought that we provided a command");
        }

        [Test]
        public void DoesExtractParametersForJoinMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(JoinChannelMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("#jammehcow", "because we thought that we provided parameters");
        }

        #endregion

        #region PART

        [Test]
        public void DoesExtractEmptyTagsForPartMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PartChannelMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractPrefixForPartMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PartChannelMessage.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("jammehcow!jammehcow@jammehcow.tmi.twitch.tv", "because we thought that we provided a prefix");
        }

        [Test]
        public void DoesExtractCommandForPartMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PartChannelMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("PART", "because we thought that we provided a command");
        }

        [Test]
        public void DoesExtractParametersForPartMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PartChannelMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("#jammehcow", "because we thought that we provided parameters");
        }

        #endregion
    }
}
