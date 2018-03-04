using VideoIoC;

namespace VideoLib
{
    public class VideoManager1
    {
        [Import]
        public IVideoProvider VideoProvider { get; set; }
    }

    [ImportConstructor]
    public class VideoManager2
    {
        public IVideoProvider VideoProvider { get; set; }

        public VideoManager2(IVideoProvider videoProvider)
        {
            VideoProvider = videoProvider;
        }
    }
}
