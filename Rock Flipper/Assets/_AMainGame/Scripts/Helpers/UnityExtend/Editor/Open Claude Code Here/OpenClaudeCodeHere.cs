using System.Diagnostics;
using System.IO;
using UnityEditor;

public static class OpenClaudeCodeHere
{
    [MenuItem("Assets/Open Claude Code here...", false, 20)]
    private static void Open()
    {
        LaunchTerminal(GetSelectedFolderPath());
    }

    private static string GetSelectedFolderPath()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }

        var fullPath = Path.GetFullPath(path);
        if (File.Exists(fullPath))
        {
            fullPath = Path.GetDirectoryName(fullPath);
        }

        return fullPath;
    }

    private static void LaunchTerminal(string workingDirectory)
    {
#if UNITY_EDITOR_WIN
        Process.Start(new ProcessStartInfo("cmd.exe", "/K claude")
        {
            WorkingDirectory = workingDirectory,
            UseShellExecute = true,
        });
#elif UNITY_EDITOR_OSX
        var escapedPath = workingDirectory.Replace("\"", "\\\"");
        var script = $"tell application \"Terminal\" to do script \"cd \\\"{escapedPath}\\\" && claude\"";
        Process.Start(new ProcessStartInfo("osascript", $"-e '{script}'")
        {
            UseShellExecute = true,
        });
#else
        Process.Start(new ProcessStartInfo("x-terminal-emulator", "-e bash -c \"claude; exec bash\"")
        {
            WorkingDirectory = workingDirectory,
            UseShellExecute = true,
        });
#endif
    }
}
