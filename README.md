Desk Break Reminder

A small C# / .NET desktop utility that lives in the system tray and reminds you to take regular breaks while working at a computer: eye rest breaks and stand-up/movement breaks.

What it does
Runs quietly in the system tray, no main window
Two independent timers: one for eye breaks (default: every 20 minutes), one for movement breaks (default: every 60 minutes)
Shows a system notification (balloon tip) when it's time for a break
Logs each break to a local SQLite database, so you can track how consistent you've been during the day
Tech stack
Language: C# / .NET
UI: WinForms (system tray application via NotifyIcon and ApplicationContext)
Database: SQLite (Microsoft.Data.Sqlite)
Architecture

The project is organized into three layers with a clear separation of concerns:

Data layer (IBreakRepository, SqliteBreakRepository) — handles reading and writing break records to SQLite. Depends only on the IBreakRepository interface, not on SQLite directly, elsewhere in the app.
Business layer (BreakSchedulerService) — owns the scheduling logic (when a break is due) and exposes a breakTimeReached event. Knows nothing about how breaks are displayed or stored — it just signals "it's time."
Presentation layer (TrayApplicationContext) — owns the tray icon and notifications. Listens to the scheduler's event and decides how to show it to the user.

Dependencies are wired together via constructor injection in Program.cs:

SqliteBreakRepository -> BreakSchedulerService -> TrayApplicationContext
Quickstart
Clone the repository
Open the solution in Visual Studio
Restore NuGet packages (Microsoft.Data.Sqlite, SQLitePCLRaw.bundle_e_sqlite3)
Run the project — the app will appear as an icon in the system tray
Notes

This is a personal utility project built to practice layered architecture and dependency injection in .NET. It's intentionally simple: no automated tests yet, break intervals are currently hardcoded rather than user-configurable, and error handling is minimal. These would be the natural next steps to make it more robust.
