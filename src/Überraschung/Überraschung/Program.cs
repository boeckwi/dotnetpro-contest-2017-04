using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Überraschung
{
    class Program
    {
        static readonly string[] FallbackLines = new[]
        {
            "asdf"
        };

        static void Main(string[] args)
        {
            var input = ReadInputFileOrReturnExceptionStackTrace(args);

            var lines = ExtractParagraphsAsStringsOrReturnNull(input) ?? FallbackLines;

            var html = GenerateHtml(lines);

            var outputDirectory = GetInputDirectoryOrReturnNull(args) ?? GetApplicationDirectory();

            var outfile = GetNewOutputFilenameInDirectory(outputDirectory);

            File.WriteAllText(outfile.FullName, html);
            Console.WriteLine(outfile.Name);
        }

        static FileInfo GetNewOutputFilenameInDirectory(DirectoryInfo outputDirectory)
        {
            return new FileInfo(Path.Combine(outputDirectory.FullName, Guid.NewGuid().ToString() + ".html"));
        }

        static DirectoryInfo GetApplicationDirectory()
        {
            return new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        static DirectoryInfo GetInputDirectoryOrReturnNull(string[] args)
        {
            try
            {
                return new DirectoryInfo(Path.GetDirectoryName(args[0]));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        string GetOutputfilename()
        {
            throw new NotImplementedException();
        }

        static string ReadInputFileOrReturnExceptionStackTrace(string[] args)
        {
            try
            {
                return File.ReadAllText(args[0], Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        static string[] ExtractParagraphsAsStringsOrReturnNull(string input)
        {
            throw new NotImplementedException();
        }

        static string GenerateHtml(string[] lines)
        {
            throw new NotImplementedException();
        }
    }
}
