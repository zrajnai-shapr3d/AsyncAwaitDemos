#region Main 

foreach (var num in Fib()) {
    if (num > 100) break;
    Console.Write(num + " ");
}
Console.WriteLine();

#endregion

static IEnumerable<int> Fib() {
    int prev = 0, next = 1;
    yield return prev;
    yield return next;

    while (true) {
        int sum = prev + next;
        yield return sum;
        prev = next;
        next = sum;
    }
}
