using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Utils
{
    public static string Stringify<T>(this IEnumerable<T> collection, string separator = ", ", 
        string leftBracket = "[", string rightBracket = "]") =>
        $"{leftBracket}{string.Join(separator, collection)}{rightBracket}";

    // public static void Print<T>(IEnumerable<T> collection) =>
    //     Console.WriteLine(collection.Stringify());
    
    public static void Print<T>(this IEnumerable<T> collection, string separator = ", ",
        string leftBracket = "[", string rightBracket = "]") =>
        Console.WriteLine(collection.Stringify(separator,leftBracket, rightBracket));
    
    public static void Println<T>(this IEnumerable<T> collection) =>
        collection.Print("\n", "", "");
    
    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        var (step, count) = (1, range.End.Value - range.Start.Value);
        if (range.Start.IsFromEnd)
            (step , count) = (-step, -count);
        if (count < 0)
            count = 0;
        return Enumerable.Range(0, count)
            .Select(item => item * step + range.Start.Value).GetEnumerator();
    }
    
    public static IEnumerable<int> AsEnumerable(this Range range)
    {
        foreach (var item in range)
        {
            yield return item;
        }
    }

    public static int[] ToArray(this Range range) => range.AsEnumerable().ToArray();
    
    public static List<int> ToList(this Range range) => range.AsEnumerable().ToList();
}

public record StepRange(int Start, int End, int Step): IEnumerable<int>
{
    public static implicit operator StepRange(Range r)
    {
        return new StepRange(r.Start.Value, r.End.Value, 1);
    }

    public static StepRange operator ^(StepRange stepRange, int step)
    {
        return new StepRange(stepRange.Start, stepRange.End, step);
    }
        
    public IEnumerator<int> GetEnumerator()
    {
        var count = (End - Start) / Step;
        if (count < 0)
            count = 0;
        return Enumerable.Range(0, count)
            .Select(item => item * Step + Start).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}