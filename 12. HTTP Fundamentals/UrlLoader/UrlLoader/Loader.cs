using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace UrlLoader
{
	public class Loader
	{
		private string _pathEndDest;

		private readonly string _address;
		private readonly string _pathDest;
		private readonly int _maxLevel;
		private readonly bool _isDomenOnly;
		private readonly List<string> _loadableFileList;
		private readonly bool _isVerbose;

		private readonly Dictionary<string, int> loadedPage;
		private readonly List<string> loadedFile;
		private readonly Encoding encode;

		private readonly string _ptrnFileReplace;
		private readonly Regex _regexFileReplace;

		private readonly ILogService _logger;


		public Loader(string address, string pathDest, int maxLevel, bool isDomenOnly, List<string> loadableFileList, bool isVerbose, ILogService logger)
		{
			_address = address;
			_pathDest = pathDest;
			_maxLevel = maxLevel;
			_isDomenOnly = isDomenOnly;
			_loadableFileList = loadableFileList;
			if (!loadableFileList.Any())
				_loadableFileList.Add("*");
			_isVerbose = isVerbose;

			loadedPage = new Dictionary<string, int>();
			loadedFile = new List<string>();
			encode = Encoding.GetEncoding("utf-8");

			_ptrnFileReplace = "\\*|\\?|\"|<|>|\\||:";
			_regexFileReplace = new Regex(_ptrnFileReplace);

			_logger = logger;
		}

		public void Load()
		{
			_pathEndDest = _pathDest + _address.Substring(_address.LastIndexOf("www.")) + "/";
			Directory.CreateDirectory(_pathEndDest);
			ScanUrl(_address, 0);
		}

		private void saveFile(WebClient webClient, string path, string fileRef, string url, string stringPage)
		{
			if (string.IsNullOrWhiteSpace(fileRef)) return;

			string newFileRef = _regexFileReplace.Replace(fileRef.Replace(@"/", @"\"), "_");
			string fileName = path + newFileRef;

			if (fileName.EndsWith("\\")) return;

			Directory.CreateDirectory(fileName.Substring(0, fileName.LastIndexOf(@"\")));

			if (!string.IsNullOrEmpty(stringPage))
			{
				File.WriteAllText(fileName, stringPage);
				Log(string.Format("Загружена страница {0}", fileName));
			}
			else
			{
                if (_loadableFileList.Contains("*") || _loadableFileList.Contains(Path.GetExtension(fileName)))
				{
					webClient.DownloadFile(url, fileName);
					loadedFile.Add(fileRef);
					Log(string.Format("Загружен файл {0}", fileName));
				}
			}

		}

		private void scanNode(WebClient webClient, HtmlNodeCollection nodeList, string typeRef, int level)
		{
			foreach (HtmlNode node in nodeList)
			{
				string refVal = node.GetAttributeValue(typeRef, null);

				if (!string.IsNullOrEmpty(refVal))
				{
					if ((!loadedPage.ContainsKey(refVal) || loadedPage[refVal] >= level) && (!loadedFile.Contains(refVal)))
					{
						if (!loadedPage.ContainsKey(refVal)) loadedPage.Add(refVal, level);
						else loadedPage[refVal] = level;

						Log(string.Format("Идет обработка {0}", refVal));

						if ((!refVal.StartsWith("http") || !_isDomenOnly)
							&& !refVal.StartsWith("//")
						   )
						{
							string hrefFullUrl = _address + (refVal.StartsWith("/") ? refVal : "/" + refVal);
							string stringPage = ScanUrl(hrefFullUrl, level);
							saveFile(webClient, _pathEndDest, refVal, hrefFullUrl, stringPage);
						}
					}
				}
			}
		}

		private string ScanUrl(string url, int level)
		{
			try
			{
				// а тут что юзинг забыл?
				// WebRequest не IDisposable
				WebRequest webRequest = WebRequest.Create(url);

				using (WebClient webClient = new WebClient())
				using (WebResponse webResponse = webRequest.GetResponse())
				using (var reader = new StreamReader(webResponse.GetResponseStream(), encode))
				{
					string htmlString = reader.ReadToEnd();

					if (htmlString.IndexOf("html") > 0)
					{
						HtmlDocument htmlDocument = new HtmlDocument();
						htmlDocument.LoadHtml(htmlString);

						HtmlNodeCollection nodeList;

						if (level <= _maxLevel)
						{
							nodeList = htmlDocument.DocumentNode.SelectNodes("//img");
							if (nodeList != null) scanNode(webClient, nodeList, "src", level + 1);

							nodeList = htmlDocument.DocumentNode.SelectNodes("//link");
							if (nodeList != null) scanNode(webClient, nodeList, "href", level + 1);

							nodeList = htmlDocument.DocumentNode.SelectNodes("//a");
							if (nodeList != null) scanNode(webClient, nodeList, "href", level + 1);
						}

						if (level == 0)
							saveFile(webClient, _pathEndDest, "index.html", null, htmlDocument.DocumentNode.OuterHtml);

						return htmlDocument.DocumentNode.OuterHtml;
					}
					else return null;
				}
			}
			catch (WebException e)
			{
				Log(string.Format("Ошибка: {0}", e.Message));
				return null;
			}
		}

		private void Log(string logMessage)
		{
			if (_isVerbose) _logger.Print(logMessage);
		}
	}

	public interface ILogService
	{
		void Print(string message);
	}
}
