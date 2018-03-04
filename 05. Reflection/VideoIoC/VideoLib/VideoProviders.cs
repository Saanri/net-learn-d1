using System;
using System.Collections.Generic;
using VideoIoC;

namespace VideoLib
{
	public class VideoProvider : IVideoProvider
	{
		public void PlayVideo()
		{
			Console.WriteLine("VideoProvider.PlayVideo");
		}
	}

    [Export(typeof(IVideoProvider))]
    public class VideoProviderSite : IVideoProvider
	{
		public void PlayVideo()
		{
			Console.WriteLine("VideoProviderSite.PlayVideo");
		}
	}
}
