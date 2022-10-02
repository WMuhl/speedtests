using System.Diagnostics;

namespace Speedtests.Tests.ThrowCatch;

public static class ThrowCatch
{
    public static void StartTest()
    {
        DisplayInfo();
        var data = GenerateData(1000000);
        Console.WriteLine("Executing first throw-catch test (10000 records)");
        var smallThrowCatchResult = ExecuteThrowCatchTest(data.Take(10000).ToList());
        Console.WriteLine("Executing second throw-catch test (100000 records)");
        var midThrowCatchResult = ExecuteThrowCatchTest(data.Take(100000).ToList());
        Console.WriteLine("Executing last throw-catch test (1000000 records)");
        var largeThrowCatchResult = ExecuteThrowCatchTest(data.Take(1000000).ToList());
        
        Console.WriteLine("Executing first normal test (10000 records)");
        var smallNormalResult = ExecuteNormalTest(data.Take(10000).ToList());
        Console.WriteLine("Executing second normal test (100000 records)");
        var midNormalTestResult = ExecuteNormalTest(data.Take(100000).ToList());
        Console.WriteLine("Executing last normal test (1000000 records)");
        var largeNormalResult = ExecuteNormalTest(data.Take(1000000).ToList());

        Console.WriteLine("Results:");
        Console.WriteLine($" Short throw-catch test: {smallThrowCatchResult.Minutes:00}:{smallThrowCatchResult.Seconds:00}.{smallThrowCatchResult.Milliseconds/10:00}");
        Console.WriteLine($"      Short normal test: {smallNormalResult.Minutes:00}:{smallNormalResult.Seconds:00}.{smallNormalResult.Milliseconds/10:00}");
        Console.WriteLine($"Medium throw-catch test: {midThrowCatchResult.Minutes:00}:{midThrowCatchResult.Seconds:00}.{midThrowCatchResult.Milliseconds/10:00}");
        Console.WriteLine($"     Medium normal test: {midNormalTestResult.Minutes:00}:{midNormalTestResult.Seconds:00}.{midNormalTestResult.Milliseconds/10:00}");
        Console.WriteLine($"  Long throw-catch test: {largeThrowCatchResult.Minutes:00}:{largeThrowCatchResult.Seconds:00}.{largeThrowCatchResult.Milliseconds/10:00}");
        Console.WriteLine($"       Long normal test: {largeNormalResult.Minutes:00}:{largeNormalResult.Seconds:00}.{largeNormalResult.Milliseconds/10:00}");
    }

    private static void DisplayInfo()
    {
        Console.WriteLine("This test will iterate over 10000, 100000, and 1000000 test cases, twice.");
        Console.WriteLine(
            "Each test case takes a model with First, Middle, and Last names with every 5th middle name left as blank.");
        Console.WriteLine(
            "The first set of tests will use a throw-catch when middle name is blank and add that record to an error list.");
        Console.WriteLine("The second set will do the same but without using a throw-catch.");
        Console.WriteLine("Use 2 sets of tests with 3 results each we can try to predict how timing would scale up.");
        Console.WriteLine("These are tests are not threaded.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static TimeSpan ExecuteThrowCatchTest(List<UserDetails> data)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        var errorDetails = new List<UserDetails>();

        foreach (var userDetail in data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userDetail.MiddleName))
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception e)
            {
                errorDetails.Add(userDetail);
            }
        }

        stopWatch.Stop();
        return stopWatch.Elapsed;
    }

    private static TimeSpan ExecuteNormalTest(List<UserDetails> data)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        var errorDetails = new List<UserDetails>();

        foreach (var userDetail in data)
        {
            if (string.IsNullOrWhiteSpace(userDetail.MiddleName))
            {
                errorDetails.Add(userDetail);
            }
        }

        stopWatch.Stop();
        return stopWatch.Elapsed;
    }

    private static List<UserDetails> GenerateData(int amount)
    {
        var response = new List<UserDetails>();
        for (var i = 0; i < amount; i++)
        {
            response.Add(i % 5 == 0
                ? new UserDetails($"Name{i}", string.Empty, $"Surname{i}")
                : new UserDetails($"Name{i}", $"MiddleName{i}", $"Surname{i}"));
        }

        return response;
    }
}

public readonly record struct UserDetails(string FirstName, string MiddleName, string LastName);