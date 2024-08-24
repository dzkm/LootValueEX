using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Common
{
    internal class TaskCache
    {
        internal ConcurrentDictionary<string, Structs.TimestampedTask> taskDict = new ConcurrentDictionary<string, Structs.TimestampedTask>();

        public bool AddTask(string itemTemplateId, Task task, long unixTimestamp)
        {
            return false;
        }

        public Structs.TimestampedTask GetItem(string key)
        {
            Structs.TimestampedTask itemTask;
            taskDict.TryGetValue(key, out itemTask);
            return itemTask;

        }

        public bool RemoveItem(string key)
        {
            return taskDict.TryRemove(key, out _);
        }

        public void EraseCache()
        {
            taskDict.Clear();
        }
    }
}
