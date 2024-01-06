using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications_apk1 : MonoBehaviour
{
    private List<DataEntry> dataList; // List to store data entries

    public DirectionalLight red_light_alarm; // Reference to a directional light for alarms

    // References to different game objects for displaying notifications
    public GameObject warning_temp;
    public GameObject urgent_fire;
    public GameObject update_temp;
    public Notifications_hologram holograma_notifications;

    // Threshold values for temperature conditions
    private double threshold_t_alta = 26;
    private double threshold_t_incendio = 50;
    private double threshold_t_normal = 25;

    // Variables to store current temperature and people count
    private double temperature;
    private double people;

    // Flags to track activation of different notifications
    private bool activ_1 = false;
    private bool activ_2 = false;
    private bool activ_3 = false;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // Load data from JSON file
        yield return StartCoroutine(LoadDataFromJson());

        // Start the Coroutine for Apk1
        StartCoroutine(Apk_1());
    }

    // Coroutine to load data from a JSON file
    IEnumerator LoadDataFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Data_Floor_Building");
        if (jsonTextAsset == null)
        {
            Debug.LogError("JSON file 'Data_Floor_Building' not found in Resources.");
            yield break;
        }

        // Parse JSON data into a list of DataEntry objects
        DataList loadedDataList = JsonUtility.FromJson<DataList>("{\"items\":" + jsonTextAsset.text + "}");
        if (loadedDataList == null)
        {
            Debug.LogError("Failed to load data from JSON.");
            yield break;
        }

        dataList = loadedDataList.items;
    }

    // Coroutine to handle Apk1 logic
    IEnumerator Apk_1()
    {
        int time = 480; // Initial time value

        while (true)
        {
            // Read temperature data for Building 1
            DataEntry temp = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 && e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            temperature = temp.Value;

            // Read people count for Building 1
            DataEntry p = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 && e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time);
            people = p.Value;

            // Check temperature conditions and activate corresponding notifications
            if (temperature >= threshold_t_alta && temperature < threshold_t_incendio && activ_1 == false)
            {
                warning_temp.SetActive(true);
                red_light_alarm.AlarmOnOrange();
                holograma_notifications.HighTemperatureHolograma();
                activ_1 = true;
            }
            else if (temperature >= threshold_t_incendio && activ_2 == false)
            {
                warning_temp.SetActive(false);
                urgent_fire.SetActive(true);
                red_light_alarm.AlarmOnRed();
                holograma_notifications.incendioHolograma();
                activ_2 = true;
            }
            else if (temperature <= threshold_t_normal && people == 0 && activ_3 == false)
            {
                urgent_fire.SetActive(false);
                update_temp.SetActive(true);
                red_light_alarm.AlarmOnGreen();
                holograma_notifications.situacionNormal_holograma();
                activ_3 = true;
            }
            else if (time >= 780)
            {
                // Turn off alarms when the time exceeds a certain limit
                update_temp.SetActive(false);
                red_light_alarm.AlarmOff();
            }

            // Ensure continuous data flow by resetting time after reaching a certain limit
            if (time >= 1434)
            {
                time = 0;
            }

            time += 5; // Move time forward by 5 units (representing 1 minute out of every 10)
            yield return new WaitForSeconds(1); // Wait for 1 second (1 second represents 10 minutes)
        }
    }
}