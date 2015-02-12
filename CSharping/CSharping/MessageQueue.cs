using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSharping
{
    public class MessageQueue
    {
        private readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public void AddMessage(string format, params object[] args)
        {
            AddMessage(string.Format(format, args));
            // string.Format(" on threadId {0}", Thread.CurrentThread.ManagedThreadId)
        }

        public void AddMessage(string message)
        {
            _messages.Enqueue(message);
        }

        public List<string> GetAll()
        {
            return _messages.ToList();
        }
    }
}