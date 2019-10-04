using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AsyncIterators
{
    internal async static IAsyncEnumerable<string> CodeCampRocks()
    {
        yield return "Code";
        await Task.Delay(1000);
        yield return "Camp";
        await Task.Delay(1000);
        yield return "Rocks!";
    }

    public static async Task Demo()
    {
        await foreach (var item in CodeCampRocks())
        {
            Console.WriteLine(item);
        }
    }
}
