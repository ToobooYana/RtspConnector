using ColinChang.FFmpegHelper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RtspConnector.Client
{
    public interface IConnector
    {

    }

    internal class Connector
    {

        private const string Subfolder = "Event";

        public Connector()
        {

        }

        public async Task RunAsync(RtspConfiguration configuration, CancellationToken token)
        {
            var rtsp = InitializeBy(configuration);

            await RecordAsync(rtsp, configuration, token);
        }

        private RtspHelper InitializeBy(RtspConfiguration configuration)
        {
            var uri = configuration.GetUri();
            var rtsp = new RtspHelper(uri, 3000);

            return rtsp;
        }

        private async Task RecordAsync(RtspHelper rtsp, RtspConfiguration configuration, CancellationToken token)
        {
            var basepath = GetPathBy(configuration);

            if (token.IsCancellationRequested)
                return;


            bool result;
            do
            {
                var dt = DateTime.Now;
                var fullfolderPath = System.IO.Path.Combine(basepath, dt.ToString("yyyy-MM-dd"), dt.ToString("HH"));
                CreateDirIfNotExists(fullfolderPath);

                var filepattern = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.mp4";

                var output = System.IO.Path.Combine(fullfolderPath, filepattern);

                if (token.IsCancellationRequested)
                    return;

                result = await rtsp.Record2VideoFileAsync(
                        output,
                        TimeSpan.FromSeconds(configuration.Duration), // default: 30
                        null,
                        Transport.Tcp
                        );

                if (result)
                    await Task.Delay(configuration.DelayAfterRecording * 1000); // Default: 10
                else
                    await Task.Delay(configuration.RtspIntervall * 1000); // Default: 3

                if (token.IsCancellationRequested)
                    return;

            } while (true);
        }

        private void CreateDirIfNotExists(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private string GetPathBy(RtspConfiguration configuration)
        {
            var path = System.IO.Path.Combine(configuration.BaseFolder, configuration.CamName, Subfolder);

            return path.Replace(" ", "_");
        }
    }
}
