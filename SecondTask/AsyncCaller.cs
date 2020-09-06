using System;
using System.Threading;

/*Задача #2
В .net есть возможность звать делегаты как синхронно:
EventHandler h = new EventHandler(this.myEventHandler);
h.Invoke(null, EventArgs.Empty);
так и асинхронно:
var res = h.BeginInvoke(null, EventArgs.Empty, null, null);

Нужно реализовать возможность полусинхронного вызова делегата (написать реализацию класса AsyncCaller), который бы работал таким образом:

EventHandler h = new EventHandler(this.myEventHandler);
ac = new AsyncCaller(h);
bool completedOK = ac.Invoke(5000, null, EventArgs.Empty);

"Полусинхронного" в данном случае означает, что делегат будет вызван, и вызывающий поток будет ждать, пока вызов не выполнится. Но если выполнение делегата займет больше 5000 миллисекунд, то ac.Invoke выйдет и вернет в completedOK значение false.
*/

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