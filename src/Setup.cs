using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

/// <summary>
/// DeepSeek Harness Launcher - Setup
/// A single-file installer + launcher.
/// Embeds: start-dsh.cmd template (resource "LauncherCmd") and the whale
/// girl icon (resource "WhaleIcon").
///
/// Usage:
///   (no args)        Install (if needed) then launch
///   -installonly     Install only, do not launch
///   -launch          Launch only (requires an existing install)
///   -dir <path>      Override install directory
///   -noshortcut      Do not create the desktop shortcut
/// </summary>
public static class Setup
{
    public static int Main(string[] args)
    {
        string installDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DeepSeekHarness-Launcher");
        bool installOnly = false;
        bool launchOnly = false;
        bool noShortcut = false;

        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "-installonly", StringComparison.OrdinalIgnoreCase)) installOnly = true;
            else if (string.Equals(args[i], "-launch", StringComparison.OrdinalIgnoreCase)) launchOnly = true;
            else if (string.Equals(args[i], "-noshortcut", StringComparison.OrdinalIgnoreCase)) noShortcut = true;
            else if (string.Equals(args[i], "-dir", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length) installDir = args[++i];
        }

        if (launchOnly)
        {
            string cmdPath = Path.Combine(installDir, "start-dsh.cmd");
            if (!File.Exists(cmdPath))
            {
                Console.WriteLine("[ERROR] Not installed yet. Run the setup without '-launch' to install first.");
                Pause();
                return 1;
            }
            RunLauncher(cmdPath);
            return 0;
        }

        bool installed = File.Exists(Path.Combine(installDir, "installed.flag"));

        if (installOnly || !installed)
        {
            if (!installOnly)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("  DeepSeek Harness Launcher - Setup");
                Console.WriteLine("==============================================");
                Console.WriteLine("Install directory : " + installDir);
                Console.WriteLine("A desktop shortcut 'DeepSeek Harness' will be created.");
                Console.Write("Install now? [Y/n] ");
                string answer = Console.ReadLine();
                if (answer != null && answer.Trim().Length > 0 &&
                    !answer.Trim().StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Install cancelled.");
                    return 0;
                }
            }
            Install(installDir, noShortcut);
            Console.WriteLine("Installation complete!");
            Console.WriteLine("  Files    : " + installDir);
            Console.WriteLine("  Shortcut : DeepSeek Harness (desktop)");
        }
        else
        {
            Console.WriteLine("Already installed in: " + installDir);
        }

        if (!installOnly)
        {
            RunLauncher(Path.Combine(installDir, "start-dsh.cmd"));
        }
        return 0;
    }

    private static void Install(string dir, bool noShortcut)
    {
        Directory.CreateDirectory(dir);
        Assembly asm = Assembly.GetExecutingAssembly();
        string launcherText = ReadResource(asm, "LauncherCmd");
        launcherText = launcherText.Replace("\r\n", "\n").Replace("\n", "\r\n");
        File.WriteAllText(Path.Combine(dir, "start-dsh.cmd"), launcherText, Encoding.ASCII);
        byte[] icon = ReadResourceBytes(asm, "WhaleIcon");
        File.WriteAllBytes(Path.Combine(dir, "DeepSeekHarness-WhaleGirl.ico"), icon);
        File.WriteAllText(Path.Combine(dir, "installed.flag"),
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Encoding.UTF8);
        if (!noShortcut) CreateDesktopShortcut(dir);
    }

    private static string ReadResource(Assembly asm, string key)
    {
        return Encoding.UTF8.GetString(ReadResourceBytes(asm, key));
    }

    private static byte[] ReadResourceBytes(Assembly asm, string key)
    {
        string[] names = asm.GetManifestResourceNames();
        string full = null;
        foreach (string n in names)
        {
            if (n.EndsWith(key, StringComparison.OrdinalIgnoreCase)) { full = n; break; }
        }
        if (full == null) throw new InvalidOperationException("Missing embedded resource: " + key);
        using (Stream s = asm.GetManifestResourceStream(full))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    private static void CreateDesktopShortcut(string dir)
    {
        try
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string lnkPath = Path.Combine(desktop, "DeepSeek Harness.lnk");
            Type t = Type.GetTypeFromProgID("WScript.Shell");
            if (t == null)
            {
                Console.WriteLine("[WARN] WScript.Shell unavailable, shortcut not created.");
                return;
            }
            dynamic shell = Activator.CreateInstance(t);
            dynamic lnk = shell.CreateShortcut(lnkPath);
            lnk.TargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
            lnk.Arguments = "/d /c \"" + Path.Combine(dir, "start-dsh.cmd") + "\"";
            lnk.WorkingDirectory = dir;
            lnk.IconLocation = Path.Combine(dir, "DeepSeekHarness-WhaleGirl.ico") + ",0";
            lnk.Description = "DeepSeek Harness Launcher";
            lnk.WindowStyle = 1;
            lnk.Save();
            Console.WriteLine("Desktop shortcut created: " + lnkPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[WARN] Could not create desktop shortcut: " + ex.Message);
        }
    }

    private static void RunLauncher(string cmdPath)
    {
        Console.WriteLine("Starting DeepSeek Harness...");
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
            psi.Arguments = "/d /c \"" + cmdPath + "\"";
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ERROR] Failed to start launcher: " + ex.Message);
        }
    }

    private static void Pause()
    {
        try { Console.WriteLine("Press Enter to exit..."); Console.ReadLine(); }
        catch { }
    }
}
