using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Common
{
    public static class Notifications
    {
        private static bool _isInitialized;
        public static ObservableCollection<ConsoleNotif> consoleNotifs;
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                consoleNotifs = new ObservableCollection<ConsoleNotif>();
                _isInitialized = true;
            }
        }
    }
}
