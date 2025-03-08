namespace ThePalace.Server.Desktop;

public partial class Program : Form
{
    public Program()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Program());
    }
}