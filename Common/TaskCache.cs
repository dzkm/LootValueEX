using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LootValueEX.Common
{
    internal class TaskCache<T>
    {
        private ConcurrentDictionary<string, Structs.TimestampedTask<T>> taskDict = new ConcurrentDictionary<string, Structs.TimestampedTask<T>>();
        private long ExpireInSeconds;

        internal TaskCache(long expireInSeconds)
        {
            ExpireInSeconds = expireInSeconds;
        }

        public bool AddTask(string key, Task<T> task, CancellationTokenSource cancellationTokenSource)
        {
            return this.AddTask(key, 
                new Structs.TimestampedTask<T>(
                    cancellationTokenSource,
                    task,
                    DateTimeOffset.Now.ToUnixTimeSeconds()
                    )
                );
        }

        public bool AddTask(string key, Structs.TimestampedTask<T> timestampedTask)
        {
            if(taskDict.ContainsKey(key))
                return taskDict.TryUpdate(key, timestampedTask, GetTask(key));

            return taskDict.TryAdd(key, timestampedTask);
        }

        public Structs.TimestampedTask<T> GetTask(string key)
        {
            if (!taskDict.TryGetValue(key, out Structs.TimestampedTask<T> itemTask))
                return default;
            return itemTask;
        }

        public bool RemoveTask(string key)
        {
            return taskDict.TryRemove(key, out _);
        }

        public bool IsTaskCacheExpired(string key)
        {
            Structs.TimestampedTask<T> item = this.GetTask(key);
            if (item.Equals(default(Structs.TimestampedTask<T>)))
                return true;

            if (ExpireInSeconds < 0)
                return false;

            if ((uint)(DateTimeOffset.Now.ToUnixTimeSeconds() - item.Timestamp) > ExpireInSeconds)
                return true;

            return false;
        }

        public void CancelTask(string key)
        {
            if (this.taskDict.TryGetValue(key, out Structs.TimestampedTask<T> item))
            {
                if (!item.Task.IsCompleted)
                {
                    item.CancellationTokenSource.Cancel();
                    item.CancellationTokenSource.Dispose();
                    Mod.TraderOfferTaskCache.taskDict.TryRemove(key, out _);
                }
            }
        }

        public void EraseCache()
        {
            taskDict.Clear();
        }
    }
}
