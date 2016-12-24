using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace StackExchange.Profiling.Tests
{
    public class LockPerformanceTest
    {
        private readonly ITestOutputHelper _output;

        public LockPerformanceTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void LockPerformance()
        {

            var time = new Stopwatch();

            var list1 = new List<int>();
            time.Start();
            for (int i = 0; i < 1000000; i++)
            {
                list1.Add(i);
            }
            time.Stop();
            _output.WriteLine("output");
            _output.WriteLine($"Without locks took: {time.Elapsed.TotalMilliseconds}ms");

            list1 = new List<int>();
            time.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                lock(list1)
                    list1.Add(i);
            }
            time.Stop();
            
            _output.WriteLine("output");
            _output.WriteLine($"With lock took: {time.Elapsed.TotalMilliseconds}ms");

            list1 = new List<int>();
            var spinLock = new SpinLock(enableThreadOwnerTracking: false);
            time.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                var locked = false;
                spinLock.Enter(ref locked);
                if (!locked)
                    throw new Exception("didn't lock");
                try
                {
                    list1.Add(i);
                }
                finally
                {
                    if (locked)
                        spinLock.Exit();
                }
            }
            time.Stop();

            _output.WriteLine("output");
            _output.WriteLine($"With spinlock took: {time.Elapsed.TotalMilliseconds}ms");

            var list2 = new ConcurrentQueue<int>();
            time.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                list2.Enqueue(i);
            }
            time.Stop();
            _output.WriteLine("output");
            _output.WriteLine($"With concurrent took: {time.Elapsed.TotalMilliseconds}ms");
        }
    }
}
