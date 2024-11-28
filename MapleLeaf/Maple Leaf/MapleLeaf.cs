using MapleLeaf.Statements;
using System;
namespace MapleLeaf;

class MapleLeaf
{
    private static readonly Interpreter interpreter = new Interpreter();
    private static bool hasError, hasRuntimeError;
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        
        //Console.WriteLine("Please enter a number: " + 1);
        //Error(4,"Error");
        
        OpenFile("test.mlf");
        
        
        Expression expression = new BinaryExpression(
            new UnaryExpression(
                new Token(TokenType.MINUS, "-", null, 1),
                new LiteralExpression(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new GroupingExpression(
                new LiteralExpression(45.67)));
        //Console.WriteLine(new ASTPrinter().Print(expression));
        

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
    /// Reports Errors to the Console
    /// </summary>
    /// <param name="token"> Failed Token</param>
    /// <param name="message">Error Message</param>
    public static void Error(Token token, string message)
    {
        hasError = true;
        if(token.tokenType == TokenType.EOF)
            Report(" at end", message, token.lineNumber);
        else Report(" at '" + token.lexeme + "'", message, token.lineNumber);
    }

    /// <summary>
    /// Runs a MapleLeaf source file
    /// </summary>
    /// <param name="sourceString">Destination of the file</param>
    public static void Run(string sourceString)
    {
        Scanner scanner = new Scanner(sourceString);
        
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new Parser(tokens);
        List<Statement> statements = parser.Parse();
        if(hasError) return;
        
        //foreach(Token token in tokens) token.PrintColored();

        
        interpreter.Interpret(statements);
        
        //Console.WriteLine(new ASTPrinter().Print(expression));
        
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
        
        //if(hasError) Environment.Exit(65);
        //if(hasRuntimeError) Environment.Exit(70);

        if (content.Length > 0 && content[0] == '\uFEFF')
            content = content.Substring(1);

        Run(content);

    }


    public static void RuntimeError(Interpreter.RuntimeError error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"{error.Message}\n[line {error.Token.lineNumber}]");
        hasRuntimeError = true;
        
    }
    
}