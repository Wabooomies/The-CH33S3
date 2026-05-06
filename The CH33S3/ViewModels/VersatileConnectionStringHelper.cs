using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Added to help with parallel tasks
using System.Threading.Tasks;

namespace The_CH33S3.ViewModels
{
    internal class VersatileConnectionStringHelper
    {
        static public string DateFormat = "MM-dd-yyyy h:mm:ss tt";

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

                        if (!string.IsNullOrWhiteSpace(cleanLine))
                        {
                            string normalized = NormalizeConnectionString(cleanLine);

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

            if (candidates.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No working connection string found.");
                return (string.Empty, false);
            }

            // 2. THE SPEED UP: TEST CONNECTION STRINGS IN PARALLEL
            // This starts a connection attempt for EVERY string at the exact same time
            var connectionTasks = candidates.Select(TryConnectAsync).ToList();

            // We monitor all the tasks. As soon as ONE of them succeeds, we lock it in!
            while (connectionTasks.Count > 0)
            {
                // Wait for the fastest task to finish (whether it succeeded or failed)
                Task<string> finishedTask = await Task.WhenAny(connectionTasks);
                connectionTasks.Remove(finishedTask);

                string result = await finishedTask;

                // If the result isn't null, it means it successfully connected!
                if (!string.IsNullOrEmpty(result))
                {
                    _cachedConnectionString = result;
                    System.Diagnostics.Debug.WriteLine("Fastest working connection locked in!");
                    return (_cachedConnectionString, true);
                }
            }

            // 3. TOTAL FAILURE
            System.Diagnostics.Debug.WriteLine("All connection strings failed.");
            return (string.Empty, false);
        }

        /// <summary>
        /// Helper method that attempts a single connection and returns null if it fails.
        /// </summary>
        private static async Task<string> TryConnectAsync(string connString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connString)
                {
                    ConnectTimeout = 3 // Fast fail for dead servers
                };

                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    await connection.OpenAsync();
                    return builder.ConnectionString; // Success! Return the string.
                }
            }
            catch
            {
                return null; // Failure! Return null so the race continues.
            }
        }

        private static string NormalizeConnectionString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string cleaned = input.Trim().Trim('"');

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cleaned);
                return builder.ConnectionString;
            }
            catch
            {
                return cleaned;
            }
        }
    }
}