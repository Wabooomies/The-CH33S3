using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace The_CH33S3.ViewModels
{
    internal class VersatileConnectionStringHelper
    {
        static public string DateFormat = "MM-dd-yyyy h:mm:ss tt";
        static public bool RandomizeNumber = false;
        static public int Number = 60;
        static public int MinRange = 60;
        static public int MaxRange = 120;

        public static int GetMinutes()
        {
            if (RandomizeNumber)
            {
                Random rnd = new Random();
                return rnd.Next(MinRange, MaxRange + 1);
            }
            return MinRange;
        }

        private static string _cachedConnectionString = string.Empty;

        public static async Task<(string ConnectionString, bool IsSuccess)> GetWorkingConnectionString()
        {
            // Return cached connection if already found
            if (!string.IsNullOrWhiteSpace(_cachedConnectionString))
                return (_cachedConnectionString, true);

            List<string> candidates = new List<string>();
            string csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "ListOfCommandStrings.csv");

            // 1. LOAD FROM CSV
            try
            {
                if (File.Exists(csvFilePath))
                {
                    string[] lines = await File.ReadAllLinesAsync(csvFilePath);

                    foreach (string line in lines)
                    {
                        string cleanLine = line?.Trim().Trim('"');

                        // Accept ANY non-empty line
                        if (!string.IsNullOrWhiteSpace(cleanLine))
                        {
                            // *** THE FIX: Run it through your normalizer! ***
                            string normalized = NormalizeConnectionString(cleanLine);

                            // Only add it if the normalizer didn't completely empty it out
                            if (!string.IsNullOrWhiteSpace(normalized))
                            {
                                candidates.Add(normalized);
                            }
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("CSV file not found.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CSV Read Error: {ex.Message}");
            }

            // 2. OPTIONAL: Add fallback only if CSV failed
            /*
            if (candidates.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No candidates found in CSV. Using fallback.");

                candidates.Add(@"Server=localhost; Database=ApiDatabase; Integrated Security=True; TrustServerCertificate=True;");
                candidates.Add(@"Server=CCL2-13\MSSQLSERVER01; Database=DatabaseForSQL; User Id=sa; Password=ccl2; TrustServerCertificate=True;");
            }
            */

            // 3. TEST CONNECTION STRINGS
            foreach (var connString in candidates)
            {
                try
                {
                    // Even though it's normalized, we still use the builder here 
                    // so we can safely inject the 3-second timeout!
                    var builder = new SqlConnectionStringBuilder(connString)
                    {
                        ConnectTimeout = 3 // Fast fail for dead servers
                    };

                    using (var connection = new SqlConnection(builder.ConnectionString))
                    {
                        await connection.OpenAsync();

                        // SUCCESS → cache and return
                        _cachedConnectionString = builder.ConnectionString;
                        return (_cachedConnectionString, true);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Connection failed: {connString} | {ex.Message}");
                    continue;
                }
            }

            // 4. TOTAL FAILURE
            System.Diagnostics.Debug.WriteLine("No working connection string found.");
            return (string.Empty, false);
        }

        private static string NormalizeConnectionString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string cleaned = input.Trim().Trim('"');

            try
            {
                // This will automatically parse the string and rewrite it perfectly!
                // It changes "Data Source" to "Data Source", "Server" to "Data Source", 
                // "Database" to "Initial Catalog", etc., ensuring absolute consistency.
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cleaned);
                return builder.ConnectionString;
            }
            catch
            {
                // If the string is absolute garbage and can't be parsed at all, 
                // just return the cleaned version and let the connection test fail later.
                return cleaned;
            }
        }
    }
}