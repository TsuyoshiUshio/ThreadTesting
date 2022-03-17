using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTesting
{
    public class Sample
    {
        public void Execute()
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (source, args) =>
            {
                Console.Error.WriteLine("Cancelling execution ...");
                args.Cancel = true;
                cts.Cancel();
            };

            Console.WriteLine($"MainThread {Thread.CurrentThread.ManagedThreadId}");

            SomeSetupAsync().GetAwaiter().GetResult(); // create a new thread so that it is different behavior, however, original purpose is getting error before loop.

            Task.Run(() => HelloLoopAsync(cts.Token));
            Task.Run(() => HelloLoopAsync(cts.Token));
            Console.ReadLine();
        }

        private async Task SomeSetupAsync()
        {
            await Task.Delay(10);
            Console.WriteLine($"Runs setup on {Thread.CurrentThread.ManagedThreadId}");
        }

        private async Task HelloLoopAsync(CancellationToken cancellationToken)
        {
            int count = 0;
            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 3);
                await SomeSetupAsync();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: count : {count}");
                count++;
            }
            Console.WriteLine($"Cancelled {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
