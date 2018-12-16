using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDLL
{
    class Logger
    {
        private static Logger instance;
        private Queue queue;

        public Logger()
        {
            queue = new Queue();
        }

        public static Logger getInstance()
        {
            if(instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        public void Write( MessageList mess_lst )
        {
           lock(queue.SyncRoot)
            {
                queue.Enqueue(mess_lst);
            }
        }

        public bool Is_Empty()
        {
            bool is_null = true;
            lock (queue.SyncRoot)
            {
                is_null = queue.Count > 0 ? false : true;
            }
            return is_null;
        }

        public MessageList Read()
        {
            MessageList obj = null;
            lock (queue.SyncRoot)
            {
                obj = (MessageList)queue.Dequeue();
            }
            return obj;
        }
    }
}
