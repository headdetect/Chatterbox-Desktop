using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Chatterbox.Gui
{
    public class MessageBlock
    {
        public MessageType MessageType { get; set; }
        internal List<MessageSegment> Segments { get; set; }
        public string PlainText { get { return ToString(); } }

        public MessageBlock(MessageType messageType = MessageType.Normal)
        {
            Segments = new List<MessageSegment>();
            MessageType = messageType;
        }

        public void AppendSegment(string text)
        {
            AppendSegment(text, Colors.Black);
        }

        public void AppendSegment(string text, Color color)
        {
            var msgSeg = new MessageSegment
            {
                Text = text,
                TextColor = color
            };

            Segments.Add(msgSeg);
        }

        public override string ToString()
        {
            if (Segments == null) return string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (MessageSegment t in Segments)
                builder.Append(t);
            return builder.ToString();
        }

        public static implicit operator MessageBlock(string s)
        {
            var block = new MessageBlock();
            block.AppendSegment(s);
            return block;
        }
    }

    internal class MessageSegment
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum MessageType
    {
        Normal,
        Error,
        System
    }
}
