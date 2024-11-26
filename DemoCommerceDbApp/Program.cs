using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ObjectEditor;
using ObjectEditor.WinForms.Forms;

namespace DemoCommerceDbApp
{
    internal static class Program
    {
        // Demo Users:
        // TenentA Admin: alice@tenanta.com, password: adminpass
        // TenentA Manager: gary@tenanta.com, password: managerpass
        // TenantB User: bob@tenantb.com, password: userpass

        private static User Login(DbSet<User> users)
        {
            var loginForm = new LoginForm()
            {
                Username = "gary@tenanta.com",
                Password = "managerpass"
            };

            while (loginForm.ShowDialog() == DialogResult.OK)
            {
                string username = loginForm.Username;
                string password = loginForm.Password;

                var user = users.FirstOrDefault(u => u.Email.ToLower() == username.ToLower() && u.Password.ToLower() == password.ToLower());
                if (user != null)
                {
                    Debug.WriteLine("Login successful.");
                    return user;
                }

                Debug.WriteLine("Login failed.");
                MessageBox.Show("Invalid username or password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("User canceled login.");
            return null;
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var dbContext = new CommerceDbContext("Data Source=CommerceDb.db");
            var user = Login(dbContext.Users);

            if (user == null)
                return;

            IDictionary<string, Permissions> userPermission = user.UserRoles.ToDictionary(ur => ur.Role.Name, ur => ur.Permissions);

            Form editorForm = new ObjectEditorForm(user.Tenant, new ObjectEditorSettings() { GroupsPermissions = userPermission });
            Application.Run(editorForm);

            dbContext.SaveChanges();
        }
    }
}