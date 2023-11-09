var echoService = new BottleNeckEchoService();

List<Task> tasks = new List<Task>();
for (var i = 0; i < 10; i++)
{
    tasks.Add(echoService.EchoAsync($"Hi, T{i}"));
}

Task.WaitAll(tasks.ToArray());

