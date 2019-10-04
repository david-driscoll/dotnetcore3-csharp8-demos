using System;
using System.Collections.Generic;

class Others
{
    // Null-coalescing assignment
    public static void NullCoalescingAssignment()
    {
        List<int> numbers = null;
        int? i = null;

        numbers ??= new List<int>();
        numbers.Add(i ??= 17);
        numbers.Add(i ??= 20);

        Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
        Console.WriteLine(i);  // output: 17
    }
}
