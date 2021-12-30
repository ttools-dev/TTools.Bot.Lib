using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class PingPongParsingTests
    {
        private const string UnprefixedPingMessage = "PING :tmi.twitch.tv";
        private const string UnprefixedPongMessage = "PONG";
        private const string PrefixedPongMessage = ":tmi.twitch.tv PONG";
        private const string UnprefixedCustomPongMessage = "PONG :Test Message";
        private const string PrefixedCustomPongMessage = ":tmi.twitch.tv PONG :Test Message";

        [Test]
        public void DoesExtractEmptyPrefixWhenUnprefixedPingMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UnprefixedPingMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we didn't provide a prefix");
        }

        [Test]
        public void DoesExtractCommandWhenUnprefixedPingMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UnprefixedPingMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("PING",
                    "because we thought that we provided a PING message");
        }

        [Test]
        public void DoesExtractCommandWhenUnprefixedPongMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UnprefixedPongMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("PONG",
                    "because we thought that we provided a PONG message");
        }

        [Test]
        public void DoesExtractCommandWhenPrefixedPongMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PrefixedPongMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("PONG",
                    "because we thought that we provided a PONG message");
        }

        [Test]
        public void DoesExtractPrefixWhenPrefixedPongMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PrefixedPongMessage.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("tmi.twitch.tv",
                    "because we thought that we provided a prefix");
        }

        [Test]
        public void DoesExtractParametersWhenPrefixedPongMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(PrefixedCustomPongMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be(":Test Message",
                    "because we thought that we provided a message in the parameters");
        }

        [Test]
        public void DoesExtractParametersWhenUnprefixedPongMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(UnprefixedCustomPongMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be(":Test Message",
                    "because we thought that we provided a message in the parameters");
        }
    }
}
