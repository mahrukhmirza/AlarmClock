using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
/// <summary>
/// mahrukh sameen mirza 
/// april 17,2019
/// this is the script for alarm 
/// </summary>
public class AlarmClock : EditorWindow
//EditorWindow class is a part of UnityEditor namespace
{
    AudioClip alarmSound;
    //alarm duration as 33 because the audio is 33 seconds long
    private int alarmDuration = 33;
    // variables declared string for serialization purposes
    string alarmTime = "12:00:00 AM";
    string alarmTimeEnter = "12:00:00 AM";
    bool hasAlarm = false;
    bool alarmRinging = false;
    float timeAlarmStarted = 0;
    bool firstTime = true;
    int oldSeconds = 0;
    /* MenuItem is an attribute which makes a menu 
    in editors file menu */
    [MenuItem("Window/AlarmClock")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AlarmClock), false, "AlarmClock");
    }
    /// <summary>
    ///  OnGUI can be used to render the GUI and 
    ///  calls only when our editor window is clicked 
    /// </summary>
    void OnGUI()
    {   /* BeginHorizonatal and EndHorizontal are used to give proper 
        layout in the window */
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Alarm Time");
        alarmTimeEnter = GUILayout.TextField(alarmTimeEnter);
        if (GUILayout.Button("SET"))
        {
            alarmRinging = false;
            try
            {
                DateTime alarm = DateTime.Parse(alarmTimeEnter);
                alarmTimeEnter = alarmTime = alarm.ToLongTimeString();
                hasAlarm = true;
            }
            catch (System.Exception)
            {
                alarmTimeEnter = alarmTime;
            }
        }

        hasAlarm = GUILayout.Toggle(hasAlarm, "Alarm On :(" + alarmTime + ")");
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("STOP ALARM"))
        {
            if (alarmSound != null) StopClip(alarmSound);
            alarmRinging = false;
        }

        if (GUILayout.Button("SNOOZE ALARM"))
        {
            if (alarmSound != null)
            {
                StopClip(alarmSound);
                PlayDelayed(5);
            }
        }
    }
    /// <summary>
    /// PlayDelayed method is used to delay the audio clip 
    /// when the snooze button is clicked 
    /// </summary>
    /// <param name="delay"></param>
    public void PlayDelayed(float delay)
    {
        PlayClip(alarmSound);
    }
    /// <summary>
    /// OnEnable is used for initializing the resources 
    /// </summary>
    void OnEnable()
    {
        if (alarmSound == null)
        {
            alarmSound = (AudioClip)EditorGUIUtility.Load("play.mp3");

        }

        if (firstTime)
        {
            DateTime alarm = DateTime.Now;
            alarmTime = alarm.ToLongTimeString();
            alarmTimeEnter = alarmTime;
            firstTime = false;
        }
    }
    /// <summary>
    /// for playing and stopping of audio 
    /// PlayClip and StopClip are used 
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static |
          BindingFlags.Public, null, new System.Type[] { typeof(AudioClip) }, null);
        method.Invoke(null, new object[] { clip });
    }
    public static void StopClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod("StopClip", BindingFlags.Static |
          BindingFlags.Public, null, new System.Type[] { typeof(AudioClip) }, null);
        method.Invoke(null, new object[] { clip });
    }
    void Update()
    {
        DateTime alarm = DateTime.Parse(alarmTime);
        int seconds = (int)(DateTime.Now.TimeOfDay - alarm.TimeOfDay).TotalSeconds;
        // taking 86400 seconds in a day
        seconds = (seconds + 86400) % 86400;
        if (seconds < 1000 && hasAlarm)
        {
            if (!alarmRinging && oldSeconds > 1000)
            {
                Debug.Log("ALARM STARTED" + DateTime.Now.ToLongTimeString());
                timeAlarmStarted = (float)EditorApplication.timeSinceStartup;
                if (alarmSound != null) PlayClip(alarmSound);
                alarmRinging = true;
            }
        }
        if (alarmRinging && seconds > alarmDuration)
        {
            Debug.Log("ALARM STOPPED" + DateTime.Now.ToLongTimeString());
            alarmRinging = false;
            if (alarmSound != null) StopClip(alarmSound);
        }
        oldSeconds = seconds;
    }
}