# unity-mono-compiler-wrapper

Updates Unity mono compiler to a newer version.

Released zip available here: [Releases](https://github.com/tinylabproductions/unity-mono-compiler-wrapper/releases)

Instructions:
 - Install latest mono http://www.mono-project.com/download/#download-win
 - Install UnityVS http://unityvs.com/
 - Copy `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5\Profile\Unity Full v3.5` to `C:\Program Files\Unity\unityfull`
 - Build attached project and run `copy-files-release.bat` as administrator (you may also want to backup them first)

If you have problems check the log file at `%TEMP%\UnityMonoUpdatedCompilerOutput.txt`
