using System.Runtime.InteropServices;

namespace AudioRecorder
{
	class MainClass
	{
		public static volatile NAudio.Wave.WasapiLoopbackCapture waveIn;
		public static string fileName = "out.mp3";
		public static NAudio.Lame.LameMP3FileWriter waveFileWriter;
		public static void Main()
		{
			SetConsoleCtrlHandler(new ConsoleEventDelegate(ConsoleEventCallback), true);
			waveIn = new NAudio.Wave.WasapiLoopbackCapture();
			waveFileWriter = new NAudio.Lame.LameMP3FileWriter(@fileName, waveIn.WaveFormat, NAudio.Lame.LAMEPreset.STANDARD);
			waveIn.DataAvailable += waveIn_DataAvailable;
			waveIn.RecordingStopped += waveIn_RecordingStopped;
			waveIn.StartRecording();

		}
		static void waveIn_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
		{
			waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
		}
		static void waveIn_RecordingStopped(object sender, NAudio.Wave.StoppedEventArgs e)
		{
			waveIn.Dispose();
			waveFileWriter.Close();
			waveFileWriter.Dispose();
		}

		static bool ConsoleEventCallback(int eventType)
		{
			if (eventType == 2 || eventType == 0)
			{
				waveIn.StopRecording();
				System.Threading.Thread.Sleep(1000);
			}
			return false;
		}
		static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
		static ConsoleEventDelegate Handler { get { return handler; } set { handler = value; } }

		// Pinvoke
		delegate bool ConsoleEventDelegate(int eventType);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
	}
}
