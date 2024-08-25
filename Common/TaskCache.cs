using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LootValueEX.Common
{
    internal class TaskCache<T>
    {
        internal ConcurrentDictionary<string, Structs.TimestampedTask<T>> taskDict = new ConcurrentDictionary<string, Structs.TimestampedTask<T>>();
        private long ExpireInSeconds;

        internal TaskCache(long expireInSeconds)
        {
            ExpireInSeconds = expireInSeconds;
        }

        public bool AddTask(string itemTemplateId, Task<T> task, CancellationTokenSource cancellationTokenSource)
        {
            return taskDict.TryAdd(itemTemplateId, 
                new Structs.TimestampedTask<T>(
                    cancellationTokenSource,
                    task,
                    DateTimeOffset.Now.ToUnixTimeSeconds()
                    )
                );
        }

        public Structs.TimestampedTask<T> GetItem(string key)
        {
            if (!taskDict.TryGetValue(key, out Structs.TimestampedTask<T> itemTask))
                return default;
            return itemTask;
        }

        public bool RemoveItem(string key)
        {
            return taskDict.TryRemove(key, out _);
        }

        public bool IsItemCacheExpired(string key)
        {
            Structs.TimestampedTask<T> item = this.GetItem(key);
            if (item.Equals(default(Structs.TimestampedTask<T>)))
            {
                return true;
            }
            if((DateTimeOffset.Now.ToUnixTimeSeconds() - item.Timestamp) > ExpireInSeconds)
            {
                return true;
            }
            return false;
        }

        public void EraseCache()
        {
            taskDict.Clear();
        }
    }
}
