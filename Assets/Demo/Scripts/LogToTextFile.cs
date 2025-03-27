using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace LogToFile
{
	public class LogToTextFile : MonoBehaviour
	{

		#region Variables

		[SerializeField]
		private string _folderName = "Logs";

		[SerializeField]
		private int _maxByteSite = 1048576;// 1048576 bytes = 1 MB

		public long ActualSize { get; private set; } = 0;

		private string _filePathAndName;

		public bool CanWrite
		{
            get
            {
                return _canWrite && !MaxSizeReached;
            }

            set
            {
                _canWrite = value && !MaxSizeReached;
            } 
        }

        private bool _canWrite = false;

		public bool MaxSizeReached => ActualSize >= _maxByteSite;

#if UNITY_EDITOR
		[SerializeField]
		private bool _useInEditor = false;
#endif

		private string newLine;
		private string newLineTab;

		private string newLineTwoTab;

		#endregion

		#region Init And Quit

		private void Awake()
		{
#if UNITY_EDITOR
			if (!_useInEditor)
			{
				enabled = false;
				return;
			}
#endif

			newLine = Environment.NewLine;
			newLineTab = $"{newLine}\t";
			newLineTwoTab = $"{newLineTab}\t";

			string directoryPath = $"{Application.dataPath}/../{_folderName}";

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			Debug.Log("Logs directory path: " + directoryPath);

			_filePathAndName = $"{directoryPath}/Log {DateTime.Now:yyyy'-'MM'-'dd HH'h'mm'm'ss's'}.log";

			try
			{
				using (var logFile = new StreamWriter(_filePathAndName, true))
				{
					CanWrite = true;
					logFile.Close();
				}

				WriteToLogFile("Application Start");
			}
			catch (Exception e)
			{
				Debug.LogError(e, this);
			}
		}

		private void OnEnable()
		{
			if (_canWrite)
			{
				Application.logMessageReceived += OnLog;
			}
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= OnLog;
		}

		private void OnApplicationQuit()
		{
			if (_canWrite)
			{
				WriteToLogFile("Application Quit");
			}
		}

		#endregion

		#region OnLog

		private void OnLog(string message, string stackTrace, LogType type)
		{
			if (!_canWrite)
			{
				return;
			}

			switch (type)
			{
				case LogType.Log:
				case LogType.Warning:
					FormatAndWriteToLogFile(message, null, type);
					break;

				default:
					FormatAndWriteToLogFile(message, stackTrace, type);
					break;
			}
		}

		#endregion

		#region FormatAndWriteToLogFile

		private readonly StringBuilder messageBuilder = new StringBuilder(4096);
		private readonly StringBuilder stackTraceBuilder = new StringBuilder(4096);

		private static readonly string replaceSymbol = "¤";
		private void FormatAndWriteToLogFile(string message, string stackTrace = null, LogType type = LogType.Log)
		{
			messageBuilder
			.Clear()
			.Append(message)
			.Replace("\r\n", replaceSymbol)
			.Replace("\r", replaceSymbol)
			.Replace("\n", replaceSymbol)
			.Replace(replaceSymbol, newLineTab);

			if (!string.IsNullOrEmpty(stackTrace))
			{
				stackTraceBuilder
				.Clear()
				.Append(replaceSymbol)
				.Append(stackTrace)
				.Replace("\r\n", replaceSymbol)
				.Replace("\r", replaceSymbol)
				.Replace("\n", replaceSymbol)
				.Replace(replaceSymbol, newLineTwoTab);

				messageBuilder.Append(stackTraceBuilder);
			}

			WriteToLogFile(messageBuilder.ToString(), type);
		}

		#endregion

		#region WriteToLogFile

		private int _frameCount = -1;

		private void WriteToLogFile(string message, LogType type = LogType.Log)
		{
			try
			{
				using (var logFile = new StreamWriter(_filePathAndName, true))
				{
					if (_frameCount != Time.frameCount)
					{
						_frameCount = Time.frameCount;

						var fileInfo = new FileInfo(_filePathAndName);

						logFile.WriteLineAsync($"{newLine}{DateTime.Now:yyyy'.'MM'.'dd HH':'mm':'ss} frame {_frameCount}{newLine}");

						ActualSize = logFile.BaseStream.Length;

						if (MaxSizeReached)
						{
							_canWrite = false;

							string finalText = $"The file has exceeded the allowed size: {ActualSize} > {_maxByteSite}";

							logFile.WriteLineAsync($"\t{finalText}");
							logFile.Close();

							Debug.LogWarning(finalText);

							return;
						}
					}

					if (type == LogType.Log)
					{
						logFile.WriteLineAsync($"\t{message}{newLine}");
					}
					else
					{
						logFile.WriteLineAsync($"\t[{type}] {message}{newLine}");
					}

					logFile.Close();
				}
			}
			catch (Exception e)
			{
				bool canWriteState = _canWrite;
				_canWrite = false;
				Debug.LogError($"Error while trying to write into log file {e.Message}");
				_canWrite = canWriteState;
			}
		}

		#endregion

	}
}