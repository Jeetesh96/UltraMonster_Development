using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.NativeAndroidDebugger.Editor
{
    /*
    * This script is the Dataset of the scriptable object "Preferences". This script saves Native Android Debugger preferences.
    */

    public class AndroidDebuggerPreferences : ScriptableObject
    {
        public string projectName;
        public Rect windowPosition;

        public bool passedTheWelcomeScreen = false;
        public int currentTab = 0;
        public bool dontShowWarningBeforeConnect = false;
        public int ip0 = 127;
        public int ip1 = 0;
        public int ip2 = 0;
        public int ip3 = 1;
        public bool autoUpdateListOfConnectedDevices = true;
        public string sdkPath;
    }
}