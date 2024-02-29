static IEnumerable<int> Fib()
{
    int prev = 0, next = 1;
    yield return prev;
    yield return next;

    while (true)
    {
        int sum = prev + next;
        yield return sum;
        prev = next;
        next = sum;
    }
}

using IEnumerator<int> e = Fib().GetEnumerator();
while (e.MoveNext())
{
    int i = e.Current;
    if (i > 100) break;
    Console.Write($"{i} ");
}
Console.WriteLine();
