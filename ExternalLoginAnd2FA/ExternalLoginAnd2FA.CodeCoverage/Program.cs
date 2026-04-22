using System.Diagnostics;

namespace ExternalLoginAnd2FA.CodeCoverage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Get the solution directory (parent of CoverageRunner directory)
                string solutionDir = FindSolutionDirectory()
                    ?? throw new Exception("Could not determine solution directory.");

                // Ensure code-coverage directory exists
                string coverageDir = Path.Combine(solutionDir, "code-coverage");
                Directory.CreateDirectory(coverageDir);

                // Clear previous coverage file to avoid merge issues
                string coverageFile = Path.Combine(coverageDir, "coverage.opencover.xml");
                if (File.Exists(coverageFile))
                {
                    File.Delete(coverageFile);
                    Console.WriteLine("Cleared previous coverage file: " + coverageFile);
                }

                // Command 1: Run dotnet test with coverage
                Console.WriteLine("Running dotnet test with coverage...");
                RunCommand(
                    "dotnet",
                    $"test --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput={solutionDir}/code-coverage/coverage.opencover.xml",
                    solutionDir
                );

                // Command 2: Run reportgenerator
                Console.WriteLine("Generating coverage report...");
                RunCommand(
                    "reportgenerator",
                    "-reports:./code-coverage/coverage.opencover.xml -targetdir:./code-coverage/coverage-report -reporttypes:Html",
                    solutionDir
                );

                Console.WriteLine("Coverage report generated at code-coverage/coverage-report/index.html");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static string? FindSolutionDirectory()
        {
            string? currentDir = Directory.GetCurrentDirectory();
            while (currentDir != null)
            {
                if (Directory.GetFiles(currentDir, "*.sln").Length > 0)
                {
                    return currentDir;
                }
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }
            return null;
        }

        private static void RunCommand(string command, string arguments, string workingDirectory)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processInfo };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Command '{command} {arguments}' failed with exit code {process.ExitCode}. Error: {error}");
            }

            Console.WriteLine(output);
        }
    }
}
