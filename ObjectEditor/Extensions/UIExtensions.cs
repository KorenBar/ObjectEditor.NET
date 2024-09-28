using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnosoUI.Configuration.UI
{
    public static class UIExtensions
    {
        /// <summary>
        /// Invoke the user action and handle exceptions, showing a message box with the error message(s) instead of throwing exceptions.
        /// Should be used for actions triggered from the UI only, as it shows a message box as a result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="actionName">Display name of the action in the error message.</param>

        public static bool InvokeUserAction(this Action action, string actionName)
        {
            try
            {
                action?.Invoke();
                return true;
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(string.Join("\n", ex.InnerExceptions.Select(ex => ex.Message)), $"{actionName} Multiple Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"{actionName} Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// Invoke an action on the UI thread.
        /// If the action is already on the UI thread, it will be executed immediately.
        /// </summary>
        /// <param name="action">The action to be invoked on the UI thread.</param>
        public static void InvokeUI(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else action();
        }
    }
}
