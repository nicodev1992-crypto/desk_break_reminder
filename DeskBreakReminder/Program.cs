namespace DeskBreakReminder
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            SQLitePCL.Batteries_V2.Init();
            ApplicationConfiguration.Initialize();

            var repository = new SqliteBreakRepository();
            await repository.InitializeAsync();

            var scheduler = new BreakSchedulerService(repository);
            var trayContext = new TrayApplicationContext(scheduler);

            Application.Run(trayContext);
        }
    }
}