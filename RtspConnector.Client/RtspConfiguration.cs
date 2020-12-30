using System.Collections.Generic;

namespace RtspConnector.Client
{
    public class RtspGlobal
    {
        public string BaseFolder { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string RtspAddress { get; }
        public string RtspPort { get; }

        /// <summary>
        /// Aufnahme-Dauer in Sekunden. Default: 30
        /// </summary>
        public int Duration { get; internal set; }

        /// <summary>
        /// Wartezeit nach einer Aufnahme in Sekunden. Default: 10
        /// </summary>
        public int DelayAfterRecording { get; internal set; }

        /// <summary>
        /// Wartezeit nach einem Request zum Rtsp-Server in Sekunden. Default: 3
        /// </summary>
        public int RtspIntervall { get; internal set; }

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

        public string RtspAddress { get; }
        public string RtspPort { get; }

        /// <summary>
        /// Channel: /live1 oder /live2 ...
        /// </summary>
        public string Channel { get; set; }
        
        /// <summary>
        /// Aufnahme-Dauer in Sekunden. Default: 30
        /// </summary>
        public int Duration { get; internal set; }
        
        /// <summary>
        /// Wartezeit nach einer Aufnahme in Sekunden. Default: 10
        /// </summary>
        public int DelayAfterRecording { get; internal set; }

        /// <summary>
        /// Wartezeit nach einem Request zum Rtsp-Server in Sekunden. Default: 3
        /// </summary>
        public int RtspIntervall { get; internal set; }

        internal string GetUri()
        {
            return $"{Protocol}://{User}:{Password}@{RtspAddress}:{RtspPort}{Channel}";
        }
    }
}