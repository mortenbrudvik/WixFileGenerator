using System;
using System.IO;
using System.Text;

namespace WixFileGenerator
{
    // Simple helper program to generate Wix file reference list
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();
            if (args.Length != 2)
            {
                logger.Info("usage: wixfilegenerator [<file folder path>] [<Source prefix>]");
                logger.Info("");
                logger.Info("Example: wixfilegenerator \"c:\\wixfolder\" $(var.Binaries_TargetDir)");
                logger.Info("");
                return;
            }
            var folderPath = args[0];
            var binariesTargetDir = args[1];

            if (!Directory.Exists(folderPath))
            {
                logger.Info("Please provide a valid folder path");
                return;
            }

            var files = Directory.GetFiles(folderPath);

            var stringBuilder = new StringBuilder();
            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var guid = Guid.NewGuid().ToString().ToLower();

                stringBuilder.AppendLine($"<Component Id=\"{filename}\" Guid=\"{guid}\" >");
                stringBuilder.AppendLine($"\t<File Id=\"{filename}\" Name=\"{filename}\" Source=\"{binariesTargetDir}{filename}\" />");
                stringBuilder.AppendLine("</Component>");
            }

            File.WriteAllText("wixfiles.txt", stringBuilder.ToString());
        }
    }
}
