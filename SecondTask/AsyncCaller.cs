using System;
using System.Threading;

namespace SecondTask
{
    public class AsyncCaller
    {
        private readonly EventHandler _eventHandler;

        private CancellationTokenSource _cts;

        public AsyncCaller(EventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public bool Invoke(int delay, object sender, EventArgs eventArgs)
        {
            _cts = new CancellationTokenSource(delay);
            var cancellationToken = _cts.Token;

            var asyncRes = _eventHandler.BeginInvoke(sender, eventArgs, BeginInvokeCallback, null);

            while (asyncRes.IsCompleted == false)
            {
                asyncRes.AsyncWaitHandle.WaitOne(1);

                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Task canceled!");
                    return false;
                }
            }

            _eventHandler.EndInvoke(asyncRes);
            Console.WriteLine("Task wait success");
            return true;
        }

        private void BeginInvokeCallback(IAsyncResult ar)
        {
            Console.WriteLine("BeginInvokeCallback invoked");
        }
    }
}