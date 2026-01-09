namespace Contextual
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

           

            ApplicationConfiguration.Initialize();

            // Initialize and show the contextual form
            ApplicationInstance.ContextualForm = new contextual();
            //ApplicationInstance.ContextualForm.Show();


            Application.Run(new frmLogin());
        }
    }
}