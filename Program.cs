using Speedtests.Tests.ThrowCatch;

if (!args.Any())
{
    DisplayMenu();
    return;
}

switch (args[0])
{
    case "throwcatch":
        ThrowCatch.StartTest();
        break;
    default:
        DisplayMenu();
        break;
}

void DisplayMenu()
{
    Console.WriteLine("Please pass one of the below tests.");
    Console.WriteLine("throwcatch - A simple test denoting the speed difference when using throw-catch vs using normal logic.");
}
