using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSharping
{
    public class AsyncModel
    {
        private readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public void AddMessage(string format, params object[] args)
        {
            _messages.Enqueue(string.Format(format, args));
            // string.Format(" on threadId {0}", Thread.CurrentThread.ManagedThreadId)
        }

        public List<string> GetMessages()
        {
            return _messages.ToList();
        }
    }
}