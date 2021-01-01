using System;

namespace RtspConnector.Client
{
    public class Filepath
    {
        public DateTime Timestamp { get; private set; }
        public string TempPath { get; private set; }

        public string BasePath { get; private set; }

        public Filepath(DateTime timestamp, string basePath, string tempPath)
        {
            Timestamp = timestamp;
            BasePath = basePath;
            TempPath = tempPath;
        }
    }

    public class FilepathWorker
    {
        private const string Subfolder = "Event";

        public string GetPathBy(RtspConfiguration configuration)
        {
            var path = System.IO.Path
                .Combine(configuration.BaseFolder, configuration.CamName, Subfolder)
                .Replace(" ", "_");

            var invalidChars = System.IO.Path.GetInvalidPathChars();

            foreach (var c in invalidChars)
                path = path.Replace(c, '_');

            CreateDirIfNotExists(path);

            return path;
        }

        public Filepath CreateFilepath(string basepath)
        {
            var dt = DateTime.Now;
            var filepattern = $"{dt.ToString("yyyy-MM-dd-HH-mm-ss")}-{System.Guid.NewGuid()}.mp4";
            var temppath = System.IO.Path.Combine(basepath, filepattern);
            return new Filepath(dt, basepath, temppath);
        }

        public void MoveToDateBasedSubfolder(Filepath filepath, out string destination)
        {
            var fullfolderPath = System.IO.Path.Combine(filepath.BasePath, 
                filepath.Timestamp.ToString("yyyy-MM-dd"), filepath.Timestamp.ToString("HH"));

            CreateDirIfNotExists(fullfolderPath);

            var filepattern = $"{filepath.Timestamp.ToString("yyyy-MM-dd-HH-mm-ss")}.mp4";
            destination = System.IO.Path.Combine(fullfolderPath, filepattern);

            System.IO.File.Move(filepath.TempPath, destination);
        }

        private void CreateDirIfNotExists(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
