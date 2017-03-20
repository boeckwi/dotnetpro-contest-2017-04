using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Überraschung
{
    class Program
    {
        static readonly string[] FallbackLines = new[]
        {
            "Hier könnte der Inhalt ihrer Textdatei stehen."
        };

        static void Main(string[] args)
        {
            var input = ReadInputFileOrReturnExceptionStackTrace(args);

            var lines = ExtractParagraphsAsStringsOrReturnNull(input) ?? FallbackLines;

            var html = GenerateHtml(lines);

            var outputDirectory = GetInputDirectoryOrReturnNull(args) ?? GetApplicationDirectory();

            var outfile = GetNewOutputFilenameInDirectory(outputDirectory);

            File.WriteAllText(outfile.FullName, html, Encoding.UTF8);
            Console.WriteLine(outfile.Name);
        }

        static FileInfo GetNewOutputFilenameInDirectory(DirectoryInfo outputDirectory)
        {
            var defaultFile = new FileInfo(Path.Combine(outputDirectory.FullName, "output.html"));
            if (!defaultFile.Exists) return defaultFile;

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
                return new DirectoryInfo(Path.GetDirectoryName(new FileInfo(args[0]).FullName));
            }
            catch (Exception ex)
            {
                return null;
            }
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
            try
            {
                var lines = input.Split(new[] { '\r', '\n' }).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (!lines.Any()) return null;
                return lines;
            }
            catch (Exception)
            {
                return null;
            }
        }

        static string GenerateHtml(string[] lines)
        {
            var template = GetTemplate();

            var js_textarray = string.Join(",\r\n", lines.Select(line => string.Format("\"{0}\"", Cleanup(line))));

            return template.Replace("%%JS_TEXTARRAY%%", js_textarray);
        }

        static string Cleanup(string line) // remove chars that are not easy to type
        {
            return line.Replace("\"", "");
        }

        public static string GetTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Überraschung.template.html";

            string xml;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new ArgumentException("Resource nicht gefunden: " + resourceName);

                using (var reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }
            return xml;
        }
    }
}
