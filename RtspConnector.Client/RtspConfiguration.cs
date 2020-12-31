using System.Collections.Generic;

namespace RtspConnector.Client
{
    public class RtspGlobal
    {
        public string BaseFolder { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string RtspAddress { get; set; }
        public string RtspPort { get; set; }

        /// <summary>
        /// Aufnahme-Dauer in Sekunden. Default: 30
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Wartezeit nach einer Aufnahme in Sekunden. Default: 10
        /// </summary>
        public int DelayAfterRecording { get; set; }

        /// <summary>
        /// Wartezeit nach einem Request zum Rtsp-Server in Sekunden. Default: 3
        /// </summary>
        public int RtspIntervall { get; set; }

        public List<RtspConfiguration> Cams { get; set; }
    }

    public class RtspConfiguration
    {
        private const string Protocol = "rtsp";

        /// <summary>
        /// Root-Folder, in das die Videos abgelegt werden
        /// Root-Folder: C:\Temp
        /// Ablage: C:\Temp\{CamName}\Event\yyyy-MM-dd\HH\yyyy-MM-dd-HH-mm-ss.mp4
        /// </summary>
        public string BaseFolder { get; set; }

        public string CamName { get; set; }

        public string User { get; set; }
        public string Password { get; set; }

        public string RtspAddress { get; set; }
        public string RtspPort { get; set; }

        /// <summary>
        /// Channel: /live1 oder /live2 ...
        /// </summary>
        public string Channel { get; set; }
        
        /// <summary>
        /// Aufnahme-Dauer in Sekunden. Default: 30
        /// </summary>
        public int Duration { get; set; }
        
        /// <summary>
        /// Wartezeit nach einer Aufnahme in Sekunden. Default: 10
        /// </summary>
        public int DelayAfterRecording { get; set; }

        /// <summary>
        /// Wartezeit nach einem Request zum Rtsp-Server in Sekunden. Default: 3
        /// </summary>
        public int RtspIntervall { get; set; }

        internal string GetUri()
        {
            return $"{Protocol}://{User}:{Password}@{RtspAddress}:{RtspPort}{Channel}";
        }
    }
}