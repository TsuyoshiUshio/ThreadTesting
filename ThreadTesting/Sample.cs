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

            Task.Run(() => HelloLoopAsync(cts.Token));
            Task.Run(() => HelloLoopAsync(cts.Token));
            Console.ReadLine();
        }

        private async Task HelloLoopAsync(CancellationToken cancellationToken)
        {
            int count = 0;
            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 3);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: count : {count}");
                count++;
            }
            Console.WriteLine($"Cancelled {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
