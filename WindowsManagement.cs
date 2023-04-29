using Microsoft.Win32;
using System.Security.Principal;

namespace WindowsUpdater
{
    public static class WindowsManagement
    {
        public static void DisableWindowsUpdate()
        {
            if (IsAdministrator())
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU",
                    "NoAutoUpdate", 1);
            }
        }

        public static void EnableWindowsUpdate()
        {
            if (IsAdministrator())
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU",
                    "NoAutoUpdate", 0);
            }
        }

        public static bool IsWindowsUpdateDisabled()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", false);

                var value = key?.GetValue("NoAutoUpdate");

                if (value != null && (int)value == 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public static bool IsAdministrator()
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();

            var principal = new WindowsPrincipal(windowsIdentity);

            var isAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdministrator)
            {
                Console.WriteLine("The application is not running as administrator. Unable to disable Windows Update.");
            }

            return isAdministrator;
        }
    }
}