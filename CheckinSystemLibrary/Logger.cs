using System.IO;

namespace CheckInSystem;

public class Logger
{
    public static void LogError(Exception e)
    {
        string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath += @"\log.txt";
        File.AppendAllText(filePath, $"At {DateTime.Now} {e}\r\n");
    } 
}