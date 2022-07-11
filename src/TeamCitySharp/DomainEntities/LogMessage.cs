using System;
using System.Collections.Generic;
using System.Text;
// ReSharper disable InconsistentNaming

namespace TeamCitySharp.DomainEntities
{
    /// <summary>
    /// app service log message
    /// </summary>
    public class MessagesResponse
    {
        public Message[] messages { get; set; }
        public object[] expandedMessagesIndices { get; set; }
        public int lastMessageIndex { get; set; }
        public bool lastMessageIncluded { get; set; }
        public int prevVisible { get; set; }
    }

    public class Message
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public bool containsMessages { get; set; }
        public string text { get; set; }
        /// <summary>
        /// 1 = normal, 2 =warn?, 3 = error
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Indent
        /// </summary>
        public int level { get; set; }
        public DateTime timestamp { get; set; }
        /// <summary>
        /// "errormessage"
        /// </summary>
        public string renderingHint { get; set; }
        public long flowId { get; set; }
        public DateTime serverTimestamp { get; set; }
        public string blockType { get; set; }
        public int duration { get; set; }
        public bool verbose { get; set; }
    }

}
