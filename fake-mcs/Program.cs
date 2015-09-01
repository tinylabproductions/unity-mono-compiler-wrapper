using System;
using System.IO;
using System.Linq;
using System.Text;

namespace fake_mcs
{
  class Program
  {
    static void Main(string[] args)
    {
      var asseblyLocation = typeof(Program).Assembly.Location;
      var sdkLocation = Path.GetFullPath(Path.Combine(
        Path.GetDirectoryName(asseblyLocation), @"..\..\..\..\..\..\unityfull"
      ));

      sdkLocation = Environment.GetEnvironmentVariable("UNITY_NEW_MONO_SDK") ?? sdkLocation;

      var mcsLocation = Environment.GetEnvironmentVariable("UNITY_NEW_MONO") 
        ?? Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Mono\lib\mono\4.5\mcs.exe");

      var procStartInfo =
        new System.Diagnostics.ProcessStartInfo(mcsLocation);
      //        new System.Diagnostics.ProcessStartInfo(@"C:\Program Files (x86)\Mono\bin\mono.exe");
      //        new System.Diagnostics.ProcessStartInfo(@"C:\Program Files (x86)\Mono\bin\mcs.bat");

      procStartInfo.Arguments = $@"""-sdk:{sdkLocation}"" --runtime:v2 " + args.Aggregate("", (s, s1) => s + " " + s1);

//      procStartInfo.Arguments = @"""C:\Program Files (x86)\Mono\lib\mono\4.5\mcs.exe"" " + procStartInfo.Arguments;

      procStartInfo.UseShellExecute = false;
//      procStartInfo.CreateNoWindow = true;
      procStartInfo.RedirectStandardError = true;
      procStartInfo.RedirectStandardOutput = true;

      var proc = new System.Diagnostics.Process();
      proc.StartInfo = procStartInfo;
      proc.Start();

      var output = new StringBuilder();
      var error = new StringBuilder();

      proc.OutputDataReceived += (sender, e) => {
        Console.WriteLine(e.Data);
        output.AppendLine(e.Data);
      };

      proc.ErrorDataReceived += (sender, e) => {
        Console.Error.WriteLine(e.Data);
        error.AppendLine(e.Data);
      };

      proc.BeginErrorReadLine();
      proc.BeginOutputReadLine();

      proc.WaitForExit();

      var outText = new StringBuilder();
      outText.AppendLine(procStartInfo.Arguments);

      var corlib_path = Path.GetDirectoryName(typeof(object).Assembly.Location);
      string fx_path = corlib_path.Substring(0, corlib_path.LastIndexOf(Path.DirectorySeparatorChar));
      outText.AppendLine(fx_path);
      outText.AppendLine(asseblyLocation);
      outText.AppendLine(sdkLocation);

//      var dict = Environment.GetEnvironmentVariables();
//      foreach (var k in dict.Keys) {
//        outText.AppendLine(k + " " + dict[k]);
//      }
      outText.AppendLine(Environment.CurrentDirectory);

      outText.AppendLine();
      outText.AppendLine("[Output start]");
      outText.AppendLine();
      outText.Append(output);
      outText.AppendLine("[Output end]");

      outText.AppendLine();
      outText.AppendLine("[Error start]");
      outText.AppendLine();
      outText.Append(error);
      outText.AppendLine("[Error end]");

      System.IO.File.WriteAllText(Environment.ExpandEnvironmentVariables(@"%TEMP%\UnityMonoUpdatedCompilerOutput.txt"), outText.ToString());
    }
  }
}
