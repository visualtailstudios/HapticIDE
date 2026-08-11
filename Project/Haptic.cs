using System;
using System.IO;

public static class HapticIDE{
    public static bool RUNNING {get; set;} = true;
    public static bool HAPTIC {get; set;}
    public static string FILE {get; set;}
    public static string[] LINES_OF_FILE {get; set;}
    public static string[] TMP_LINES {get; set;}
    public static string NEO_INTERPRETER = 
        Path.Combine(AppContext.BaseDirectory, "HapticScript.py");

    public static void Main(){
        Console.Clear();
        GetFileToWrite();
        foreach (string line in LINES_OF_FILE){
            Console.WriteLine(line);
        }
        while (RUNNING){
            ConsoleKeyInfo PressedKey = Console.ReadKey(true);
            int LastLineIndex = LINES_OF_FILE.Length - 1;
            
            if (PressedKey.Key == ConsoleKey.Escape){
                RUNNING = false;
            } else if (PressedKey.Key == ConsoleKey.Backspace){
                if (LINES_OF_FILE[LastLineIndex].Length > 0){
                    int CurrentLength = LINES_OF_FILE[LastLineIndex].Length;
                    LINES_OF_FILE[LastLineIndex] = LINES_OF_FILE[LastLineIndex].Remove(CurrentLength - 1, 1);
                }
            } else{
                LINES_OF_FILE[LastLineIndex] = LINES_OF_FILE[LastLineIndex] + PressedKey.KeyChar;
            }
            
            if (RUNNING){
                foreach (string line in LINES_OF_FILE){
                    Console.WriteLine(line);
                }
                int TargetY = Math.Max(0, Console.CursorTop - 1);
                int TargetX = LINES_OF_FILE[LastLineIndex].Length;
                if (TargetX >= Console.WindowWidth){
                    TargetX = Console.WindowWidth - 1;
                }
                Console.SetCursorPosition(TargetX, TargetY);
            }
        }
        Environment.Exit(0);
    }
    
    public static void GetFileToWrite(){
        Console.WriteLine("Please input a file path to start editing");
        FILE = (((Console.ReadLine().Replace("'", "")).Replace("\\", "")).Trim());
        if (File.Exists(FILE)){
            LINES_OF_FILE = File.ReadAllLines(FILE);
            TMP_LINES = (string[])LINES_OF_FILE.Clone();
        }
        HAPTIC = CheckFileForNeoIndex();
    }

    public static bool CheckFileForNeoIndex(){
        if (FILE.EndsWith(".n")){
            return true;
        }
        return false;
    }
}