using FluentAssertions;
using NUnit.Framework;
using TTools.Bot.Lib.Parser.Parsers;

namespace TTools.Bot.Lib.Parser.Test.RawMessageParsing
{
    [TestFixture]
    public class MotdParsingTests
    {
        private static Dictionary<string, string> MotdMessages = new()
        {
            { "001", ":tmi.twitch.tv 001 justinfan12345 :Welcome, GLHF!" },
            { "002", ":tmi.twitch.tv 002 justinfan12345 :Your host is tmi.twitch.tv" },
            { "003", ":tmi.twitch.tv 003 justinfan12345 :This server is rather new" },
            { "004", ":tmi.twitch.tv 004 justinfan12345 :-" },
            { "372", ":tmi.twitch.tv 372 justinfan12345 :You are in a maze of twisty passages, all alike." },
            { "375", ":tmi.twitch.tv 375 justinfan12345 :-" },
            { "376", ":tmi.twitch.tv 376 justinfan12345 :>" }
        };

        [Test, TestCaseSource(nameof(MotdMessages))]
        public void DoesExtractEmptyTagsForMotdMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Tags
                .Should()
                .Be(ReadOnlyMemory<char>.Empty,
                    $"because we thought that we did not provide a prefix for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(MotdMessages))]
        public void DoesExtractPrefixForMotdMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Prefix.ToString()
                .Should()
                .Be("tmi.twitch.tv",
                    $"because we thought that we provided a prefix for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(MotdMessages))]
        public void DoesExtractCommandForMotdMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Command.ToString()
                .Should()
                .Be(pair.Key,
                    $"because we thought that we provided a command for command {pair.Key}");
        }

        [Test, TestCaseSource(nameof(MotdMessages))]
        public void DoesExtractParametersForMotdMessages(KeyValuePair<string, string> pair)
        {
            var parsedResult = RawMessageParser.ParseRawMessage(pair.Value.ToCharArray());
            parsedResult.Parameters.ToString()
                .Should()
                .Be(pair.Value.Split($"{pair.Key} ").Last(),
                    $"because we thought that we provided a parameters for command {pair.Key}");
        }
    }
}
