using TechnosoCommons.Configuration.UI.Forms;
using ObjectEditor.Demo.Data;

namespace ObjectEditor.Demo
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
            var data = new MainDataClass();
            var form = ObjectEditorFactory.CreateForm(data);
            Application.Run(form);
        }
    }
}