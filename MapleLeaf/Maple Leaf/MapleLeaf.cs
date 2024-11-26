namespace MapleLeaf;

class MapleLeaf
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Error(4,"Error");
    }

    
    /// <summary>
    /// Prints an Error to the screen
    /// </summary>
    /// <param name="lineNumber"> The line number in MapleLeaf source</param>
    /// <param name="error"> The type of error</param>
    public static void Error(int lineNumber, string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error on line {lineNumber}:  {error}");
        Console.ResetColor();
    }

    /// <summary>
    /// Runs a MapleLeaf source file
    /// </summary>
    /// <param name="sourceFile">Destination of the file</param>
    public static void Run(string sourceFile)
    {
        Scanner scanner = new Scanner(sourceFile);
        List<Token> tokens = scanner.ScanTokens();
        
        foreach(Token token in tokens)
            Console.WriteLine(token);
    }
}