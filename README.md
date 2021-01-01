# RtspConnector
Connector for surveillance cameras

To store the recorded streams from surveillance cameras, such as **Eufy**. The recording is done via RTSP stream. Periodically it is obeyed if new streams are available. If this is the case, this stream is stored as a file in the specified directory:

> [BasePath]\[Cam-Name]\Event\yyyy-MM-dd\HH

For example:

> \\\\NAS\Recordings\Garage\Event\2020-12-31\15

This project uses:
FFmpegHelper: https://github.com/colin-chang/FFmpegHelper
FFmpeg Libraries: http://ffmpeg.org/

Many thanks to Colin Chang for making this repository FFmpegHelper available!
