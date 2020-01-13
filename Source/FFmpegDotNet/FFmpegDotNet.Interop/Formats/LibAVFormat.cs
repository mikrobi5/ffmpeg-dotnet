
#region Using Directives

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;


#endregion

namespace FFmpegDotNet.Interop.Formats
{

    public static partial class AVCodecLibraryResolver
    {
        const string _aVCodec = Libraries.AVCodec;
        const string _aVFormat = Libraries.AVFormat;
        private static IntPtr _localAvFormatNativeLibrary = IntPtr.Zero;
        private static IntPtr _localAvCodecNativeLibrary = IntPtr.Zero;

        public static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == _aVFormat)
            {
                if (_localAvFormatNativeLibrary == IntPtr.Zero)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        string codeBase = assembly.CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        var test = Path.GetDirectoryName(path);
                        var ffmpegPath = Path.Combine(test, "ffmpeg");
                        var files = Directory.GetFiles(ffmpegPath, "*avformat*.dll");
                        NativeLibrary.TryLoad(files.FirstOrDefault(), out _localAvFormatNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        NativeLibrary.TryLoad("cmsCore", out _localAvFormatNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        NativeLibrary.TryLoad("cmsCore.dylib", out _localAvFormatNativeLibrary);
                    }
                }
                return _localAvFormatNativeLibrary;
            }

            if (libraryName == _aVCodec)
            {
                if (_localAvCodecNativeLibrary == IntPtr.Zero)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        string codeBase = assembly.CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        var test = Path.GetDirectoryName(path);
                        var ffmpegPath = Path.Combine(test, "ffmpeg");
                        var files = Directory.GetFiles(ffmpegPath, "*avcodec*.dll");
                        NativeLibrary.TryLoad(files.FirstOrDefault(), out _localAvCodecNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        NativeLibrary.TryLoad("cmsCore", out _localAvCodecNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        NativeLibrary.TryLoad("cmsCore.dylib", out _localAvCodecNativeLibrary);
                    }
                }
                return _localAvCodecNativeLibrary;
            }
            return IntPtr.Zero;
        }

    }

    public static partial class AVUtilsLibraryResolver
    {
        const string _aVUtil = Libraries.AVUtil;
        private static IntPtr _localAvUtilNativeLibrary = IntPtr.Zero;

        public static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {


            if (libraryName == _aVUtil)
            {
                if (_localAvUtilNativeLibrary == IntPtr.Zero)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        string codeBase = assembly.CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        var test = Path.GetDirectoryName(path);
                        var ffmpegPath = Path.Combine(test, "ffmpeg");
                        var files = Directory.GetFiles(ffmpegPath, "*avutil*.dll");
                        NativeLibrary.TryLoad(files.FirstOrDefault(), out _localAvUtilNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        NativeLibrary.TryLoad("cmsCore", out _localAvUtilNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        NativeLibrary.TryLoad("cmsCore.dylib", out _localAvUtilNativeLibrary);
                    }
                }
                return _localAvUtilNativeLibrary;
            }
            return IntPtr.Zero;
        }

    }

    public static partial class SWScaleLibraryResolver
    {
        const string _swScale = Libraries.SwScale;
        private static IntPtr _localSwScaleNativeLibrary = IntPtr.Zero;

        public static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {


            if (libraryName == _swScale)
            {
                if (_localSwScaleNativeLibrary == IntPtr.Zero)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        string codeBase = assembly.CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        var test = Path.GetDirectoryName(path);
                        var ffmpegPath = Path.Combine(test, "ffmpeg");
                        var files = Directory.GetFiles(ffmpegPath, "*swscale*.dll");
                        NativeLibrary.TryLoad(files.FirstOrDefault(), out _localSwScaleNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        NativeLibrary.TryLoad("cmsCore", out _localSwScaleNativeLibrary);
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        NativeLibrary.TryLoad("cmsCore.dylib", out _localSwScaleNativeLibrary);
                    }
                }
                return _localSwScaleNativeLibrary;
            }
            return IntPtr.Zero;
        }

    }

    /// <summary>
    /// Represents a simple PInvoke wrapper aroung the libavformat library of the FFmpeg project.
    /// </summary>
    public static class LibAVFormat
    {

        const string AVFormat = Libraries.AVFormat;

        //static LibAVFormat()
        //{
        //    NativeLibrary.SetDllImportResolver(typeof(LibAVFormat).Assembly, AVFormatLibraryResolver.ImportResolver);
        //}

        #region Public Methods

        /// <summary>
        /// Initializes libavformat and registers all the muxers, demuxers and protocols.
        /// </summary>
        [DllImport(AVFormat)]
        public static extern void av_register_all();

        /// <summary>
        /// Open an input stream and read the header. The codecs are not opened. The stream must be closed with avformat_close_input().
        /// </summary>
        /// <param name="ps"> <summary>
        /// Pointer to the user-supplied AVFormatContext (allocated by avformat_alloc_context). May be a pointer to <c>null</c>, in which case an
        /// <see cref="AVFormatContext"/> is allocated by this function and written into <see cref="ps"/> Note that a user-supplied
        /// <see cref="AVFormatContext"/> will be freed on failure.
        /// </summary>
        /// <param name="url">URL of the stream to open.</param>
        /// <param name="fmt">If non-<c>null</c>, this parameter forces a specific input format. Otherwise the format is autodetected.</param>
        /// <param name="options">
        /// A dictionary filled with <see cref="AVFormatContext"/> and demuxer-private options. On return this parameter will be destroyed and replaced
        /// with a dictionary containing options that were not found. May be <c>null</c>.
        /// </param>
        /// <returns>Returns 0 on success, and a negative AVERROR on failure.</returns>
        [DllImport(AVFormat)]
        public static extern int avformat_open_input([Out] out IntPtr ps, [MarshalAs(UnmanagedType.LPStr)] string url, IntPtr fmt, IntPtr options);

        /// <summary>
        /// Close an opened input <see cref="AVFormatContext"/>. Free it and all its contents and set <see cref="s"/> to <c>null</c>.
        /// </summary>
        /// <param name="s">The <see cref="AVFormatContext"/> that is to be closed.</param>
        [DllImport(AVFormat)]
        public static extern void avformat_close_input(IntPtr s);

        /// <summary>
        /// Read packets of a media file to get stream information. This is useful for file formats with no headers such as MPEG. This function also computes the
        /// real framerate in case of MPEG-2 repeat frame mode. The logical file position is not changed by this function; examined packets may be buffered for
        /// later processing. Note, this function isn't guaranteed to open all the codecs, so options being non-empty at return is a perfectly normal behavior.
        /// </summary>
        /// <param name="ic">The <see cref="AVFormatContext"/> media file handle.</param>
        /// <param name="options">
        /// If non-<c>null</c>, an ic.nb_streams long array of pointers to dictionaries, where i-th member contains options for codec corresponding to i-th stream.
        /// On return each dictionary will be filled with options that were not found.
        /// </param>
        /// <returns>Returns a value greater than or equal to 0 when everything went alright and a negative number otherwise.</returns>
        [DllImport(AVFormat)]
        public static extern int avformat_find_stream_info(IntPtr ic, IntPtr options);

        /// <summary>
        /// Retrieves the next frame of a stream. This function returns what is stored in the file, and does not validate that what is there are valid frames for
        /// the decoder. It will split what is stored in the file into frames and return one for each call. It will not omit invalid data between valid frames so
        /// as to give the decoder the maximum information possible for decoding. If pkt->buf is NULL, then the packet is valid until the next av_read_frame() or
        /// until avformat_close_input(). Otherwise the packet is valid indefinitely. In both cases the packet must be freed with av_packet_unref when it is no
        /// longer needed. For video, the packet contains exactly one frame. For audio, it contains an integer number of frames if each frame has a known fixed
        /// size (e.g. PCM or ADPCM data). If the audio frames have a variable size (e.g. MPEG audio), then it contains one frame. pkt->pts, pkt->dts and
        /// pkt->duration are always set to correct values in AVStream.time_base units (and guessed if the format cannot provide them). pkt->pts can be
        /// AV_NOPTS_VALUE if the video format has B-frames, so it is better to rely on pkt->dts if you do not decompress the payload.
        /// </summary>
        /// <param name="s">A pointer to the <see cref="seeAVFormatContext"/>.</param>
        /// <param name="pkt">The packet that is benig output.</param>
        /// <returns>Return 0 if everything went alright, a negative value other for an error, or end of file.</returns>
        [DllImport(AVFormat)]
        public static extern int av_read_frame(IntPtr s, IntPtr pkt);

        #endregion
    }
}