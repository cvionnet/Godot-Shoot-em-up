namespace BulletBallet.utils.NucleusFW.Utils;

/// <summary>
/// Class that contains all variables and methods useful between projects
/// </summary>
public sealed class Nucleus_Logs
{
    private string _gameShortName;
    private string _uniqueId; 
    
    //*-------------------------------------------------------------------------*//

    public Nucleus_Logs()
    {
        _gameShortName = ProjectSettings.GetSetting("application/config/description").ToString();
        _uniqueId = Guid.NewGuid().ToString();      // generate a unique ID to be able to follow logs of the session
        Initialize_Serilog();
        Write_StartupLog();
    }

    /// <summary>
    /// Initialize the Serilog Logger
    /// </summary>
    private void Initialize_Serilog()
    {
        // Address to Loki server
        //var credentials = new BasicAuthCredentials("https://logs-prod-us-central1.grafana.net", "71119", "eyJrIjoiOTU3OTA4OWUyZjFkNjNkMzdjNTA3MmE2MmExMzllM2EwMDk1NjkxYSIsIm4iOiJMb2tpQVBJS2V5IiwiaWQiOjUxMzI3N30=");
    }

    /// <summary>
    /// Display an information message
    /// ⚠️ ONLY if DEBUG_MODE = true
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="className">Name of the class  (GetType().Name)</param>
    /// <param name="methodName">Name of the method  (MethodBase.GetCurrentMethod().Name)</param>
    /// <param name="writeToDBLog">True = write the message in the database log system</param>
    public void Info(string message, string className = "", string methodName = "", bool writeToDBLog = true)
    {
        if (Nucleus.DEBUG_MODE)
        {
            if (className != "") className = string.Concat("[C:", className, "]");
            if (methodName != "") methodName = string.Concat("[M:", methodName, "]");
            GD.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [INF][{_gameShortName}][{_uniqueId}]{className}{methodName}{message}");
            //if (pWriteToDBLog) TODO write to Loki server
        }
    }

    /// <summary>
    /// Display a debug message
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="className">Name of the class  (GetType().Name)</param>
    /// <param name="methodName">Name of the method  (MethodBase.GetCurrentMethod().Name)</param>
    /// <param name="writeToDBLog">True = write the message in the database log system</param>
    public void Debug(string message, string className = "", string methodName = "", bool writeToDBLog = true)
    {
        if (className != "") className = string.Concat("[C:", className, "]");
        if (methodName != "") methodName = string.Concat("[M:", methodName, "]");
        GD.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [DBG][{_gameShortName}][{_uniqueId}]{className}{methodName}{message}");
        //if (pWriteToDBLog) TODO write to Loki server
    }

    /// <summary>
    /// Display a debug message using Serilog
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="exception">The exception raised</param>
    /// <param name="className">Name of the class  (GetType().Name)</param>
    /// <param name="methodName">Name of the method  (MethodBase.GetCurrentMethod().Name)</param>
    /// <param name="writeToDBLog">True = write the message in the database log system</param>
    public void Debug(string message, Exception exception, string className = "", string methodName = "", bool writeToDBLog = true)
    {
        if (className != "") className = string.Concat("[C:", className, "]");
        if (methodName != "") methodName = string.Concat("[M:", methodName, "]");
        GD.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [DBG][{_gameShortName}][{_uniqueId}]{className}{methodName}{message} - {exception}");
        //if (pWriteToDBLog) TODO write to Loki server
    }

    /// <summary>
    /// Display an error message using Serilog
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="exception">The exception raised</param>
    /// <param name="className">Name of the class  (GetType().Name)</param>
    /// <param name="methodName">Name of the method  (MethodBase.GetCurrentMethod().Name)</param>
    /// <param name="writeToDBLog">True = write the message in the database log system</param>
    public void Error(string message, Exception exception, string className = "", string methodName = "", bool writeToDBLog = true)
    {
        if (className != "") className = string.Concat("[C:", className, "]");
        if (methodName != "") methodName = string.Concat("[M:", methodName, "]");
        GD.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [ERR][{_gameShortName}][{_uniqueId}]{className}{methodName}{message} - {exception}");
        //if (pWriteToDBLog) TODO write to Loki server
    }

    /// <summary>
    /// Display a fatal error message using Serilog
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="exception">The exception raised</param>
    /// <param name="className">Name of the class  (GetType().Name)</param>
    /// <param name="methodName">Name of the method  (MethodBase.GetCurrentMethod().Name)</param>
    /// <param name="writeToDBLog">True = write the message in the database log system</param>
    public void Fatal(string message, Exception exception, string className = "", string methodName = "", bool writeToDBLog = true)
    {
        if (className != "") className = string.Concat("[C:", className, "]");
        if (methodName != "") methodName = string.Concat("[M:", methodName, "]");
        GD.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [FTL][{_gameShortName}][{_uniqueId}]{className}{methodName}{message} - {exception}");
        //if (pWriteToDBLog) TODO write to Loki server
    }

    /// <summary>
    /// Print system information at startup
    /// </summary>
    private void Write_StartupLog()
    {
        // Godot OS options : https://docs.godotengine.org/en/stable/classes/class_os.html
        Info($"Game Name : { ProjectSettings.GetSetting("application/config/name") } ({_gameShortName})");
        Info($"Debug Build Internal : { Nucleus.DEBUG_MODE.ToString().ToUpper() } / Godot : { OS.IsDebugBuild().ToString().ToUpper() }");
        Info($"Id Unique : { _uniqueId }");
        Info($"Time : { Time.GetTimeZoneFromSystem() } UTC / { Time.GetTimeStringFromSystem() } Local");

        Info($"System : { OS.GetName() }");
        Info($"CPU Number of cores : { OS.GetProcessorCount()/2 } / Memory : { (OS.GetStaticMemoryUsage() / 1024).ToString() } Go");

        Info($"Video Driver : { OS.GetVideoAdapterDriverInfo() } / Screen size { DisplayServer.ScreenGetSize() } / Game screen size { DisplayServer.WindowGetSize() }");

        Info($"Mobile Model : { OS.GetModelName() }");
        Info("---------------------------------------------------------------------------------------------", writeToDBLog: false);
    }
}
