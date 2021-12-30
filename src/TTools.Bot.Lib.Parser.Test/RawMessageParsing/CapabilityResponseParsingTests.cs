using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class CapabilityResponseParsingTests
    {
        private const string CapabilityListResponseMessage = "CAP * LS :twitch.tv/commands twitch.tv/membership twitch.tv/tags";
        private const string CapabilityRequestAcknowledgeMessage = "CAP * ACK :twitch.tv/tags twitch.tv/commands";
        private const string CapabilityRequestNotAcknowledgeMessage = "CAP * NAK :twitch.tv/tags twitch.tv/commands";

        #region CAP * LS

        [Test]
        public void DoesExtractEmptyTagsForCapabilityListResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListResponseMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractEmptyPrefixForCapabilityListResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListResponseMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide a prefix");
        }

        [Test]
        public void DoesExtractCommandForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListResponseMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("CAP", "because we thought that we did provide a command");
        }

        [Test]
        public void DoesExtractParametersForCapabilityListMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityListResponseMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("* LS :twitch.tv/commands twitch.tv/membership twitch.tv/tags",
                    "because we thought that we did provide parameters");
        }

        #endregion

        #region CAP * ACK

        [Test]
        public void DoesExtractEmptyTagsForCapabilityAcknowledgeResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestAcknowledgeMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractEmptyPrefixForCapabilityAcknowledgeResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestAcknowledgeMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide a prefix");
        }

        [Test]
        public void DoesExtractCommandForCapabilityAcknowledgeMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestAcknowledgeMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("CAP", "because we thought that we did provide a command");
        }

        [Test]
        public void DoesExtractParametersForCapabilityAcknowledgeMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestAcknowledgeMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("* ACK :twitch.tv/tags twitch.tv/commands",
                    "because we thought that we did provide parameters");
        }

        #endregion

        #region CAP * NAK

        [Test]
        public void DoesExtractEmptyTagsForCapabilityNotAcknowledgeResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestNotAcknowledgeMessage.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide tags");
        }

        [Test]
        public void DoesExtractEmptyPrefixForCapabilityNotAcknowledgeResponseMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestNotAcknowledgeMessage.ToCharArray());
            parsedResult.Prefix
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    "because we thought that we did not provide a prefix");
        }

        [Test]
        public void DoesExtractCommandForCapabilityNotAcknowledgeMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestNotAcknowledgeMessage.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be("CAP", "because we thought that we did provide a command");
        }

        [Test]
        public void DoesExtractParametersForCapabilityNotAcknowledgeMessage()
        {
            var parsedResult = RawMessageParser.ParseRawMessage(CapabilityRequestNotAcknowledgeMessage.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be("* NAK :twitch.tv/tags twitch.tv/commands",
                    "because we thought that we did provide parameters");
        }

        #endregion
    }
}
