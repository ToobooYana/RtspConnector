using ColinChang.FFmpegHelper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RtspConnector.Client
{
    public interface IConnector
    {
        Task RunAsync(RtspConfiguration configuration, CancellationToken token);
    }

    internal class Connector : IConnector
    {
        private FilepathWorker _filepathWorker;

        public Connector()
        {
            _filepathWorker = new FilepathWorker();
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
            var basepath = _filepathWorker.GetPathBy(configuration);

            if (token.IsCancellationRequested)
                return;

            int healthcheck = 0;

            bool result;
            do
            {
                var filepath = _filepathWorker.CreateFilepath(basepath);

                if (token.IsCancellationRequested)
                    return;
                
                if (healthcheck == 0)
                    System.Console.WriteLine($"{filepath.Timestamp.ToString("G")} => Working for {configuration.CamName}");
                
                healthcheck++;

                result = await rtsp.Record2VideoFileAsync(
                        filepath.TempPath,
                        TimeSpan.FromSeconds(configuration.Duration), // default: 30
                        null,
                        Transport.Tcp
                        );

                if (result)
                {
                    _filepathWorker.MoveToDateBasedSubfolder(filepath, out var destination);
                    System.Console.WriteLine($" ==> Recording exists in '{destination}'.");

                    await Task.Delay(configuration.DelayAfterRecording * 1000); // Default: 10
                }
                else
                {
                    await Task.Delay(configuration.RtspIntervall * 1000); // Default: 3
                }

                if (token.IsCancellationRequested)
                    return;

                if (healthcheck == 10)
                    healthcheck = 0;

            } while (true);
        }

        

    }
}
