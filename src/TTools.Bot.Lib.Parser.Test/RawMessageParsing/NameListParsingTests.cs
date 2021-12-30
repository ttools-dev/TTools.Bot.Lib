using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    public class NameListParsingTests
    {
        private static Dictionary<string, string> _nameListMessages = new()
        {
            { "353", ":justinfan12345.tmi.twitch.tv 353 justinfan12345 = #jammehcow :xtrarapid charsty kotigo" +
                     " swayffe ranford whaleship hugsforporos" },
            { "366", ":justinfan12345.tmi.twitch.tv 366 justinfan12345 #jammehcow :End of /NAMES list" }
        };

        [Test, TestCaseSource(nameof(_nameListMessages))]
        public void DoesExtractEmptyTagsForNameListMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    $"because we thought that we did not provide tags for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(_nameListMessages))]
        public void DoesExtractPrefixForNameListMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("justinfan12345.tmi.twitch.tv",
                    $"because we thought that we provided a prefix for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(_nameListMessages))]
        public void DoesExtractCommandForNameListMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be(pair.Key,
                    $"because we thought that we provided a command for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(_nameListMessages))]
        public void DoesExtractParametersForNameListMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be(pair.Key,
                    $"because we thought that we provided command parameters for command {pair.Key}");
        }
    }
}
