using System.IO.Ports;
using System.Security.Cryptography;

const int DefaultBaudRate = 115200;
const int DefaultTestsCount = 10;

Console.WriteLine("Serial ports:");

foreach(var serialPort in SerialPort.GetPortNames())
{
    Console.WriteLine(serialPort);
}

Console.Write("Choose port: ");

var selectedPort = Console.ReadLine();

if (string.IsNullOrEmpty(selectedPort))
{
    Console.WriteLine("Serial port is empty!");
    return;
}

using var sp = new SerialPort(selectedPort);

int baudRate;

Console.Write($"Enter baudrate (default: {DefaultBaudRate}): ");

if(!int.TryParse(Console.ReadLine(), out baudRate))
{
    baudRate = DefaultBaudRate;
}

int testsCount;

Console.Write($"Enter number of tests (default: {DefaultTestsCount}):");

if(!int.TryParse(Console.ReadLine(), out testsCount))
{
    testsCount = DefaultTestsCount;
}

sp.BaudRate = baudRate;

sp.Open();

Console.WriteLine("Starting tests...");

using var rng = RandomNumberGenerator.Create();

var rnd = new Random();

sp.ReadTimeout = 10000;
sp.WriteTimeout = 10000;

for(int i = 1; i < testsCount; i++)
{
    var arrLen = rnd.Next(1 * i, 2 * i);

    byte[] arr = new byte[arrLen];
    byte[] res = new byte[arrLen];

    rng.GetNonZeroBytes(arr);

    Console.WriteLine($"Length: {arrLen}");

    Console.WriteLine("Sending...");

    sp.Write(arr, 0, arr.Length);

    Console.WriteLine("Waiting...");

    int rd = 0;
    while (rd < arrLen)
    {
        rd += sp.Read(res, rd, res.Length - rd);
    }

    Console.WriteLine("Gor response...");

    if(!Enumerable.SequenceEqual(arr, res))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error in response with length: {arrLen}, bytes read: {rd}");
        Console.ForegroundColor = ConsoleColor.White;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Response validated!");
        Console.ForegroundColor = ConsoleColor.White;
    }
}