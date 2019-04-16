using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// mahrukh sameen mirza
/// april 15,2019
/// this script displays the time according to your system
/// </summary>

public class DigitalClockSystem : MonoBehaviour
{
    DateTime dateFetcher;
    public Text hoursUI, minutesUI, secondsUI;
    public Text ampmUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dateFetcher = DateTime.Now;
        hoursUI.text = dateFetcher.Hour.ToString();
        minutesUI.text = dateFetcher.Minute.ToString();
        secondsUI.text = dateFetcher.Second.ToString();
        if(dateFetcher.Hour>12 && dateFetcher.Hour<24)
        {
            ampmUI.text = "PM";
        }
        if (dateFetcher.Hour<12 && dateFetcher.Hour>=0)
        {
            ampmUI.text = "AM";
        }
      //  print("print:" + dateFetcher);
    }
}
