using Serilog;

namespace ApiProject {
    public class LogUtils {
        public static void Startup() {
            var assembly = typeof(Program).Assembly.GetName();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"log-{DateTime.Now.ToShortDateString()}.txt", Serilog.Events.LogEventLevel.Information)
                .Enrich.WithProperty("AppName", assembly.Name)
                .Enrich.WithProperty("AppFullName", assembly.FullName)
                .Enrich.WithProperty("Codebase", assembly.CodeBase)
                .Enrich.WithProperty("AppName", assembly.ContentType)
                .Enrich.WithProperty("AppName", assembly.Version)
                .Enrich.WithProperty("AppName", assembly.VersionCompatibility)
                .Enrich.WithProperty("AppName", Environment.UserName)
                .CreateLogger();
        }
    }
}
