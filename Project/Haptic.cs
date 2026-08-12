using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public static class HapticIDE{
    // Global bools
    public static bool RUNNING {get; set;} = true;
    public static bool HAPTIC {get; set;}

    public static string FILE {get; set;}

    // Comparison arrays for the visualization
    public static string[] LINES_OF_FILE {get; set;}
    public static string[] TMP_LINES {get; set;}

    // Custom interpreter for HapticScript files
    public static string NEO_INTERPRETER = 
        Path.Combine(AppContext.BaseDirectory, "HapticScript.py");

    // Initialize the cursor values
    public static int CursorY {get; set;} = 0;
    public static int CursorX {get; set;} = 0;

    public static void Main(){
        // Foolproof console clearing data
        Clear();
        // Let user input file to edit
        GetFileToWrite();

        // Initially writing the text to the screen
        foreach (string line in LINES_OF_FILE){
            Console.WriteLine(line);
        }

        while (RUNNING){
            ConsoleKeyInfo PressedKey = Console.ReadKey(true);

            // Writing the text to the screen
            if (TMP_LINES != LINES_OF_FILE){
                Clear();
                foreach (string line in LINES_OF_FILE){
                    Console.WriteLine(line);
                }
                TMP_LINES = (string[])LINES_OF_FILE.Clone();
            }

            // Key navigation
            if (PressedKey.Key == ConsoleKey.Escape){
                RUNNING = false;
            } else{
                // Adding the key pressed to lines of file
                string KeyAsString = PressedKey.Key.ToString();
                LINES_OF_FILE = LINES_OF_FILE.Append(KeyAsString).ToArray();

                // Setting the console cursor to be custom x and y
                Console.SetCursorPosition(CursorX, CursorY);
            }
        }

        // Writes the screen to the file
        File.WriteAllLines(FILE, LINES_OF_FILE);
        Environment.Exit(0);
    }

    public static void Clear(){
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Legacy clearing
            Console.Write("\x1b[3J");
        } else{
            Console.Clear();
        }
    }
    
    public static void GetFileToWrite(){
        Console.WriteLine("Please input a file path to start editing");
        // Cleans the input
        FILE = (((Console.ReadLine().Replace("'", "")).Replace("\\", "")).Trim());
        if (File.Exists(FILE)){
            // Adds lines to the proper arrays and comparison
            LINES_OF_FILE = File.ReadAllLines(FILE);
            TMP_LINES = (string[])LINES_OF_FILE.Clone();
        }
        HAPTIC = CheckFileForNeoIndex();
    }

    public static bool CheckFileForNeoIndex(){
        // Checks if the file added is HapticScript
        if (FILE.EndsWith(".n")){
            return true;
        }
        return false;
    }
}