using System.Runtime.InteropServices;

namespace Enterprise.TextEncoding.ConsoleEncoding;
// https://github.com/julielerman/EFCoreEncodingDemo/tree/main
// CP stands for "code page"

public static class ConsoleEncodingService
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleOutputCP(uint wCodePageId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCP(uint wCodePageId);

    public static void AllowSpecialCharacters()
    {
        SetConsoleOutputCP(1256);
        SetConsoleCP(1256);
    }

    public static void RevertEncoding()
    {
        Console.OutputEncoding = System.Text.Encoding.Default;
    }
}