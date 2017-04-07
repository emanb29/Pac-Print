using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacUPrintHelper
{
	class ViewModel : INotifyPropertyChanged
	{
		public class SourceFile
		{
			private string filePath;
			public static Collection<String> SourceExtensions = new Collection<string>() {"c", "cpp", "h", "hpp"};

			public String getFullyQualifiedName()
			{
				return filePath;
			}

			public String getFileName()
			{
				return Regex.Match(filePath, "^.*\\\\(.*\\..*)$").Groups[1].ToString();
			}

			public String getExtension()
			{
				return Regex.Match(filePath, "^.*\\.(.*)$").Groups[1].ToString();
			}

			public SourceFile(string path)
			{
				filePath = path;
			}

			public override string ToString()
			{
				return getFileName();
			}

			//Converting constructor
			public static implicit operator SourceFile(string path)
			{
				return new SourceFile(path);
			}
		}

		private string inFolder;

		public String cInFolder
		{
			get { return inFolder; }
			set
			{
				inFolder = value;
				OnPropertyChanged();
			}
		}

		private string outFile;

		public String cOutFile
		{
			get { return outFile; }
			set
			{
				outFile = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<SourceFile> cFileNames { get; set; } = new ObservableCollection<SourceFile>();

		public ViewModel()
		{
			cInFolder = "";
			cOutFile = "";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void ListFilesInDialog()
		{
			cFileNames.Clear();
			var fileList = Directory.GetFiles(cInFolder);
			for (var i = fileList.Length - 1; i >= 0; --i)
			{
				SourceFile cFile = fileList[i];
				var ext = cFile.getExtension();

				if (SourceFile.SourceExtensions.Contains(cFile.getExtension()))
				{
					cFileNames.Add(cFile);
				}
			}
		}

		public void DoMagic()
		{
			if (cFileNames.Count > 0 && cOutFile.Length > 0)
			{
				var outFile = new System.IO.StreamWriter(cOutFile);
				foreach (var file in cFileNames)
				{
					int lineNum = 0;
					var inFile = new System.IO.StreamReader(file.getFullyQualifiedName());
					string line;
					while ((line = inFile.ReadLine()) != null)
					{
						outFile.WriteLine(line);
						++lineNum;
						if (line.Length > 80)
						{
							lineNum += line.Length / 80;
						}
					}
					inFile.Close();
					while (lineNum % 52 != 0)
					{
						outFile.WriteLine();
						++lineNum;
					}
				}
				outFile.Close();
			}
		}
	}
}