namespace Paging;

public static class UserInput
{
    public static bool GetConfirmation(string prompt)
    {
        while (true)
        {
            Console.WriteLine($"{prompt} (Y/n)");
            var input = Console.ReadLine();
            
            switch (input?.Trim())
            {
                case null:
                    throw new InputException("Something went wrong whilst reading user input.");
                case "Y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.Error.WriteLine("Invalid input, try again.");
                    break;
            }
        }
    }

    public static int GetInt(string prompt) => GetNumber(prompt, int.Parse);
    
    public static uint GetAddress(string prompt) => GetNumber(prompt, ParseAddress);

    private static uint ParseAddress(string input) => input.Trim().StartsWith("0x")
            ? Convert.ToUInt32(input, 16)
            : Convert.ToUInt32(input);
    
    private static T GetNumber<T>(string prompt, Func<string, T> parseNumber)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();

            if (input == null)
                throw new InputException("Something went wrong whilst reading user input.");

            switch (TryParseNumberWithReason(input, out var result, parseNumber))
            {
                case (true, _):
                    // This is safe because we wouldn't return true in case of a null value.
                    return result!;
                case (false, var reason):
                    Console.Error.WriteLine($"{reason} Try again.");
                    break;
            }
        }
    }

    private static (bool success, string reason) TryParseNumberWithReason<T>(string input, out T? result, Func<string, T> parseNumber)
    {
        try
        {
            result = parseNumber(input.Trim());
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            result = default;
            return (false, ConvertToParseNumberReason(ex));
        }
    }

    private static string ConvertToParseNumberReason(Exception ex) =>
        ex switch
        {
            OverflowException => "Provided input is too large.",
            FormatException => "Provided input is of wrong type.",
            _ => "Provided input is invalid."
        };
}