using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class CapabilityRequestsParsingTests
    {
        private const string CapabilityListMessage = "CAP LS";
        private const string CapabilityRequestMessage = "CAP REQ :twitch.tv/tags twitch.tv/commands";

        [Test]
        public void DoesExtractEmptyTagsForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractEmptyPrefixForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide a prefix");
        }

        [Test]
        public void DoesExtractCommandForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("CAP", "because we thought that we did provide a command");
        }

        [Test]
        public void DoesExtractParametersForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("LS", "because we thought that we did provide parameters");
        }

        [Test]
        public void DoesExtractEmptyTagsForCapabilityRequestMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractEmptyPrefixForCapabilityRequestMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide a prefix");
        }

        [Test]
        public void DoesExtractCommandForCapabilityRequestMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("CAP", "because we thought that we did provide a command");
        }

        [Test]
        public void DoesExtractParametersForCapabilityRequestMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("REQ :twitch.tv/tags twitch.tv/commands", "because we thought that we did provide a command");
        }
    }
}
