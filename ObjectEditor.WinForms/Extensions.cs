using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.WinForms
{
    internal static class Extensions
    {
        /// <summary>
        /// Invoke the user action and handle exceptions, showing a message box with the error message(s) instead of throwing exceptions.
        /// Should be used for actions triggered from the UI only, as it shows a message box as a result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="actionName">Display name of the action in the error message.</param>
        public static bool InvokeUserAction(this Action action, string actionName)
        {
            string msg = null;
            string caption = $"{actionName} Error";

            try
            {
                action?.Invoke();
                return true;
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                msg = ex.InnerException.Message;
            }
            catch (AggregateException ex)
            {
                msg = string.Join("\n", ex.InnerExceptions.Select(ex => ex.Message));
                caption = $"{actionName} Multiple Errors";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        /// <summary>
        /// Invoke the user action asynchronously and handle exceptions, showing a message box with the error message(s) instead of throwing exceptions.
        /// Should be used for actions triggered from the UI only, as it shows a message box as a result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="actionName">Display name of the action in the error message.</param>
        /// <returns></returns>
        public static Task<bool> InvokeUserActionAsync(this Action action, string actionName)
        {
            return Task.Run(() => InvokeUserAction(action, actionName));
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
