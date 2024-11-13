using ObjectEditor.Demo.Data;
using System.Diagnostics;
using ObjectEditor.WinForms.Forms;

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
            var form = new ObjectEditorForm(data);
            form.Controller.ValueChanged += (s, e) => Debug.WriteLine(e.ToString() + (e.ByUser ? " (User)" : ""));
            form.Controller.SaveRequiredChanged += (s, e) => Debug.WriteLine("Save required: " + e.SaveRequired);
            Application.Run(form);
        }
    }
}