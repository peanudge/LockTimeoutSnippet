public class BottleNeckEchoService
{
    private int _counter = 0;
    private object _lock = new object();
    private int _lockTimeout = 3000; // 1000 milliseconds
    private TimeSpan _operationTimeout = TimeSpan.FromMilliseconds(1000);
    private TimeSpan _operationTime = TimeSpan.FromMilliseconds(10);
    public async Task EchoAsync(string message)
    {
        await Task.Delay(1); // For preventing "This async method lacks 'await' operators and will run synchronously"

        Console.WriteLine($"Send {message}");
        bool acquiredLock = false;
        Interlocked.Increment(ref _counter);
        try
        {
            Monitor.TryEnter(_lock, _lockTimeout, ref acquiredLock);
            if (!acquiredLock)
            {
                Console.WriteLine($"{DateTime.Now} {message} (timeout)");
                return;
            }

            CancellationTokenSource source = new CancellationTokenSource(_operationTimeout);

            // TODO: here

            // INFO: Simulate a long running operation
            var cancellationTriggered = source.Token.WaitHandle.WaitOne((int)_operationTime.TotalMilliseconds);
            if (cancellationTriggered)
            {
                Console.WriteLine($"{DateTime.Now} {message} (timeout) in action. waiting: {_counter - 1}");
                return;
            }

            Console.WriteLine($"Recevied '{message}'. waiting: {_counter - 1})");
        }
        finally
        {
            Interlocked.Decrement(ref _counter);
            if (acquiredLock)
            {
                Monitor.Exit(_lock);
            }
        }
    }
}
