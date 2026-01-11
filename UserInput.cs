namespace Paging;

public static class UserInput
{
    public static bool GetConfirmation(string prompt)
    {
        while (true)
        {
            Console.WriteLine($"{prompt} (Y/n)");
            var input = Console.ReadLine();
            switch (input)
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

    public static T GetInput<T>(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();

            if (input == null)
                throw new InputException("Something went wrong whilst reading user input.");
        
            switch (TryConvertInputToType<T>(input, out var result))
            {
                case (true, _):
                    return result!;
                case (false, var reason):
                    Console.Error.WriteLine($"{reason} Try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Tries to convert the input safely to the specified type. If this does not work, a printable reason for the failed attempt is given.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Either returns true and a non-null result or false with a garbage value.</returns>
    private static (bool success, string reason) TryConvertInputToType<T>(string input, out T? result)
    {
        result = default;
        
        try
        {
            result = (T)Convert.ChangeType(input, typeof(T));
            return (true, string.Empty);
        }
        catch (OverflowException)
        {
            return (false, "Provided input is too large.");
        }
        catch (FormatException)
        {
            return (false, "Provided input is of wrong type.");
        }
        catch (Exception)
        {
            return (false, "Provided input is invalid.");
        }
    }
}