using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace App.Application.Util
{
    public static class VideoUtil
    {

        public static List<byte[]> CaptureScreenshots(string base64Video, int id)
        {
            var screenshots = new List<byte[]>();
            var videoBytes = Convert.FromBase64String(base64Video);
            var tempVideoPath = Path.Combine(AppContext.BaseDirectory, id + ".mp4");

            System.IO.File.WriteAllBytes(tempVideoPath, videoBytes);

            using var capture = new VideoCapture(tempVideoPath);
            var frameCount = capture.FrameCount;
            var fps = capture.Fps;
            var duration = frameCount / fps;

            for (int i = 0; i < duration; i += 5)
            {
                capture.Set(VideoCaptureProperties.PosMsec, i * 1000);
                using var frame = new Mat();
                capture.Read(frame);

                if (frame.Empty())
                    break;

                var outputPath = Path.Combine(AppContext.BaseDirectory, $"{Guid.NewGuid()}.png");
                Cv2.ImWrite(outputPath, frame);

                var screenshotBytes = File.ReadAllBytes(outputPath);
                screenshots.Add(screenshotBytes);
                File.Delete(outputPath);
            }

            // File.Delete(tempVideoPath);
            return screenshots;
        }
        public static byte[] CreateZipFile(List<byte[]> screenshots)
        {
            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < screenshots.Count; i++)
                    {
                        var entry = archive.CreateEntry($"screenshot_{i + 1}.png");
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(screenshots[i], 0, screenshots[i].Length);
                        }
                    }
                }
                return zipStream.ToArray();
            }
        }

    }
}
