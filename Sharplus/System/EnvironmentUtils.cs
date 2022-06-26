using System;
using System.IO;

namespace Sharplus.System
{
    public static class EnvironmentUtils
    {
        public static void LoadVariables(string filePath)
        {
            using Stream file = File.OpenRead(filePath);
            LoadVariables(file);
        }

        public static void LoadVariables(Stream stream)
        {
            using TextReader reader = new StreamReader(stream);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!line.StartsWith('#'))
                {
                    string[] parts = line.Split('=');
                    string variable = parts[0].Trim();
                    string value = parts[1].Trim();

                    Environment.SetEnvironmentVariable(variable, value);
                }
            }
        }
    }
}