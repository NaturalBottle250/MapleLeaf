namespace MapleLeaf;

class MapleLeaf
{
    private static bool hasError = false;
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        //Error(4,"Error");
        
        //OpenFile("test.mlf");
        
        
        Expression expression = new BinaryExpression(
            new UnaryExpression(
                new Token(TokenType.MINUS, "-", null, 1),
                new LiteralExpression(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new GroupingExpression(
                new LiteralExpression(45.67)));
        Console.WriteLine(new ASTPrinter().Print(expression));
        

    }

    
    /// <summary>
    /// Prints an Error to the screen
    /// </summary>
    /// <param name="lineNumber"> The line number in MapleLeaf source</param>
    /// <param name="error"> The type of error</param>
    public static void Error(int lineNumber, string error)
    {
        hasError = true;
        Report("", error, lineNumber);
        /*
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error on line {lineNumber}:  {error}");
        Console.ResetColor();
        */
    }

    private static void Report(string errorLocation, string message, int lineNumber = -1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (lineNumber == -1)
        {
            Console.WriteLine($"MapleLeafError : {errorLocation}: {message}");

        }
        else
        {
            Console.WriteLine($"Error on line {lineNumber}: {errorLocation}: {message}");
        }
        Console.ResetColor();
    }

    /// <summary>
    /// Runs a MapleLeaf source file
    /// </summary>
    /// <param name="sourceString">Destination of the file</param>
    public static void Run(string sourceString)
    {
        Scanner scanner = new Scanner(sourceString);
        List<Token> tokens = scanner.ScanTokens();
        
        foreach(Token token in tokens)
            //Console.WriteLine(token);
            token.PrintColored();
    }
    
    /// <summary>
    /// Attempts to Open a MapleLeaf source file
    /// </summary>
    /// <param name="path">Path to the source file</param>
    private static void OpenFile(string path)
    {
        byte[] bytes = new byte[1];
        try
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string srcPath = Path.Combine(basePath, "../../../", "src", path);

            srcPath = Path.GetFullPath(srcPath);

            bytes = File.ReadAllBytes(srcPath);
            //foreach (var VARIABLE in bytes) Console.WriteLine(VARIABLE);
            
        }
        catch (Exception e)
        {
            Report("OpenFile", $"Unable to find or access {path}");
            Console.WriteLine(e);
        }
        string content = System.Text.Encoding.UTF8.GetString(bytes).Trim();

        if (content.Length > 0 && content[0] == '\uFEFF')
            content = content.Substring(1);

        Run(content);

    }
    
}