using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;

namespace TeamCitySharp.AppServices
{
    public enum LogLevel
    {
        Default = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
    }

    public enum LogBlockType
    {
        Default = 0,
        TestSuite = 1,
        TestBlock = 2,
        TargetBlock = 3,
    }

    public class LogMessage
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public LogMessage Parent { get; set; }
        public SortedDictionary<int, LogMessage> Children { get; } = new();
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        LogLevel Level { get; set; }
        public int Indent { get; set; }
        public LogBlockType BlockType { get; set; }

        public LogMessage()
        {
        }

        public LogMessage(Message msg, LogContext context = null)
        {
            Id = msg.id;
            ParentId = msg.parentId;
            Level = (LogLevel)msg.status;
            Text = msg.text;
            Timestamp = msg.timestamp;
            Indent = msg.level;
            BlockType = msg.GetBlockType();

            if (context != null)
            {
                if (context.Logs.ContainsKey(ParentId))
                {
                    Parent = context.Logs[ParentId];
                    Parent.Children[Id] = this;
                }
            }
        }

        public string TestCaseName
        {
            get
            {
                if (BlockType.IsTestType())
                {
                    List<string> caseNames = new List<string> { Text };
                    var block = this;
                    while (block.Parent != null && block.Parent.BlockType.IsTestType())
                    {
                        caseNames.Add(block.Parent.Text);
                        block = block.Parent;
                    }

                    caseNames.Reverse();
                    var finalName = string.Join(".", caseNames);
                    return finalName.Trim();
                }

                return Text;
            }
        }

        public override string ToString()
        {
            return $"[{Timestamp:s}][{Level}] id:{Id} | {(BlockType.IsTestType()? TestCaseName : Text)}";
        }
    }

    public class LogContext
    {
        public LogContext(int buildId)
        {
            BuildId = buildId;
        }

        public LogContext(string buildId)
        {
            BuildId = int.Parse(buildId);
        }

        public int BuildId { get; set; }
        public SortedDictionary<int, LogMessage> Logs { get; private set; } = new();

        public void Update(MessagesResponse response)
        {
            foreach (var msg in response.messages)
            {
                if (Logs.ContainsKey(msg.id))
                {
                    continue;
                }

                var log = new LogMessage(msg, this);
                Logs.Add(log.Id, log);
            }
        }

        public int LatestId => Logs.Count > 0 ? Logs.Keys.LastOrDefault() : -1;

        public void Clear()
        {
            Logs.Clear();
        }
    }

    public static class LogExtension
    {
        public static IEnumerable<LogMessage> GetByIndent(this IEnumerable<LogMessage> logs, int indent) =>
            logs.Where(l => l.Indent == indent);

        public static IEnumerable<LogMessage> GetTopParents(this IEnumerable<LogMessage> logs) =>
            logs.Where(l => l.Parent == null);

        public static IEnumerable<LogMessage> GetLogsLaterThanId(this IEnumerable<LogMessage> logs, int id) =>
            logs.Where(l => l.Id >= id);

        public static LogBlockType GetBlockType(this Message msg)
        {
            if (msg == null)
                return LogBlockType.Default;
            return msg.blockType switch
            {
                "$TARGET_BLOCK$" => LogBlockType.TargetBlock,
                "$TEST_BLOCK$" => LogBlockType.TestBlock,
                "$TEST_SUITE$" => LogBlockType.TestSuite,
                _ => LogBlockType.Default
            };
        }

        public static bool IsTestType(this LogBlockType type)
        {
            return type == LogBlockType.TestBlock || type == LogBlockType.TestSuite;
        }
    }

    public class Messages
    {
        public static int MaxLogCount = 5000;
        private readonly ITeamCityCaller m_caller;

        public Messages(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public async Task<MessagesResponse> GetAllByIdAsync(int buildId)
        {
            var rsp =
                await m_caller.GetAsync<MessagesResponse>(
                    $"/app/messages?buildId={buildId}&messagesCount=0,{MaxLogCount}&target=tail&expandAll=true", false);
            return rsp;
        }

        public async Task<MessagesResponse> GetRangeMessagesAsync(int buildId, int messageId, int count = -1)
        {
            count = count > 0 ? count : MaxLogCount;
            var rsp =
                await m_caller.GetAsync<MessagesResponse>(
                    $"/app/messages?buildId={buildId}&messagesCount={count}&filter=verbose&messageId={messageId}&expandAll=true",
                    false);
            return rsp;
        }

        public async Task FetchAllLogs(LogContext context)
        {
            context.Update(await GetAllByIdAsync(context.BuildId));
        }

        public async Task UpdateLogs(LogContext context, int count = -1)
        {
            var lastId = context.LatestId;
            if (lastId < 0)
            {
                 await FetchAllLogs(context);
                 return;
            }

            count = count > 0 ? count : MaxLogCount;
            context.Update(await GetRangeMessagesAsync(context.BuildId, lastId, count));
            return;
        }
    }
}