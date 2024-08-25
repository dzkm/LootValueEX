using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LootValueEX.Structs
{
    internal struct TimestampedTask<T>
    {
        internal CancellationTokenSource CancellationTokenSource { get; }
        internal Task<T> Task { get; }
        internal long Timestamp { get; }

        internal TimestampedTask(CancellationTokenSource cancellationTokenSource, Task<T> task, long timestamp)
        {
            CancellationTokenSource = cancellationTokenSource;
            Task = task;
            Timestamp = timestamp;
        }
    }
}
