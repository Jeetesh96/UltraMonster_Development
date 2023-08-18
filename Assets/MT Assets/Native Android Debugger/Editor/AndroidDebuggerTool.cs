using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MTAssets.NativeAndroidDebugger.Editor
{
    public class AndroidDebuggerTool : EditorWindow
    {
        /*
          This class is responsible for the functioning of the "Native Android Debugger" component, and all its functions.
        */
        /*
         * The Native Android Debugger was developed by Marcos Tomaz in 2021.
         * Need help? Contact me (mtassets@windsoft.xyz)
         */

        //Private variables of window
        private static AndroidDebuggerPreferences androidDebuggerPreferences;
        private bool preferencesLoadedOnInspectorUpdate = false;
        private bool isWindowOnFocus = false;
        private TimeSpan lastTimeOfProccessCheck = TimeSpan.Zero;
        private bool isProccessRunning = false;
        private Ping devicePingObject = null;
        private TimeSpan lastDevicePingStarted = TimeSpan.Zero;
        private int lastDevicePingTime = -1;
        private TimeSpan lastAutoCheckOfConnectedDevices = TimeSpan.Zero;

        //Classes of script
        private class ConnectedDevice
        {
            //This class stores data about a connected device
            public ConnectedOver connectedOver = ConnectedOver.None;
            public string deviceName;
            public ConnectedStatus deviceStatus = ConnectedStatus.Offline;
        }

        //Enums of script
        private enum ConnectedOver
        {
            None,
            Usb,
            WiFi
        }
        private enum ConnectedStatus
        {
            Offline,
            Online,
            Device
        }
        private enum CommandPreSet
        {
            Pick,
            usb,
            tcpip5555,
            reboot,
            devices_l,
            logcat_c,
            logcat_d,
            uninstall_package,
            uninstall_k_package,
            shell_pm_clear_package,
            exec_out_screencap_p,
            version,
            disconnect,
            connect,
        }

        //Private variables of UI
        private Vector2 verticalAdaptativeScrollViewPos = Vector2.zero;
        private Vector2 commandsLogsScrollViewPos = Vector2.zero;
        private Vector2 connectedDevicesScrollViewPos = Vector2.zero;
        private Vector2 commandsTerminalLogsScrollViewPos = Vector2.zero;
        private bool[] helpfulTopicsExpanded = new bool[7];
        private string currentQuickCommandsLog = "";
        private int currentQuickCommandsLog_lastCharsLenght = 0;
        private string currentTerminalCommandsLog = "----= Native Android Debugger Terminal =----";
        private int currentTerminalCommandsLog_lastCharsLenght = 0;
        private CommandPreSet adbCommandToRunInTerminalPreSet = CommandPreSet.Pick;
        private string adbCommandToRunInTerminal = "";
        private List<string> lastRunnedCommands = new List<string>();
        private List<ConnectedDevice> listOfCurrentlyConnectedDevices = new List<ConnectedDevice>();

        public static void OpenWindow()
        {
            //Method to open the Window
            var window = GetWindow<AndroidDebuggerTool>("Debugger Tool");
            window.minSize = new Vector2(485, 655);
            var position = window.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            window.position = position;
            window.Show();
        }

        //UI Code

        #region INTERFACE_CODE
        void OnEnable()
        {
            //On enable this window, on re-start this window after compilation
            isWindowOnFocus = true;

            //Load the preferences
            LoadThePreferences(this);
        }

        void OnDisable()
        {
            //On disable this window, after compilation, disables the window and enable again
            isWindowOnFocus = false;

            //Save the preferences
            EditorApplication.delayCall += () => { SaveThePreferences(this); }; //<-- Avoid log erros on enter play mode with window opened
            //SaveThePreferences(this);
        }

        void OnDestroy()
        {
            //On close this window
            isWindowOnFocus = false;

            //Save the preferences
            EditorApplication.delayCall += () => { SaveThePreferences(this); }; //<-- Avoid log erros on enter play mode with window opened
            //SaveThePreferences(this);
        }

        void OnFocus()
        {
            //On focus this window
            isWindowOnFocus = true;
        }

        void OnLostFocus()
        {
            //On lose focus in window
            isWindowOnFocus = false;
        }

        void OnGUI()
        {
            //Start the undo event support, draw default inspector and monitor of changes 
            EditorGUI.BeginChangeCheck();

            //Try to load needed assets
            Texture iconOfUi = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Icon.png", typeof(Texture));
            Texture processRunning = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Running.png", typeof(Texture));
            Texture processNotRunning = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/NotRunning.png", typeof(Texture));
            Texture pingOn = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/PingOn.png", typeof(Texture));
            Texture pingOff = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/PingOff.png", typeof(Texture));
            Texture usb = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/USB.png", typeof(Texture));
            Texture wifiOn = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/WifiOn.png", typeof(Texture));
            Texture wifiOff = (Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/WifiOff.png", typeof(Texture));
            //If fails on load needed assets, locks ui
            if (iconOfUi == null || processRunning == null || processNotRunning == null || pingOn == null || pingOff == null)
            {
                EditorGUILayout.HelpBox("Unable to load required files. Please reinstall Native Android Debugger to correct this problem.", MessageType.Error);
                return;
            }
            //If this tool is running in other platform, that is not Windows, stop
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                EditorGUILayout.HelpBox("Sorry, but at the moment the Native Android Debugger is not supported on platforms other than Unity Editor for Windows.", MessageType.Error);
                return;
            }
            //If user is not passed the welcome screen yet, show it and cancel run of program
            if (androidDebuggerPreferences.passedTheWelcomeScreen == false)
            {
                UI_WelcomeScreen();
                return;
            }

            //Render the TopBar of UI
            UI_TopBar(iconOfUi);

            GUILayout.Space(10);

            //Show the toolbar
            androidDebuggerPreferences.currentTab = GUILayout.Toolbar(androidDebuggerPreferences.currentTab, new string[] { "Quick Debug", "ADB Terminal", "Device Settings", "SDK Settings" });

            GUILayout.Space(10);

            //Start vertical adaptative scroll view
            verticalAdaptativeScrollViewPos = EditorGUILayout.BeginScrollView(verticalAdaptativeScrollViewPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 234));

            //Draw the content of toolbar selected
            switch (androidDebuggerPreferences.currentTab)
            {
                case 0:
                    UI_QuickDebug(usb, wifiOn, wifiOff);
                    break;
                case 1:
                    UI_ADBTerminal();
                    break;
                case 2:
                    UI_DeviceSettings();
                    break;
                case 3:
                    UI_SDKSettings();
                    break;
            }

            //End vertical adaptative scroll view
            EditorGUILayout.EndScrollView();

            //Show the status bar
            UI_StatusBar(processRunning, processNotRunning, pingOn, pingOff);

            //Show the help remember
            GUILayout.Space(6);
            EditorGUILayout.HelpBox("Remember to read the Native Android Debugger documentation to understand how to use it.\nGet support at: mtassets@windsoft.xyz", MessageType.None);

            //Apply changes on script, case is not playing in editor
            if (GUI.changed == true && Application.isPlaying == false)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            if (EditorGUI.EndChangeCheck() == true)
            {

            }
        }

        void UI_WelcomeScreen()
        {
            //Draw the welcome screen
            GUILayout.FlexibleSpace();

            GUIStyle titulo = new GUIStyle();
            titulo.fontSize = 25;
            titulo.normal.textColor = Color.black;
            titulo.alignment = TextAnchor.LowerCenter;
            titulo.wordWrap = true;
            EditorGUILayout.LabelField("Welcome to Native Android Debugger!", titulo);
            GUILayout.Space(40);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle text = new GUIStyle();
            text.fontSize = 12;
            text.alignment = TextAnchor.MiddleLeft;
            text.wordWrap = true;
            EditorGUILayout.LabelField("Native Android Debugger is a tool that aims to give greater control of your Unity Editor's ADB, in a simple, easy and useful way. With Native Android debugger, you can do some things like...\n\n" +
                                        "- Connect your device for debugging via Wi-Fi (no USB cable needed) and then you can send your game's Build to your cell phone, without using a USB cable.\n\n" +
                                        "- Check things and debug your Android device.\n\n" +
                                        "- Control more effectively your Unity Editor ADB.\n\n" +
                                        "- Run commands directly in your Unity Editor's ADB through a handy Terminal.\n\n" +
                                        "- Check how many devices are currently connected to your ADB dp Unity Editor.\n\n" +
                                        "- Monitor whenever you want, the connection between your computer and device."
                                        , text, GUILayout.Width(455));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(40);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Let's start!", GUILayout.Width(160), GUILayout.Height(24)))
                androidDebuggerPreferences.passedTheWelcomeScreen = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }

        void UI_TopBar(Texture iconOfUi)
        {
            GUIStyle estiloIcone = new GUIStyle();
            estiloIcone.border = new RectOffset(0, 0, 0, 0);
            estiloIcone.margin = new RectOffset(4, 0, 4, 0);

            //Topbar
            GUILayout.BeginHorizontal("box");
            GUILayout.Space(4);
            GUILayout.BeginVertical(GUILayout.Height(44), GUILayout.Width(48));
            GUILayout.Space(6);
            GUILayout.Box(iconOfUi, estiloIcone, GUILayout.Width(48), GUILayout.Height(44));
            GUILayout.Space(6);
            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUIStyle titulo = new GUIStyle();
            titulo.fontSize = 25;
            titulo.normal.textColor = Color.black;
            titulo.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("Native Android Debugger", titulo);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        bool isDeviceSettingsValid()
        {
            //Prepare the response
            bool isValid = true;

            //If IP is invalid, return
            if (androidDebuggerPreferences.ip0 < 0 || androidDebuggerPreferences.ip0 > 255)
                isValid = false;
            if (androidDebuggerPreferences.ip1 < 0 || androidDebuggerPreferences.ip1 > 255)
                isValid = false;
            if (androidDebuggerPreferences.ip2 < 0 || androidDebuggerPreferences.ip2 > 255)
                isValid = false;
            if (androidDebuggerPreferences.ip3 < 0 || androidDebuggerPreferences.ip3 > 255)
                isValid = false;
            if (androidDebuggerPreferences.ip0 == 127 && androidDebuggerPreferences.ip1 == 0 && androidDebuggerPreferences.ip2 == 0 && androidDebuggerPreferences.ip3 == 1)
                isValid = false;

            //Return the response
            return isValid;
        }

        bool isPhoneSdkSettingsValid()
        {
            //Prepare the response
            bool isValid = true;

            //If IP is invalid, return
            if (string.IsNullOrEmpty(androidDebuggerPreferences.sdkPath) == true)
                isValid = false;
            if (isValid == true)
                if (File.Exists(androidDebuggerPreferences.sdkPath + "/platform-tools/adb.exe") == false)
                    isValid = false;

            //Return the response
            return isValid;
        }

        string getSdkPathFromUnity()
        {
            //Return the path
            return (EditorApplication.applicationPath.Replace("/Unity.exe", "") + "/Data/PlaybackEngines/AndroidPlayer/SDK");
        }

        void UI_QuickDebug(Texture usb, Texture wifiOn, Texture wifiOff)
        {
            //If settings is not valid, return
            if (isDeviceSettingsValid() == false)
            {
                EditorGUILayout.HelpBox("There is a problem with your Device Settings. Please resolve the issues to be able to use this function of Native Android Debugger.", MessageType.Warning);
                if (GUILayout.Button("Go To Settings", GUILayout.Height(24)))
                    androidDebuggerPreferences.currentTab = 2;
                GUILayout.Space(10);
            }
            if (isPhoneSdkSettingsValid() == false)
            {
                EditorGUILayout.HelpBox("There is a problem with your SDK Settings. Please resolve the issues to be able to use this function of Native Android Debugger.", MessageType.Warning);
                if (GUILayout.Button("Go To Settings", GUILayout.Height(24)))
                    androidDebuggerPreferences.currentTab = 3;
                GUILayout.Space(10);
            }
            //If someone is invalid, return
            if (isDeviceSettingsValid() == false || isPhoneSdkSettingsValid() == false)
                return;

            //If the ADB is stopped, cancel
            if (isProccessRunning == false)
            {
                GUILayout.Space(20);
                GUIStyle title = new GUIStyle();
                title.fontStyle = FontStyle.Bold;
                title.alignment = TextAnchor.MiddleCenter;
                EditorGUILayout.LabelField("ADB Server Initialization", title);
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("The ADB of Android SDK that you provided is currently not running. For this interface to be available, it is first necessary to start the ADB Server of Android SDK which is currently configured in the Native Android Debugger.\n\nAlso make sure there is no other program or version of Unity Engine running another copy of ADB on your computer. If this is happening, it may not be possible to start this ADB server.", MessageType.Warning);
                GUILayout.Space(10);
                if (GUILayout.Button("Start Server Of Current ADB", GUILayout.Height(24)))
                {
                    RunCommandsInWindowsCmd("Start Server Of Current ADB", new string[] { "adb start-server", "adb devices" });
                    lastTimeOfProccessCheck = TimeSpan.Zero;
                }
                return;
            }

            //Update list of connected devices
            UpdateListOfAllConnectedDevices();

            GUILayout.Space(20);

            //Render the Quick Debug UI
            GUIStyle tituloBox = new GUIStyle();
            tituloBox.fontStyle = FontStyle.Bold;
            tituloBox.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Connect or Disconnect device", tituloBox);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            //Connect
            if (GUILayout.Button("Connect To Device", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                if (androidDebuggerPreferences.dontShowWarningBeforeConnect == false)
                {
                    //Show warning
                    int response = EditorUtility.DisplayDialogComplex("Checklist Before Connect",
                                                                      "Native Android Debugger will command ADB to connect to your device over Wi-Fi. The IP of the device ADB will try to connect to, is the IP provided in \"Device Settings\" in Native Android Debugger.\n\nFollow the checklist below, if everything is OK, proceed with the connection.\n\n" +
                                                                      "1. Make sure your device is connected to the same local network (router) that your computer is connected to.\n\n" +
                                                                      "2. Make sure the IP provided in \"Device Settings\" of Native Android Debugger is the same IP your device is using.\n\n" +
                                                                      "3. If this is going to be the first time in this session that your device will be connected to ADB over Wi-Fi, make sure it is connected with USB.\n\n" +
                                                                      "4. Make sure your device \"Developer Mode\" is active.\n\n" +
                                                                      "5. Make sure that \"USB Debugging\" in your device \"Developer Mode\" settings is enabled.\n\n" +
                                                                      "6. If a Alert Dialog appears on your device, asking for Debugging Permission for your Computer, remember to accept it.\n\n" +
                                                                      "Once the Connect command has been completed, you can check if your device has been listed as connected in the list of connected devices. If it is connected, you can unplug it from the USB, as it will already be connected to ADB via Wi-Fi."
                                                                      , "Continue", "Cancel", "Don't Show Again");
                    //If "Cancel"
                    if (response == 1)
                        return;
                    //If "Don't Show Again"
                    if (response == 2)
                        androidDebuggerPreferences.dontShowWarningBeforeConnect = true;
                }

                string deviceIp = androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3;
                RunCommandsInWindowsCmd("Connect To Device", new string[] { "adb tcpip 5555", "adb connect " + deviceIp, });
                lastTimeOfProccessCheck = TimeSpan.Zero;
                lastAutoCheckOfConnectedDevices = TimeSpan.Zero;
            }
            GUILayout.FlexibleSpace();
            //Disconnect from
            if (GUILayout.Button("Disconnect From Device", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                string deviceIp = androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3;
                RunCommandsInWindowsCmd("Disconnect From Device", new string[] { "adb disconnect " + deviceIp });
                lastTimeOfProccessCheck = TimeSpan.Zero;
                lastAutoCheckOfConnectedDevices = TimeSpan.Zero;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //List
            if (GUILayout.Button("List Connecteds Devices", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                RunCommandsInWindowsCmd("List Connecteds Devices", new string[] { "adb devices" });
                lastTimeOfProccessCheck = TimeSpan.Zero;
                lastAutoCheckOfConnectedDevices = TimeSpan.Zero;
            }
            GUILayout.FlexibleSpace();
            //Disconnect all
            if (GUILayout.Button("Disconnect All Devices", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                RunCommandsInWindowsCmd("Disconnect All Devices", new string[] { "adb disconnect" });
                lastTimeOfProccessCheck = TimeSpan.Zero;
                lastAutoCheckOfConnectedDevices = TimeSpan.Zero;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Device DOZE System Debug", tituloBox);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            //Check doze
            if (GUILayout.Button("Check DOZE Current Stats", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                RunCommandsInWindowsCmd("Check DOZE Current Stats", new string[] { "echo Status Light Mode do DOZE" + " & " + "adb shell dumpsys deviceidle get light" + " & " + "echo Status Deep Mode do DOZE" + " & " + "adb shell dumpsys deviceidle get deep" });
                lastTimeOfProccessCheck = TimeSpan.Zero;
            }
            GUILayout.FlexibleSpace();
            //Advance doze
            if (GUILayout.Button("Advance DOZE to Next Level", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                RunCommandsInWindowsCmd("Check DOZE Current Stats", new string[] { "adb shell dumpsys deviceidle step light" + " & " + "adb shell dumpsys deviceidle step deep" });
                lastTimeOfProccessCheck = TimeSpan.Zero;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("ADB Server And Unity Management", tituloBox);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            //Stop server
            if (GUILayout.Button("Stop Server of Current ADB", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                RunCommandsInWindowsCmd("Stop Server of Current ADB", new string[] { "adb disconnect", "adb kill-server" });
                lastTimeOfProccessCheck = TimeSpan.Zero;
            }
            GUILayout.FlexibleSpace();
            //Unity build
            if (GUILayout.Button("Open Unity Build Settings", GUILayout.Height(24), GUILayout.Width(Screen.width / 2.0f - 12)))
            {
                EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
                lastTimeOfProccessCheck = TimeSpan.Zero;
            }
            EditorGUILayout.EndHorizontal();

            //Rnder the console
            GUILayout.Space(30);
            EditorGUILayout.LabelField("Commands Logs", tituloBox);
            GUILayout.Space(10);
            if (currentQuickCommandsLog.Length != currentQuickCommandsLog_lastCharsLenght)
            {
                commandsLogsScrollViewPos.y += 9999999;
                currentQuickCommandsLog_lastCharsLenght = currentQuickCommandsLog.Length;
            }
            EditorGUILayout.BeginVertical("box", GUILayout.Height(120));
            GUILayout.FlexibleSpace();
            commandsLogsScrollViewPos = EditorGUILayout.BeginScrollView(commandsLogsScrollViewPos, GUILayout.Height(120));
            GUIStyle logsBox = new GUIStyle();
            logsBox.alignment = TextAnchor.UpperLeft;
            logsBox.wordWrap = true;
            EditorGUILayout.LabelField(((string.IsNullOrEmpty(currentQuickCommandsLog) == true) ? "The Commands Log is empty. Run the Command \"Connect To Device\" above, to start." : currentQuickCommandsLog), logsBox);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            //Rnder the list of connected devices
            GUILayout.Space(30);
            EditorGUILayout.LabelField("Currently Connected Devices (" + listOfCurrentlyConnectedDevices.Count + ")", tituloBox);
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box", GUILayout.Height(60));
            GUILayout.FlexibleSpace();
            connectedDevicesScrollViewPos = EditorGUILayout.BeginScrollView(connectedDevicesScrollViewPos, GUILayout.Height(60));
            //If auto refresh is enabled, show the content
            if (androidDebuggerPreferences.autoUpdateListOfConnectedDevices == true)
            {
                //If not have connecteds
                if (listOfCurrentlyConnectedDevices.Count == 0)
                {
                    GUIStyle notification = new GUIStyle();
                    notification.alignment = TextAnchor.MiddleCenter;
                    notification.fontStyle = FontStyle.Italic;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("Your ADB connected devices will appear here!", notification);
                    GUILayout.FlexibleSpace();
                }
                //Prepare the formatation
                GUIStyle estiloConnectedIcon = new GUIStyle();
                estiloConnectedIcon.border = new RectOffset(0, 0, 0, 0);
                estiloConnectedIcon.margin = new RectOffset(4, 0, 4, 0);
                GUIStyle connectedTextLeft = new GUIStyle();
                connectedTextLeft.alignment = TextAnchor.MiddleLeft;
                GUIStyle connectedTextRight = new GUIStyle();
                connectedTextRight.alignment = TextAnchor.MiddleRight;
                //Render all devices connected
                foreach (ConnectedDevice device in listOfCurrentlyConnectedDevices)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(14));
                    GUILayout.Space(5);
                    if (device.connectedOver == ConnectedOver.Usb)
                        GUILayout.Box(usb, estiloConnectedIcon, GUILayout.Width(12), GUILayout.Height(12));
                    if (device.connectedOver == ConnectedOver.WiFi)
                    {
                        if (device.deviceStatus == ConnectedStatus.Online)
                            GUILayout.Box(wifiOn, estiloConnectedIcon, GUILayout.Width(12), GUILayout.Height(12));
                        if (device.deviceStatus == ConnectedStatus.Offline)
                            GUILayout.Box(wifiOff, estiloConnectedIcon, GUILayout.Width(12), GUILayout.Height(12));
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Device " + device.deviceName, connectedTextLeft);
                    if (device.deviceStatus == ConnectedStatus.Device || device.deviceStatus == ConnectedStatus.Online)
                        EditorGUILayout.LabelField("Online", connectedTextRight);
                    if (device.deviceStatus == ConnectedStatus.Offline)
                        EditorGUILayout.LabelField("Offline", connectedTextRight);
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                }
            }
            //If auto refresh is disabled, show a warning
            if (androidDebuggerPreferences.autoUpdateListOfConnectedDevices == false)
            {
                GUIStyle notification = new GUIStyle();
                notification.alignment = TextAnchor.MiddleCenter;
                notification.fontStyle = FontStyle.Italic;
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Automatic update was disabled!", notification);
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Enable Auto Update", GUILayout.Height(18), GUILayout.Width(140)))
                {
                    androidDebuggerPreferences.autoUpdateListOfConnectedDevices = true;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            GUILayout.Space(30);
            EditorGUILayout.LabelField("Helpful Topics", tituloBox);
            GUILayout.Space(10);
            GUIStyle topicStyle = new GUIStyle();
            topicStyle.fontStyle = FontStyle.Bold;
            topicStyle.alignment = TextAnchor.MiddleLeft;

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("What is the function of each of the Buttons (Commands) above?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[5] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[5] = !helpfulTopicsExpanded[5];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[5] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Each of the buttons above represents a command you can run for the ADB from the SDK you provided. With the buttons above, you can connect your device to your computer via Wi-Fi, disconnect it, check the list of devices currently connected to your computer and so on. For example, after connecting your device to ADB via Wi-Fi, it will appear here in Unity Editor even if it is no longer connected via USB, so you can send your Builds directly to your cell phone, without the need for a USB cable! Cool, huh?", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("After clicking one of the above buttons, Native Android Debugger will automatically communicate to ADB the command you chose and then it will be executed. You can see what ADB returned in response below under \"Commands Logs\".", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Now, see below for a list that describes exactly what each of the above commands does.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Connect To Device", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Connect your device to the ADB via Wi-Fi, at the IP informed in the \"Device Settings\" of Native Android Debugger. Note that, if it is the first time you are going to connect your device to ADB via Wi-Fi, you need to have your phone connected via USB and with Debug Mode enabled.\n\nThen just connect your device using this command, then you can always connect it without a cable, but if you reboot your device or computer, you will need to perform this procedure again.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("List Connecteds Devices", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("List all devices currently connected to the ADB of your Android SDK. Devices listed with the \"device\" suffix are currently connected successfully, in your ADB. Devices with the \"offline\" suffix are not communicating with the ADB correctly, or are disconnected. If your device is connected or should be connecting, but the suffix \"offline\" still appears, see the help topic below.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Disconnect From Device", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Disconnect the device currently connected to the informed IP in \"Device Settings\" of Native Android Debugger, to which it is connected to this ADB.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Disconnect All Devices", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Disconnect all devices currently connected to this ADB except those connected via USB.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Check DOZE Current Stats", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("DOZE is your Android device's energy saving system. DOZE saves power for your device after it has gone for a long period without user interaction. DOZE has several optimization levels, the highest level being Deep Sleep, which disables several useless processes, reduces the number of Broadcasts, or schedules, according to priority, etc. The device only comes out of Deep Sleep when the user interacts with the device or when an app purposely wakes him up to do some necessary task. You can use ADB DOZE commands to debug how your app reacts in each DOZE optimization level for example.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("This command displays the current stats, that is, the current step (level) that the device's DOZE is at. Below are the steps of the DOZE.\n\nACTIVE - Device is active and in use.\n\nINACTIVE - Device has just been out of use recently.\n\nIDLE_PENDING - Device will go to sleep soon, it may take from 30 minutes to 1 hour. Normally the device cannot go into IDLE if there is a task or alarm scheduled to run in 1 hour or less.\n\nSENSING - Device is feeling it can go to sleep or not.\n\nLOCATING - Device is using location to perform checks.\n\nIDLE - Device is in deep sleep.\n\nIDLE_MAINTENANCE - Device is in deep sleep, but has woken up to run some scheduled task by some application and will soon be back to IDLE.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Advance DOZE To Next Level", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Force the device to advance the current step of DOZE to the next step. Your device must be out of use and locked (screen off) for this command to work.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Stop Server Of Current ADB", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Shut down and kill the ADB server from the current Android SDK.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Open Unity Build Settings", topicStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Opens the Unity Editor Build window so you can build your application and upload/install it to your Android device automatically. If your device is connected here to the ADB (over Wi-Fi), it will appear in the list of devices where you can submit the build, automatically.", textStyle);
            }
            GUILayout.EndVertical();

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("My device is being listed as \"Offline\" how can I fix it?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[6] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[6] = !helpfulTopicsExpanded[6];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[6] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Sometimes when your device is listed as \"offline\", even in situations where it should be connected normally, it can mean that the device has been inactive because of a long time without receiving commands from ADB, or it has disconnected from ADB or there is a problem that has prevented your device from maintaining or connecting to the ADB via Wi-Fi.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If you've already checked, followed all necessary steps correctly before connecting your device to ADB, and it clearly should be listed as \"device\" but is being listed as \"offline\", try following the steps below to fix this issue.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("In the walkthrough below, we will assume that your device's \"Developer Mode\" and \"USB Debugging\" settings are enabled normally, and your computer has been allowed by your device to debug it.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("1. Make sure that \"Aggressive Wi-Fi to Cellular handover\" under Networking section in the device's Developer Options is turned off.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("2. To make sure your device's Networking system is not in Idle Mode, see if the Native Android Debugger can ping it. Just observe if the \"Device Ping\" below is exhibiting latency. If the \"Device Ping\" is not showing latency (for the IP you set in \"Device Settings\") check if your device is properly connected to the Wi-Fi network, try reboot it.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("3. Connect your device to USB.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("4. Run the \"usb\" command in \"ADB Terminal\" of Native Android Debugger. This command will reset the ADB USB connection module.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("5. Run the \"tcpip 5555\" command in \"ADB Terminal\" of Native Android Debugger. This command will reset the ADB TCP/IP connection module.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("6. Try connecting to your device normally!", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("7. In case it's still not connected, try to switch the usb connection mode as MTP/PTP/Camera while the device is connected through USB and repeat these steps over again...", textStyle);
                GUILayout.Space(20);
                EditorGUILayout.LabelField("HINT: If all of the above tips and steps fail, it could mean that the device just needs to be \"wake up\" to receive APKs over Wi-Fi again. If you've done all of the above steps and your device still shows up as \"Offline\" on Wi-Fi, connect your device via USB and upload a first build to it via USB. After sending this first compilation over USB, unplug it from the USB and try connecting it normally over Wi-Fi again.", textStyle);
            }
            GUILayout.EndVertical();
        }

        void UI_ADBTerminal()
        {
            //If settings is not valid, return
            if (isDeviceSettingsValid() == false)
            {
                EditorGUILayout.HelpBox("There is a problem with your Device Settings. Please resolve the issues to be able to use this function of Native Android Debugger.", MessageType.Warning);
                if (GUILayout.Button("Go To Settings", GUILayout.Height(24)))
                    androidDebuggerPreferences.currentTab = 2;
                GUILayout.Space(10);
            }
            if (isPhoneSdkSettingsValid() == false)
            {
                EditorGUILayout.HelpBox("There is a problem with your SDK Settings. Please resolve the issues to be able to use this function of Native Android Debugger.", MessageType.Warning);
                if (GUILayout.Button("Go To Settings", GUILayout.Height(24)))
                    androidDebuggerPreferences.currentTab = 3;
                GUILayout.Space(10);
            }
            //If someone is invalid, return
            if (isDeviceSettingsValid() == false || isPhoneSdkSettingsValid() == false)
                return;

            GUILayout.Space(20);

            //Render the Quick Debug UI
            GUIStyle tituloBox = new GUIStyle();
            tituloBox.fontStyle = FontStyle.Bold;
            tituloBox.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Mini ADB Terminal", tituloBox);

            GUILayout.Space(10);

            //Rnder the console
            if (currentTerminalCommandsLog.Length != currentTerminalCommandsLog_lastCharsLenght)
            {
                commandsTerminalLogsScrollViewPos.y += 9999999;
                currentTerminalCommandsLog_lastCharsLenght = currentTerminalCommandsLog.Length;
            }
            EditorGUILayout.BeginVertical("box", GUILayout.Height(400));
            GUILayout.FlexibleSpace();
            commandsTerminalLogsScrollViewPos = EditorGUILayout.BeginScrollView(commandsTerminalLogsScrollViewPos, GUILayout.Height(400));
            GUIStyle logsBox = new GUIStyle();
            logsBox.alignment = TextAnchor.UpperLeft;
            logsBox.wordWrap = true;
            EditorGUILayout.LabelField(((string.IsNullOrEmpty(currentTerminalCommandsLog) == true) ? "The Terminal Log is empty. Run a Command below, to start." : currentTerminalCommandsLog), logsBox);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Type below any ADB command you want to use. You don't need to type the \"adb\" prefix here in this Terminal, as the \"adb\" prefix is automatically inserted into your command.\nThe ADB that will be used to run your Command will be the same ADB that was registered in the Native Android Debugger.", MessageType.Info);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            adbCommandToRunInTerminalPreSet = (CommandPreSet)EditorGUILayout.EnumPopup(new GUIContent("",
                        "Choose a command from the predefined commands..."),
                        adbCommandToRunInTerminalPreSet, GUILayout.Width(45));
            if (adbCommandToRunInTerminalPreSet != CommandPreSet.Pick)
                adbCommandToRunInTerminal = GetCommandPickedFromPresets();
            GUILayout.Space(12);
            GUIStyle preCommand = new GUIStyle();
            preCommand.fontStyle = FontStyle.Bold;
            preCommand.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("adb", preCommand, GUILayout.Width(25));
            adbCommandToRunInTerminal = EditorGUILayout.TextField(new GUIContent("",
                  "Type here the Command to run."),
                  adbCommandToRunInTerminal);
            GUILayout.Space(16);
            if (GUILayout.Button("Run", GUILayout.Height(18), GUILayout.Width(50)))
            {
                if (string.IsNullOrEmpty(adbCommandToRunInTerminal) == false)
                {
                    //Add to last commands runned
                    if (lastRunnedCommands.Count >= 3)
                    {
                        lastRunnedCommands.RemoveAt(0);
                        lastRunnedCommands.Add(adbCommandToRunInTerminal);
                    }
                    if (lastRunnedCommands.Count < 3)
                        lastRunnedCommands.Add(adbCommandToRunInTerminal);
                    //Run command
                    RunCommandsInWindowsCmd("adb " + adbCommandToRunInTerminal, new string[] { "adb " + adbCommandToRunInTerminal });
                    //Refresh assets if is a command that modifies project
                    if (adbCommandToRunInTerminal.Contains("logcat") == true || adbCommandToRunInTerminal.Contains("push") == true || adbCommandToRunInTerminal.Contains("pull") == true || adbCommandToRunInTerminal.Contains("screencap") == true)
                        AssetDatabase.Refresh();
                    //Reset field and check process
                    adbCommandToRunInTerminal = "";
                    lastTimeOfProccessCheck = TimeSpan.Zero;
                    return;
                }
                if (string.IsNullOrEmpty(adbCommandToRunInTerminal) == true)
                    EditorUtility.DisplayDialog("Warning", "Please enter a valid command.", "Ok");
            }
            if (GUILayout.Button("Clear", GUILayout.Height(18), GUILayout.Width(50)))
            {
                currentTerminalCommandsLog = "----= Native Android Debugger Terminal =----";
                adbCommandToRunInTerminal = "";
                lastTimeOfProccessCheck = TimeSpan.Zero;
                lastRunnedCommands.Clear();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(30);
            EditorGUILayout.LabelField("Last Runned Commands", tituloBox);
            GUILayout.Space(10);

            if (lastRunnedCommands.Count == 0)
            {
                GUIStyle topicStyle = new GUIStyle();
                topicStyle.alignment = TextAnchor.MiddleCenter;
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("No commands executed yet!", topicStyle);
                EditorGUILayout.EndHorizontal();
            }
            if (lastRunnedCommands.Count > 0)
            {
                GUIStyle topicStyle = new GUIStyle();
                topicStyle.alignment = TextAnchor.LowerLeft;
                topicStyle.wordWrap = true;
                for (int i = (lastRunnedCommands.Count - 1); i >= 0; i--)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.LabelField(lastRunnedCommands[i], topicStyle);
                    if (GUILayout.Button("Pick", GUILayout.Height(18), GUILayout.Width(45)))
                        adbCommandToRunInTerminal = lastRunnedCommands[i];
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        void UI_DeviceSettings()
        {
            GUILayout.Space(20);

            //Render the Device Settings
            GUIStyle tituloBox = new GUIStyle();
            tituloBox.fontStyle = FontStyle.Bold;
            tituloBox.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Local Wi-Fi IP Of Your Device", tituloBox);
            GUIStyle dotStyle = new GUIStyle();
            dotStyle.fontStyle = FontStyle.Bold;
            dotStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle topicStyle = new GUIStyle();
            topicStyle.fontStyle = FontStyle.Bold;
            topicStyle.alignment = TextAnchor.MiddleLeft;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            androidDebuggerPreferences.ip0 = EditorGUILayout.IntField(new GUIContent("",
                    "Local IP of Your Device."),
                    androidDebuggerPreferences.ip0, GUILayout.Width((float)Screen.width / 4.0f - 13f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(".", dotStyle, GUILayout.Width(4));
            GUILayout.FlexibleSpace();
            androidDebuggerPreferences.ip1 = EditorGUILayout.IntField(new GUIContent("",
                    "Local IP of Your Device."),
                    androidDebuggerPreferences.ip1, GUILayout.Width((float)Screen.width / 4.0f - 13f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(".", dotStyle, GUILayout.Width(4));
            GUILayout.FlexibleSpace();
            androidDebuggerPreferences.ip2 = EditorGUILayout.IntField(new GUIContent("",
                    "Local IP of Your Device."),
                    androidDebuggerPreferences.ip2, GUILayout.Width((float)Screen.width / 4.0f - 13f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(".", dotStyle, GUILayout.Width(4));
            GUILayout.FlexibleSpace();
            androidDebuggerPreferences.ip3 = EditorGUILayout.IntField(new GUIContent("",
                    "Local IP of Your Device."),
                    androidDebuggerPreferences.ip3, GUILayout.Width((float)Screen.width / 4.0f - 13f));
            GUILayout.EndHorizontal();
            if (isDeviceSettingsValid() == false)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Current configuration is invalid. Please provide an IP other than 127.0.0.1, the IP must refer to your device's IP in your Wifi network.", MessageType.Error);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Save Preferences", GUILayout.Height(24)))
            {
                SaveThePreferences(this);
                UnityEngine.Debug.Log("All preferences of Native Android Debugger are now saved.");
            }
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("For everything to work, make sure that your Android Device and Computer where this Editor is running are connected in the same Router. Computer can be connected by Cable or Wi-Fi, the important thing is that Computer and Android Device are connected in the same Router.", MessageType.Info);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Local Wi-Fi IP Of Your Device", tituloBox);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Auto Update Currently Connected Devices");
            androidDebuggerPreferences.autoUpdateListOfConnectedDevices = EditorGUILayout.Toggle(new GUIContent("",
                    ""),
                    androidDebuggerPreferences.autoUpdateListOfConnectedDevices, GUILayout.Width(14));
            GUILayout.EndHorizontal();

            GUILayout.Space(40);

            EditorGUILayout.LabelField("Helpful Topics", tituloBox);

            GUILayout.Space(10);

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Why do I need to configure this?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[0] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[0] = !helpfulTopicsExpanded[0];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[0] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                int width = Screen.width - 12;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("The debugging of this tool is focused on debugging your device over Wi-Fi, without the need for a USB cable. For this to be possible, it is necessary to perform this configuration where you can must inform the IP of your Android device, which is connected to your local network. If you do not provide a valid configuration here, providing your device IP, the vast majority of Native Android Debugger functions will not be available.", textStyle);
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("How do I configure my device's Local Wifi IP?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[1] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[1] = !helpfulTopicsExpanded[1];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[1] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                int width = Screen.width - 12;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("This configuration can be done very easily! For that, follow the steps below and everything should work fine!", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Step 1", tituloBox);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Connect your device to the SAME local network that your computer currently running this Unity Editor is connected to. Your computer can be connected through an Ethernet cable or over Wi-Fi, the important thing is that it is connected to the same local network (or router) that your device is connected to. Your Android device must be connected to that same local network via Wi-Fi.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Step 2", tituloBox);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("This step requires you to have a basic level of knowledge in Networking to perform it. You are also required to know the IP Gateway of the Wi-Fi Router your Android device is connected to.", textStyle);
                GUIStyle textStyleWarn = new GUIStyle();
                textStyleWarn.normal.textColor = Color.red;
                textStyleWarn.wordWrap = true;
                EditorGUILayout.LabelField("If you don't have these two skills, it might be better to skip to the next step. Performing this step incorrectly can leave your cell phone without internet access, and it may be necessary to delete data from your wifi network and connect to it again.", textStyleWarn);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Now let's set a static IP for your Android device so that whenever it connects to your Wi-Fi network, it receives the same IP every time from your Router. This is necessary because depending on your Router's settings, your cell phone may receive different IPs each time it connects, and it will be less of a hassle for you as you won't need to find out your device's current IP every time you use Native Android Debugger. If you prefer, you can skip this step if you really don't want to, or don't need to set a static IP.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("First, make sure your device is connected to your Wi-Fi network and you've followed the instructions in Step 1.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Go to your Android device's Wi-Fi settings, and go to your Wi-Fi networks list. Then tap to open the details of your Wi-Fi network you are currently connected to. Then click to edit your Wi-Fi network details.", textStyle);
                GUILayout.Space(10);
                GUIStyle estiloIcone = new GUIStyle();
                estiloIcone.border = new RectOffset(0, 0, 0, 0);
                estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                estiloIcone.alignment = TextAnchor.MiddleCenter;
                GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Tutorial1.png", typeof(Texture)), estiloIcone, GUILayout.Width(Screen.width - 36), GUILayout.Height(160));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("In the editing screen that opens, click on \"Advanced Options\". Let's edit your device's connection parameters.", textStyle);
                GUILayout.Space(10);
                GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Tutorial2.png", typeof(Texture)), estiloIcone, GUILayout.Width(Screen.width - 36), GUILayout.Height(280));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Now in the advanced options, change \"IP Settings\" from \"DHCP\" to \"Static\".", textStyle);
                GUILayout.Space(10);
                GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Tutorial3.png", typeof(Texture)), estiloIcone, GUILayout.Width(Screen.width - 36), GUILayout.Height(280));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Now make the changes as described below.", textStyle);
                GUILayout.Space(10);
                EditorGUI.indentLevel += 1;
                EditorGUILayout.LabelField("IP Address - Enter an IP you want. You only need to enter your IP Gateway, however, with the last house changed. For example, if your Gateway is 192.168.1.1, enter the IP 192.168.1.50 for example. Be sure to enter an IP that is not being used by other equipment.", textStyle);
                GUILayout.Space(4);
                EditorGUILayout.LabelField("Gateway - Here you just need to enter the IP Gateway of your Router that your device is connected to.", textStyle);
                GUILayout.Space(4);
                EditorGUILayout.LabelField("Network Prefix Lenght - You can leave it as it is.", textStyle);
                GUILayout.Space(4);
                EditorGUILayout.LabelField("DNS 1 - You can leave it empty if possible. If this is not possible, you can enter a DNS of your choice, such as Google (8.8.8.8) or CloudFlare (1.1.1.1).", textStyle);
                GUILayout.Space(4);
                EditorGUILayout.LabelField("DNS 2 - You can leave it empty if possible. If this is not possible, you can enter a DNS of your choice, such as Google (8.8.4.4) or CloudFlare (1.0.0.1).", textStyle);
                EditorGUI.indentLevel -= 1;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If you performed this step correctly, your Android device should now be connecting to your Wi-Fi network using a Static IP (which should not change anymore). If you ever delete this Wi-Fi network from your device, you will need to perform this step again.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If your Android device has no internet access after performing this step, it may require some more complex setup that is needed or something you need to do on your Router. To regain internet access from your device you will only need to erase your Wi-Fi network and then reconnect to it.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Step 3", tituloBox);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Now, you just need to enter the IP you set on your Android Device, here in Native Android Debugger too! If you have set IP 192.168.1.50 on your device, just enter this same IP here in this window too.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If you don't know the IP of your Android device, in your local network, you can use some app to do this, or access your Wi-Fi network details in your device's Wi-Fi settings.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("By entering an IP here in Native Android Debugger, you can see a Ping test that Native Android Debugger does, below. If the latency of your device is displayed, it means that the IP worked and the Native Android Debugger has already found your cell phone in your local network. Read the topic below to better understand the Native Android Debugger Ping.", textStyle);
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("A little warning about Device Ping", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[4] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[4] = !helpfulTopicsExpanded[4];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[4] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                int width = Screen.width - 12;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If the IP you provided to Native Android Debugger is valid and different from 127.0.0.1, Native Android Debugger will always be measuring the latency between your Computer and your device using the IP you provided. This way, you will be able to know if your computer has a communication with your device, in your local network, and you will be able to know through latency. If the Native Android Debugger is showing latency, it means there is communication between your computer and your device. If latency cannot be displayed, it means that there is some problem in communication, your cell phone or computer may be disconnected from your local network for example, or your device's IP may have changed.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Important Note: Depending on the configuration of your Android device, or Router, the Native Android Debugger may not be able to ping the device and thus display an error message. If you are sure that your computer and device are connected to the network, your device IP is correct and everything is right, don't worry, things should work as expected.", textStyle);
            }
            GUILayout.EndVertical();
        }

        void UI_SDKSettings()
        {
            GUILayout.Space(20);

            //Render the Device Settings
            GUIStyle tituloBox = new GUIStyle();
            tituloBox.fontStyle = FontStyle.Bold;
            tituloBox.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Directory of your Android SDK Folder", tituloBox);
            GUIStyle topicStyle = new GUIStyle();
            topicStyle.fontStyle = FontStyle.Bold;
            topicStyle.alignment = TextAnchor.MiddleLeft;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            androidDebuggerPreferences.sdkPath = EditorGUILayout.TextField(new GUIContent("",
                   "Path to your Android SDK."),
                   androidDebuggerPreferences.sdkPath, GUILayout.Width(Screen.width - 126));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Get From Editor", GUILayout.Height(18), GUILayout.Width(100)))
            {
                androidDebuggerPreferences.sdkPath = getSdkPathFromUnity();
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
            if (isPhoneSdkSettingsValid() == false)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Current configuration is invalid. Please provide a valid path to your Android SDK Root folder. Click the \"Get From Editor\" button to do this automatically (if your Editor already has its own Android SDK included).", MessageType.Error);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Save Preferences", GUILayout.Height(24)))
            {
                SaveThePreferences(this);
                UnityEngine.Debug.Log("All preferences of Native Android Debugger are now saved.");
            }
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("For everything to work as expected, provide the Directory for your Android SDK Root folder. It is recommended that you use the SDK that is already included with your Unity Editor. To do this, click the \"Get From Editor\" button to use the same Android SDK that this Unity Editor uses.", MessageType.Info);

            GUILayout.Space(40);

            EditorGUILayout.LabelField("Helpful Topics", tituloBox);

            GUILayout.Space(10);

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Why do I need to configure this?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[2] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[2] = !helpfulTopicsExpanded[2];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[2] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                int width = Screen.width - 12;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("To perform debugging, the Native Android Debugger uses ADB, which is part of the Android SDK (Android App Development Toolkit). ADB is also used by Unity Editor to send the generated APK to your device and do other things.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("You need to provide the path to some Android SDK that is on your computer, so Native Android Debugger will access the ADB of the SDK you provide, to use the debugging functions. It is always recommended that you use the same Android SDK that Unity Editor uses. You can click \"Get From Editor\" to get the Android SDK path of this Unity Editor.", textStyle);
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            //Topic
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("How do I find the directory for my Android SDK Root Folder?", topicStyle);
            if (GUILayout.Button((helpfulTopicsExpanded[3] == false) ? "Show" : "Hide", GUILayout.Height(18), GUILayout.Width(80)))
                helpfulTopicsExpanded[3] = !helpfulTopicsExpanded[3];
            GUILayout.EndHorizontal();
            if (helpfulTopicsExpanded[3] == true)
            {
                GUIStyle textStyle = new GUIStyle();
                textStyle.wordWrap = true;
                int width = Screen.width - 12;
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If your Unity Editor has an Android SDK built in, you just need to get the Android SDK root directory and insert it here in Native Android Debugger. To see if your Editor has an Android SDK built in, go to \"Editor > Preferences > External Tools\".", textStyle);
                GUILayout.Space(10);
                GUIStyle estiloIcone = new GUIStyle();
                estiloIcone.border = new RectOffset(0, 0, 0, 0);
                estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                estiloIcone.alignment = TextAnchor.MiddleCenter;
                GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/Native Android Debugger/Editor/Images/Tutorial4.png", typeof(Texture)), estiloIcone, GUILayout.Width(Screen.width - 36), GUILayout.Height(450));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("If your Unity Editor doesn't have an Android SDK built in, you'll just need to take the root directory of an Android SDK you've already downloaded, and insert it here in the Native Android Debugger.", textStyle);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("By clicking \"Get From Editor\", the Native Android Debugger will fill in the path to the SDK automatically, using the Android SDK built into this Unity Editor. This will only work if this Unity Editor has a built-in Android SDK.", textStyle);
            }
            GUILayout.EndVertical();
        }

        void UI_StatusBar(Texture running, Texture notRunning, Texture pingOn, Texture pingOff)
        {
            //Draw the status bar
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            //Prepare the adb process checker
            if (isPhoneSdkSettingsValid() == true)
            {
                GUIStyle estiloIcone = new GUIStyle();
                estiloIcone.border = new RectOffset(0, 0, 0, 0);
                estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                GUIStyle tituloBox = new GUIStyle();
                tituloBox.fontStyle = FontStyle.Bold;
                tituloBox.alignment = TextAnchor.MiddleLeft;

                GUILayout.BeginHorizontal();
                string FilePath = Path.GetDirectoryName(androidDebuggerPreferences.sdkPath + "/platform-tools/adb.exe");
                string FileName = Path.GetFileNameWithoutExtension(androidDebuggerPreferences.sdkPath + "/platform-tools/adb.exe").ToLower();
                TimeSpan timeElapsed = (new TimeSpan(DateTime.Now.Ticks) - lastTimeOfProccessCheck);
                if (timeElapsed.Seconds > 5 || lastTimeOfProccessCheck == TimeSpan.Zero)
                {
                    isProccessRunning = false;
                    Process[] pList = Process.GetProcessesByName(FileName);
                    foreach (Process p in pList)
                        if (p.MainModule.FileName.StartsWith(FilePath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isProccessRunning = true;
                            break;
                        }
                    lastTimeOfProccessCheck = new TimeSpan(DateTime.Now.Ticks);
                }
                if (isProccessRunning == true)
                {
                    GUILayout.BeginVertical(GUILayout.Width(14));
                    GUILayout.Space(5);
                    GUILayout.Box(running, estiloIcone, GUILayout.Width(12), GUILayout.Height(11));
                    GUILayout.EndVertical();
                    string[] sdkPathFixed = androidDebuggerPreferences.sdkPath.Split(new[] { "/2" }, StringSplitOptions.None);
                    if (sdkPathFixed.Length <= 1)
                        sdkPathFixed = androidDebuggerPreferences.sdkPath.Split(new[] { "\\2" }, StringSplitOptions.None);
                    EditorGUILayout.LabelField("ADB Running - 2" + sdkPathFixed[1], tituloBox, GUILayout.Width(20));
                }
                if (isProccessRunning == false)
                {
                    GUILayout.BeginVertical(GUILayout.Width(14));
                    GUILayout.Space(5);
                    GUILayout.Box(notRunning, estiloIcone, GUILayout.Width(12), GUILayout.Height(11));
                    GUILayout.EndVertical();
                    string[] sdkPathFixed = androidDebuggerPreferences.sdkPath.Split(new[] { "/2" }, StringSplitOptions.None);
                    if (sdkPathFixed.Length <= 1)
                        sdkPathFixed = androidDebuggerPreferences.sdkPath.Split(new[] { "\\2" }, StringSplitOptions.None);
                    EditorGUILayout.LabelField("ADB Stopped - 2" + sdkPathFixed[1], tituloBox, GUILayout.Width(20));
                }
                GUILayout.EndHorizontal();
            }
            if (isPhoneSdkSettingsValid() == false)
            {
                GUIStyle tituloBox = new GUIStyle();
                tituloBox.fontStyle = FontStyle.Bold;
                tituloBox.alignment = TextAnchor.MiddleCenter;
                EditorGUILayout.LabelField("Please correct your settings in SDK Settings!", tituloBox);
            }
            //Prepare the IP
            string deviceIp = androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3;
            //If the IP is different from 127.0.0.1
            if (deviceIp != "127.0.0.1")
            {
                //Create the ping object, if not exists
                if (devicePingObject == null)
                {
                    lastDevicePingStarted = new TimeSpan(DateTime.Now.Ticks);
                    devicePingObject = new Ping(deviceIp);
                }
                if (devicePingObject != null)
                {
                    if (devicePingObject.isDone == true)
                    {
                        lastDevicePingStarted = new TimeSpan(DateTime.Now.Ticks);
                        lastDevicePingTime = devicePingObject.time;
                        devicePingObject = new Ping(deviceIp);
                    }
                    if (devicePingObject.isDone == false)
                    {
                        TimeSpan timeElapsed = (new TimeSpan(DateTime.Now.Ticks) - lastDevicePingStarted);
                        if (timeElapsed.Seconds > 5)
                        {
                            lastDevicePingTime = -1;
                            devicePingObject = null;
                        }
                    }
                }
                //Show the ping
                if (lastDevicePingTime == -1)
                {
                    GUIStyle estiloIcone = new GUIStyle();
                    estiloIcone.border = new RectOffset(0, 0, 0, 0);
                    estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                    GUIStyle tituloBox = new GUIStyle();
                    tituloBox.alignment = TextAnchor.MiddleLeft;
                    tituloBox.normal.textColor = new Color(107f / 255.0f, 0f / 255.0f, 0f / 255.0f, 1.0f);
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(14));
                    GUILayout.Space(5);
                    GUILayout.Box(pingOff, estiloIcone, GUILayout.Width(12), GUILayout.Height(12));
                    GUILayout.EndVertical();
                    EditorGUILayout.LabelField("Device Ping in " + androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3 + " is timed out", tituloBox);
                    GUILayout.EndHorizontal();
                }
                if (lastDevicePingTime != -1)
                {
                    GUIStyle estiloIcone = new GUIStyle();
                    estiloIcone.border = new RectOffset(0, 0, 0, 0);
                    estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                    GUIStyle tituloBox = new GUIStyle();
                    tituloBox.alignment = TextAnchor.MiddleLeft;
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(14));
                    GUILayout.Space(5);
                    GUILayout.Box(pingOn, estiloIcone, GUILayout.Width(12), GUILayout.Height(12));
                    GUILayout.EndVertical();
                    EditorGUILayout.LabelField("Device Ping in " + androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3 + " is " + lastDevicePingTime + "ms", tituloBox);
                    GUILayout.EndHorizontal();
                }
            }
            //If the IP is equal to 127.0.0.1
            if (deviceIp == "127.0.0.1")
            {
                GUIStyle estiloIcone = new GUIStyle();
                estiloIcone.border = new RectOffset(0, 0, 0, 0);
                estiloIcone.margin = new RectOffset(4, 0, 4, 0);
                GUIStyle tituloBox = new GUIStyle();
                tituloBox.alignment = TextAnchor.MiddleLeft;

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(14));
                GUILayout.Space(5);
                GUILayout.Box(pingOff, estiloIcone, GUILayout.Width(12), GUILayout.Height(12));
                GUILayout.EndVertical();
                EditorGUILayout.LabelField("Please inform the IP of the Device!", tituloBox);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        void OnInspectorUpdate()
        {
            //On inspector update, on lost focus in this Window, update the GUI (force update if window is on focus or not, to mantain accuracy of ping)
            if (isWindowOnFocus == false || isWindowOnFocus == true)
            {
                //Update this window
                Repaint();
                //Update the scene GUI
                if (SceneView.lastActiveSceneView != null)
                    SceneView.lastActiveSceneView.Repaint();
            }

            //Try to load the preferences on inspector update (if this window is in focus or not, try to load here, because this method runs after OpenWindow() method)
            if (preferencesLoadedOnInspectorUpdate == false)
            {
                if (androidDebuggerPreferences.windowPosition.x != 0 && androidDebuggerPreferences.windowPosition.y != 0)
                    LoadThePreferences(this);
                preferencesLoadedOnInspectorUpdate = true;
            }
        }

        void RunCommandsInWindowsCmd(string commandTitle, string[] commandsToBeRunned)
        {
            //Show the progress bar
            EditorUtility.DisplayProgressBar("Running", "Running ADB Command: " + commandTitle, 1.0f);

            //Get current date
            DateTime date = DateTime.Now;

            //Start CMD process
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = Application.dataPath;
            process.StartInfo = startInfo;
            process.Start();

            //Prepare the list of commands to be runned
            List<string> commands = new List<string>();
            //Fill the list
            commands.Add("echo off");
            commands.Add("cd /d " + androidDebuggerPreferences.sdkPath + "/platform-tools");
            foreach (string str in commandsToBeRunned)
                commands.Add(str);
            commands.Add("exit");

            //Register variables to get exits
            string normalExit = "";
            string errorsExit = "";

            //Send commands to CMD
            using (StreamWriter sw = process.StandardInput)
                if (sw.BaseStream.CanWrite)
                    foreach (string cmd in commands)
                        sw.WriteLine(cmd);

            //Register the exits
            using (StreamReader streamReader = process.StandardOutput)
                normalExit = streamReader.ReadToEnd();
            using (StreamReader streamReader = process.StandardError)
                errorsExit = streamReader.ReadToEnd();

            //Split exit into lines
            string[] normalExitInLines = normalExit.Split('\n');

            //Read each line and add to apropriate place
            for (int i = 0; i < normalExitInLines.Length; i++)
            {
                //Remo equal lines to command sends
                for (int x = 0; x < commands.Count; x++)
                    if (normalExitInLines[i].Contains(commands[x]) == true || normalExitInLines[i].Contains("cd /d") == true || normalExitInLines[i].Contains("Microsoft Windows [") == true || normalExitInLines[i].Contains("Microsoft Corporation.") == true || string.IsNullOrEmpty(normalExitInLines[i]) == true)
                        normalExitInLines[i] = "%exclude%";
            }

            //Monta a nova saida modificada
            string currentDate = "[" + ((date.Hour < 10) ? "0" + date.Hour.ToString() : date.Hour.ToString()) + ":" + ((date.Minute < 10) ? "0" + date.Minute.ToString() : date.Minute.ToString()) + ":" + ((date.Second < 10) ? "0" + date.Second.ToString() : date.Second.ToString()) + "] ";
            StringBuilder newCmdExit = new StringBuilder();
            newCmdExit.Append(currentDate);
            newCmdExit.Append("[ Running Command \"" + commandTitle + "\" ]");
            foreach (string str in normalExitInLines)
            {
                if (str.Contains("%exclude%") == true)
                    continue;

                newCmdExit.Append("\n");
                newCmdExit.Append(str);
            }
            if (string.IsNullOrEmpty(errorsExit) == false)
            {
                newCmdExit.Append("\n[ Lines Below Require Your Attention ]\n");
                newCmdExit.Append("\n");
                newCmdExit.Append(errorsExit);
            }
            DateTime newDate = DateTime.Now;
            string newCurrentDate = "[" + ((newDate.Hour < 10) ? "0" + newDate.Hour.ToString() : newDate.Hour.ToString()) + ":" + ((newDate.Minute < 10) ? "0" + newDate.Minute.ToString() : newDate.Minute.ToString()) + ":" + ((newDate.Second < 10) ? "0" + newDate.Second.ToString() : newDate.Second.ToString()) + "] ";
            newCmdExit.Append("\n");
            newCmdExit.Append(newCurrentDate);
            newCmdExit.Append("[ Command Execution Is Completed In " + ((new TimeSpan(newDate.Ticks) - new TimeSpan(date.Ticks)).TotalSeconds.ToString("F1")) + " Seconds]");
            currentQuickCommandsLog = newCmdExit.ToString();
            currentTerminalCommandsLog += "\n\n" + newCmdExit.ToString() + "\n\n" + "---------------------------------------------";

            //Clear the progress bar
            EditorUtility.ClearProgressBar();
        }

        string GetCommandPickedFromPresets()
        {
            //Prepare the command
            string command = "";
            //Get the command
            switch (adbCommandToRunInTerminalPreSet)
            {
                case CommandPreSet.Pick:
                    command = "";
                    break;
                case CommandPreSet.usb:
                    UnityEngine.Debug.Log("Command \"usb\": Will restart ADB USB connection mode.");
                    command = "usb";
                    break;
                case CommandPreSet.tcpip5555:
                    UnityEngine.Debug.Log("Command \"tcpip 5555\": Will restart ADB TCP/IP connection mode.");
                    command = "tcpip 5555";
                    break;
                case CommandPreSet.reboot:
                    UnityEngine.Debug.Log("Command \"reboot\": Will restart your phone.");
                    command = "reboot";
                    break;
                case CommandPreSet.devices_l:
                    UnityEngine.Debug.Log("Command \"devices -l\": It will list all devices connected to your ADB, however, it will also list with the device brand and model.");
                    command = "devices -l";
                    break;
                case CommandPreSet.logcat_c:
                    UnityEngine.Debug.Log("Command \"logcat -c\": It will clear logs from your device.");
                    command = "logcat -c";
                    break;
                case CommandPreSet.logcat_d:
                    UnityEngine.Debug.Log("Command \"logcat -d\": It will get all current logs from your device and save it in a txt file, in a directory on your PC.");
                    command = "logcat -d > \"" + Application.dataPath + "/logcat.txt\"";
                    break;
                case CommandPreSet.uninstall_package:
                    UnityEngine.Debug.Log("Command \"uninstall\": Will uninstall an app from your phone.");
                    command = "uninstall com.package";
                    break;
                case CommandPreSet.uninstall_k_package:
                    UnityEngine.Debug.Log("Command \"uninstall -k\": Will uninstall an app from your phone, however, keeping the data.");
                    command = "uninstall -k com.package";
                    break;
                case CommandPreSet.shell_pm_clear_package:
                    UnityEngine.Debug.Log("Command \"shell pm clear\": Deletes all data associated with a package.");
                    command = "shell pm clear com.package";
                    break;
                case CommandPreSet.exec_out_screencap_p:
                    UnityEngine.Debug.Log("Command \"exec-out screencap -p\": It will capture your device's screen.");
                    command = "exec-out screencap -p > \"" + Application.dataPath + "/printscreen.png\"";
                    break;
                case CommandPreSet.version:
                    UnityEngine.Debug.Log("Command \"version\": Show current version of ADB.");
                    command = "version";
                    break;
                case CommandPreSet.connect:
                    UnityEngine.Debug.Log("Command \"connect\": Connect to a device with IP.");
                    command = "connect " + androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3;
                    break;
                case CommandPreSet.disconnect:
                    UnityEngine.Debug.Log("Command \"disconnect\": Disconnect from a device with IP.");
                    command = "disconnect " + androidDebuggerPreferences.ip0 + "." + androidDebuggerPreferences.ip1 + "." + androidDebuggerPreferences.ip2 + "." + androidDebuggerPreferences.ip3;
                    break;
            }
            //Reset the selector
            adbCommandToRunInTerminalPreSet = CommandPreSet.Pick;
            //REturn the command
            return command;
        }

        void UpdateListOfAllConnectedDevices()
        {
            //If auto update is disabled, cancel
            if (androidDebuggerPreferences.autoUpdateListOfConnectedDevices == false)
                return;
            //If cannot run the command to update, cancel
            TimeSpan timeElapsed = (new TimeSpan(DateTime.Now.Ticks) - lastAutoCheckOfConnectedDevices);
            if (timeElapsed.Seconds < 3 && lastAutoCheckOfConnectedDevices != TimeSpan.Zero)
                return;
            //Clear the list of connected devices
            listOfCurrentlyConnectedDevices.Clear();
            //Get time of start of cmd
            DateTime startDate = DateTime.Now;
            //Get the string with connected devices
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = Application.dataPath;
            process.StartInfo = startInfo;
            process.Start();
            List<string> commands = new List<string>();
            commands.Add("echo off");
            commands.Add("cd /d " + androidDebuggerPreferences.sdkPath + "/platform-tools");
            commands.Add("adb devices");
            commands.Add("exit");
            using (StreamWriter sw = process.StandardInput)
                if (sw.BaseStream.CanWrite)
                    foreach (string cmd in commands)
                        sw.WriteLine(cmd);
            string cmdTextReturn = "";
            using (StreamReader streamReader = process.StandardOutput)
                cmdTextReturn = streamReader.ReadToEnd();
            //Get time of end of cmd
            DateTime endDate = DateTime.Now;
            //If time of cmd is long, cancel auto refresh
            if ((new TimeSpan(endDate.Ticks) - new TimeSpan(startDate.Ticks)).Milliseconds > 300)
            {
                UnityEngine.Debug.LogWarning("It appears that there is some kind of delay occurring when checking the list of currently connected devices. Automatic update of currently connected devices has been disabled. You can enable it again under \"Device Settings\" in Native Android Debugger.");
                androidDebuggerPreferences.autoUpdateListOfConnectedDevices = false;
            }
            //Process returned text
            string[] cmdTextReturnInLines = cmdTextReturn.Split('\n');
            for (int i = 0; i < cmdTextReturnInLines.Length; i++)
            {
                for (int x = 0; x < commands.Count; x++)
                    if (cmdTextReturnInLines[i].Contains(commands[x]) == true || cmdTextReturnInLines[i].Contains("cd /d") == true || cmdTextReturnInLines[i].Contains("Microsoft Windows [") == true || cmdTextReturnInLines[i].Contains("Microsoft Corporation.") == true || string.IsNullOrEmpty(cmdTextReturnInLines[i]) == true)
                        cmdTextReturnInLines[i] = "%exclude%";
            }
            //Fill the list
            foreach (string str in cmdTextReturnInLines)
            {
                //Skip if is not a device
                if (str.Contains("%exclude%") == true)
                    continue;
                if (str.Contains("List of devices") == true)
                    continue;
                if (Regex.IsMatch(str, @"[a-zA-Z]") == false && Regex.IsMatch(str, @"[0-9]") == false)
                    continue;

                //If is a device...
                ConnectedDevice device = new ConnectedDevice();
                //Reveal type
                if (str.Contains("device") == true)
                {
                    device.deviceName = str.Split(new[] { "device" }, StringSplitOptions.None)[0].Replace(" ", "");
                    if (device.deviceName.Contains(":5555") == true) //<- If is connected over wifi
                    {
                        device.connectedOver = ConnectedOver.WiFi;
                        device.deviceStatus = ConnectedStatus.Online;
                    }
                    if (device.deviceName.Contains(":5555") == false) //<- If is connected over usb
                    {
                        device.connectedOver = ConnectedOver.Usb;
                        device.deviceStatus = ConnectedStatus.Device;
                    }
                }
                if (str.Contains("offline") == true)
                {
                    device.deviceName = str.Split(new[] { "offline" }, StringSplitOptions.None)[0].Replace(" ", "");
                    device.connectedOver = ConnectedOver.WiFi;
                    device.deviceStatus = ConnectedStatus.Offline;
                }
                //Add this device to list
                listOfCurrentlyConnectedDevices.Add(device);
            }
            //Fill the timer
            lastAutoCheckOfConnectedDevices = new TimeSpan(DateTime.Now.Ticks);
        }
        #endregion

        private static void LoadThePreferences(AndroidDebuggerTool instance)
        {
            //Create the default directory, if not exists
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/_AssetsData"))
                AssetDatabase.CreateFolder("Assets/MT Assets", "_AssetsData");
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/_AssetsData/Preferences"))
                AssetDatabase.CreateFolder("Assets/MT Assets/_AssetsData", "Preferences");

            //Try to load the preferences file
            androidDebuggerPreferences = (AndroidDebuggerPreferences)AssetDatabase.LoadAssetAtPath("Assets/MT Assets/_AssetsData/Preferences/NativeAndroidDebugger.asset", typeof(AndroidDebuggerPreferences));
            //Validate the preference file. if this preference file is of another project, delete then
            if (androidDebuggerPreferences != null)
            {
                if (androidDebuggerPreferences.projectName != Application.productName)
                {
                    AssetDatabase.DeleteAsset("Assets/MT Assets/_AssetsData/Preferences/NativeAndroidDebugger.asset");
                    androidDebuggerPreferences = null;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                if (androidDebuggerPreferences != null && androidDebuggerPreferences.projectName == Application.productName)
                {
                    //Set the position of Window 
                    //instance.position = androidDebuggerPreferences.windowPosition; <- To prevent unpin the window, if user as pinned the window in Editor
                }
            }
            //If null, create and save a preferences file
            if (androidDebuggerPreferences == null)
            {
                androidDebuggerPreferences = ScriptableObject.CreateInstance<AndroidDebuggerPreferences>();
                androidDebuggerPreferences.projectName = Application.productName;
                AssetDatabase.CreateAsset(androidDebuggerPreferences, "Assets/MT Assets/_AssetsData/Preferences/NativeAndroidDebugger.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void SaveThePreferences(AndroidDebuggerTool instance)
        {
            //Cancel if not found the preferences <-- Avoid log erros on enter play mode with window opened
            if (androidDebuggerPreferences == null || Application.isPlaying == true)
                return;

            //Save the preferences in Prefs.asset
            androidDebuggerPreferences.projectName = Application.productName;
            androidDebuggerPreferences.windowPosition = new Rect(instance.position.x, instance.position.y, instance.position.width, instance.position.height);
            EditorUtility.SetDirty(androidDebuggerPreferences);
            AssetDatabase.SaveAssets();
        }
    }
}