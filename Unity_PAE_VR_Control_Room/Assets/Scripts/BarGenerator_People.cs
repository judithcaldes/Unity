using Oculus.Interaction.UnityCanvas;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BarGenerator_People : MonoBehaviour
{
    public Text peopleText;
    private List<DataEntry> dataList;
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;
    public GameObject BarDay1;
    public GameObject BarDay2;
    public GameObject BarDay3;
    public GameObject BarDay4;
    public GameObject BarDay5;
    public GameObject BarDay6;
    public GameObject BarDay7;
    public GameObject ValueAxes1;
    public GameObject ValueAxes2;
    public GameObject ValueAxes3;
    System.Random rand = new System.Random();
    List<Color> colors = new List<Color>();
    public Text txt1;
    public Text txt2;
    public Text txt3;
    public Text txt4;
    public Text txt5;
    public Text txt6;
    public Text txt7;
    // Contains the mean value of people obtained at every hour for the entire building
    List<float> Building1 = new List<float>();
    List<float> Building2 = new List<float>();
    List<float> Building3 = new List<float>();
    public int start_time = 480;
    [HideInInspector]
    public int time = 0;
    bool build1 = false;
    bool build2 = false;
    bool build3 = false;
    int t = 0;
    double value1 = 0;
    double value2 = 0;
    double value3 = 0;
    List<string> week_days = new List<string>();
    string month = "December 2023";
    List<string> dates = new List<string>();
    int count_date = 0;
    int count_day = 0;
    int actual_day = 6;

    List<float> daysBuilding1 = new List<float>();
    List<float> daysBuilding2 = new List<float>();
    List<float> daysBuilding3 = new List<float>();

    void Start()
    {
        // Initialize colors list:
        colors.Add(Color.blue);
        colors.Add(Color.cyan);
        colors.Add(Color.green);
        colors.Add(Color.yellow);
        colors.Add(Color.red);

        // Initialize week_days List:
        week_days.Add("Tuesday, ");
        week_days.Add("Wednesady, ");
        week_days.Add("Thursday, ");
        week_days.Add("Friday, ");
        week_days.Add("Saturday, ");
        week_days.Add("Sunday, ");
        week_days.Add("Monday, ");

        // Initialize dates List:
        dates.Add("12th ");
        dates.Add("13th ");
        dates.Add("14th ");
        dates.Add("15th ");
        dates.Add("16th ");
        dates.Add("17th ");
        dates.Add("18th ");
        dates.Add("19th ");
        dates.Add("20th ");
        dates.Add("21st ");
        dates.Add("22nd ");
        dates.Add("23rd ");
        dates.Add("24th ");
        dates.Add("25th ");
        dates.Add("26th ");
        dates.Add("27th ");
        dates.Add("28th ");
        dates.Add("29th ");
        dates.Add("30th ");

        // Days building1 %
        daysBuilding1.Add((float)0.76);
        daysBuilding1.Add((float)0.95);
        daysBuilding1.Add((float)0.89);
        daysBuilding1.Add((float)0.65);
        daysBuilding1.Add((float)1);
        daysBuilding1.Add((float)0.96);
        daysBuilding1.Add((float)1);

        // Days building2 %
        daysBuilding2.Add((float)0.98);
        daysBuilding2.Add((float)0.82);
        daysBuilding2.Add((float)1);
        daysBuilding2.Add((float)0.65);
        daysBuilding2.Add((float)1);
        daysBuilding2.Add((float)0.96);
        daysBuilding2.Add((float)0.75);

        // Days building1 %
        daysBuilding3.Add((float)1);
        daysBuilding3.Add((float)0.62);
        daysBuilding3.Add((float)0.89);
        daysBuilding3.Add((float)1);
        daysBuilding3.Add((float)1);
        daysBuilding3.Add((float)0.96);
        daysBuilding3.Add((float)0.83);


        // Initialize text label
        txt1.text = week_days[count_day] + dates[count_date] + month;
        txt2.text = week_days[count_day + 1] + dates[count_date + 1] + month;
        txt3.text = week_days[count_day + 2] + dates[count_date + 2] + month;
        txt4.text = week_days[count_day + 3] + dates[count_date + 3] + month;
        txt5.text = week_days[count_day + 4] + dates[count_date + 4] + month;
        txt6.text = week_days[count_day + 5] + dates[count_date + 5] + month;
        txt7.text = week_days[count_day + 6] + dates[count_date + 6] + month;
        count_day = (count_day + 1) % 7;
        count_date = (count_date + 1) % 19;

        // Load data from JSON 
        StartCoroutine(LoadDataFromJson());

        // Initialize the previous days data
        CompletePreviousDays();

        // Enables the visibility of the bars from last 6 days
        EnableBarVisibility();
        Debug.Log("Completed previous days");
        // Updates today's bars
        StartCoroutine(UpdatePeople());

    }
    private void Update()
    {
        //Debug.Log("Time: " + time.ToString());
        //if (time == 10380 && build1 == false)
        //{
        //    DropdownHologramSelected(1);
        //    Debug.Log("dropdown pressed to change to 2");
        //    build1 = true;
        //}
        //if (time == 11640 && build2 == false)
        //{
        //    DropdownHologramSelected(0);
        //    Debug.Log("dropdown pressed to change to 1");
        //    build2 = true;
        //}
        //if (time == 12000 && build3 == false)
        //{
        //    DropdownHologramSelected(2);
        //    Debug.Log("dropdown pressed to change to 3");
        //    build3 = true;
        //}
    }
    IEnumerator LoadDataFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Data_Floor_Building");
        if (jsonTextAsset == null)
        {
            Debug.LogError("JSON file 'Data_Floor_Building' not found in Resources.");
            yield break;
        }

        DataList loadedDataList = JsonUtility.FromJson<DataList>("{\"items\":" + jsonTextAsset.text + "}"); // Crea una lista de datos desde el archivo JSON
        if (loadedDataList == null)
        {
            Debug.LogError("Failed to load data from JSON.");
            yield break;
        }

        dataList = loadedDataList.items;
    }

    IEnumerator UpdatePeople()
    {
        Debug.Log("Starting with the update of today's Building1 Graph");
        //int t = 0;
        int hour = start_time / 60;
        int day = 7;
        string str;
        GameObject barx = null;
        //double value1 = 0;
        //double value2 = 0;
        //double value3 = 0;

        // For the previous 6 days
        while (Hologram1.activeSelf && day == 7)
        {
            // Get value Building 1
            DataEntry entry11 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry12 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry13 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry14 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry15 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 2
            DataEntry entry21 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry22 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry23 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 3
            DataEntry entry31 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry32 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry33 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                            e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry34 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry35 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry36 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                                e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry37 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            if (entry11 != null && entry12 != null && entry13 != null && entry14 != null && entry15 != null)
            {
                // Accumulate the values obtained during 1 hour (1 value = 1 minute --> 1hour = 60 minutes = 60 values)
                // We read 1 value every 10 minutes (60 minutes --> 6 values read)
                value1 += entry11.Value + entry12.Value + entry13.Value + entry14.Value + entry15.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }
            if (entry21 != null && entry22 != null && entry23 != null)
            {
                value2 += entry21.Value + entry22.Value + entry23.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }
            if (entry31 != null && entry32 != null && entry33 != null && entry34 != null && entry35 != null && entry36 != null && entry37 != null)
            {
                value3 += entry31.Value + entry32.Value + entry33.Value + entry34.Value + entry35.Value + entry36.Value + entry37.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (t == 50)
            {
                // Update Bars with data from Building 1:

                // Get the bar object 
                str = "BarDay" + day.ToString();
                barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

                // Mean number of people identified in the last hour in all floors
                // CHANGE: depending on day and values obtained for number of people
                float num = Math.Max((float)((value1 + rand.Next(-50, 20)) / (6 * 5 * 316)), 0); ; //+ rand.Next(-30, 30)) / (6 * 5 * 30));
                num *= daysBuilding1[day - 1];
                Building1.Add(num);
                // Change bar's material to opaque (color depends on its value)
                barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
                barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
                // Translate and scale bar
                barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
                barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

                // Store Building 2 and Building 3 values for later use:
                float num2 = Math.Max((float)((value2 + rand.Next(-200, 20)) / (6 * 3 * 902)), 0);
                num2 *= daysBuilding2[day - 1];
                Building2.Add(num2);
                float num3 = Math.Max((float)((value3 + rand.Next(-200, 20)) / (6 * 7 * 856)), 0);
                num3 *= daysBuilding3[day - 1];
                Building3.Add(num3);

                // Update hour
                hour += 1;
                value1 = 0;
                value2 = 0;
                value3 = 0;
            }

            // Look at values every 10 minutes (10 minutes = 10 values):
            time += 10;
            t += 10;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }
            // When t is higher than 6, an hour has passed and it needs to be updated
            if (t >= 60)
            {
                t = 0;
            }

            //if (time == 9240)
            //{
            //    DropdownHologramSelected(1);
            //    Debug.Log("Pass to building 2");
            //}

            // 1 second in virtual world = 5 minutes in real world
            yield return new WaitForSeconds((float)(2));
        }
        if (time == 10080)
        {

            Debug.Log("UPdate building1");
            //txt1.text = week_days[count_day] + dates[count_date] + month;
            //txt2.text = week_days[count_day + 1] + dates[count_date + 1] + month;
            //txt3.text = week_days[count_day + 2] + dates[count_date + 2] + month;
            //txt4.text = week_days[count_day + 3] + dates[count_date + 3] + month;
            //txt5.text = week_days[count_day + 4] + dates[count_date + 4] + month;
            //txt6.text = week_days[count_day + 5] + dates[count_date + 5] + month;
            //txt7.text = week_days[count_day + 6] + dates[count_date + 6] + month;
            //count_day = (count_day + 1) % 7;
            //count_date = (count_date + 1) % 19;
            //DropdownHologramSelected(0);
            StartCoroutine(Change2Building1(true));
        }
        yield break;
    }

    void CompletePreviousDays()
    {
        //int t = 0;
        int hour = 0;
        int day = 1;
        string str;
        GameObject barx = null;
        int type = 5;
        int floor = 2;
        //double value1 = 0;
        //double value2 = 0;
        //double value3 = 0;

        Hologram2.SetActive(false);
        Hologram3.SetActive(false);

        // For the previous 6 days
        while (day < 7 || (day == 7 && time < start_time + 1440 * 6))
        {
            if (day == 5 || day == 6)
            {
                type = 17;
            }
            // Get value Building 1
            DataEntry entry11 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry12 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 2 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry13 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 4 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry14 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 4 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry15 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 5 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 2
            DataEntry entry21 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry22 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry23 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 3
            DataEntry entry31 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry32 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry33 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                            e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry34 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry35 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry36 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                                e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry37 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                             e.ID_DataType == type && e.Data == "19/10/23" && e.Time == time % 1440);

            if (entry11 != null && entry12 != null && entry13 != null && entry14 != null && entry15 != null)
            {
                // Accumulate the values obtained during 1 hour (1 value = 1 minute --> 1hour = 60 minutes = 60 values)
                // We read 1 value every 10 minutes (60 minutes --> 6 values read)
                value1 += entry11.Value + entry12.Value + entry13.Value + entry14.Value + entry15.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }
            if (entry21 != null && entry22 != null && entry23 != null)
            {
                value2 += entry21.Value + entry22.Value + entry23.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }
            if (entry31 != null && entry32 != null && entry33 != null && entry34 != null && entry35 != null && entry36 != null && entry37 != null)
            {
                value3 += entry31.Value + entry32.Value + entry33.Value + entry34.Value + entry35.Value + entry36.Value + entry37.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }


            if (t == 50)
            {
                // Update Bars with data from Building 1:

                // Get the bar object 
                str = "BarDay" + day.ToString();
                barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

                // Disable its visibility
                barx.SetActive(false);
                // Mean number of people identified in the last hour in all floors
                // CHANGE: depending on day and values obtained for number of people
                float num = Math.Max((float)((value1 + rand.Next(-50, 20)) / (6 * 5 * 316)), 0); //+ rand.Next(-30, 30)) / (6 * 5 * 30));
                num *= daysBuilding1[day - 1];
                Building1.Add(num);
                // Change bar's material to opaque (color depends on its value)
                barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
                barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
                // Translate and scale bar
                barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
                barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

                // Store Building 2 and Building 3 values for later use:
                float num2 = Math.Max((float)((value2 + rand.Next(-200, 20)) / (6 * 3 * 902)), 0);
                num2 *= daysBuilding2[day - 1];
                Building2.Add(num2);
                float num3 = Math.Max((float)((value3 + rand.Next(-200, 20)) / (6 * 7 * 856)), 0);
                num3 *= daysBuilding3[day - 1];
                Building3.Add(num3);

                // Update hour
                hour += 1;
                value1 = 0;
                value2 = 0;
                value3 = 0;
            }

            // Look at values every 10 minutes (10 minutes = 10 values):
            time += 10;
            t += 10;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }

            // When t is higher than 6, an hour has passed and it needs to be updated
            if (t >= 60)
            {
                t = 0;
            }
        }

    }


    void EnableBarVisibility()
    {
        int hour = 0;
        int day = 1;
        string str;
        GameObject barx = null;

        while (day < 8)
        {
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;
            barx.SetActive(true);

            hour += 1;
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }
        }
    }

    void reScaleBarDay7()
    {
        int hour = 0;
        int day = 7;
        string str;
        GameObject barx = null;

        while (hour < 24)
        {
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;
            barx.transform.localScale = new Vector3((float)0.05, 0, (float)0.05);
            barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), 0, (float)0.4720002);

            hour += 1;
        }
    }

    IEnumerator Change2Building3()
    {

        Debug.Log("CHANGE2BUILDING3");
        //int t = 0;
        int hour = 0;
        int day = 1;
        string str;
        int floor;
        int floor2;
        GameObject barx = null;
        //double value1 = 0;
        //double value2 = 0;
        //double value3 = 0;
        int previous_days = Mathf.Min(Mathf.FloorToInt(time / 1440), 6);
        int starting_time = time % 1440;
        int initial_t = Building3.Count - previous_days * 24 - Mathf.FloorToInt(starting_time / 60);


        reScaleBarDay7();

        // Complete the previous days
        while (initial_t < Building3.Count)
        {
            // Get the bar object 
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

            // Disable its visibility
            barx.SetActive(false);

            // Get value from list
            float num = Building3[initial_t];

            // Change bar's material to opaque (color depends on its value)
            barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
            barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
            // Translate and scale bar
            barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
            barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

            // Update hour
            hour += 1;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }
            initial_t++;
        }

        EnableBarVisibility();
        if (time < 10080)
        {
            floor = 1;
            floor2 = 3;
        }
        else
        {
            floor = 2;
            floor2 = 4;
        }
        yield return new WaitForSeconds(2);
        // Update today's data
        //yield return new WaitForSeconds((float)(1));
        while (Hologram3.activeSelf)
        {
            // Get value Building 1
            DataEntry entry11 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry12 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry13 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry14 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry15 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 2
            DataEntry entry21 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry22 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry23 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            // Get value Building 3
            DataEntry entry31 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry32 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry33 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry34 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry35 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry36 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry37 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            if (entry11 != null && entry12 != null && entry13 != null && entry14 != null && entry15 != null)
            {
                // Accumulate the values obtained during 1 hour (1 value = 1 minute --> 1hour = 60 minutes = 60 values)
                // We read 1 value every 10 minutes (60 minutes --> 6 values read)
                value1 += entry11.Value + entry12.Value + entry13.Value + entry14.Value + entry15.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry21 != null && entry22 != null && entry23 != null)
            {
                value2 += entry21.Value + entry22.Value + entry23.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry31 != null && entry32 != null && entry33 != null && entry34 != null && entry35 != null && entry36 != null && entry37 != null)
            {
                value3 += entry31.Value + entry32.Value + entry33.Value + entry34.Value + entry35.Value + entry36.Value + entry37.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (t == 50)
            {
                // Update Bars with data from Building 1:

                // Get the bar object 
                str = "BarDay" + day.ToString();
                barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

                // Mean number of people identified in the last hour in all floors
                // CHANGE: depending on day and values obtained for number of people
                float num = Math.Max((float)((value3 + rand.Next(-200, 20)) / (6 * 7 * 856)), 0); //+ rand.Next(-30, 30)) / (6 * 5 * 30));
                num *= daysBuilding3[actual_day];
                Building3.Add(num);
                // Change bar's material to opaque (color depends on its value)
                barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
                barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
                // Translate and scale bar
                barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
                barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

                // Store Building 2 and Building 3 values for later use:
                float num1 = Math.Max((float)((value1 + rand.Next(-50, 20)) / (6 * 5 * 316)), 0);
                num1 *= daysBuilding1[actual_day];
                Building1.Add(num1);
                float num2 = Math.Max((float)((value2 + rand.Next(-200, 20)) / (6 * 3 * 902)), 0);
                num2 *= daysBuilding2[actual_day];
                Building2.Add(num2);

                value1 = 0;
                value2 = 0;
                value3 = 0;
                hour++;
            }

            // Look at values every 10 minutes (10 minutes = 10 values):
            time += 10;
            t += 10;

            // When t is higher than 6, an hour has passed and it needs to be updated
            if (t >= 60)
            {
                t = 0;
            }

            // 1 second in virtual world = 5 minutes in real world
            yield return new WaitForSeconds((float)(2));
            if (hour == 24)
            {
                UpdateGraphDays(3);
                hour = 0;
                actual_day = (actual_day + 1) % 7;
                yield return new WaitForSeconds(1);
            }
        }
        yield break;
    }

    IEnumerator Change2Building2()
    {

        Debug.Log("CHANGE2BUILDING2");
        //int t = 0;
        int hour = 0;
        int day = 1;
        string str;
        int floor;
        int floor2;
        GameObject barx = null;
        //double value1 = 0;
        //double value2 = 0;
        //double value3 = 0;
        int previous_days = Mathf.Min(Mathf.FloorToInt(time / 1440), 6);
        int starting_time = time % 1440;
        int initial_t = Building2.Count - previous_days * 24 - Mathf.FloorToInt(starting_time / 60);

        Debug.Log("initial_t " + initial_t.ToString());
        Debug.Log("prev days " + previous_days.ToString());
        Debug.Log("start_time " + starting_time.ToString());
        Debug.Log("List contains " + Building2.Count.ToString());
        reScaleBarDay7();

        // Complete the previous days
        while (initial_t < Building2.Count)
        {
            // Get the bar object 
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

            // Disable its visibility
            barx.SetActive(false);

            // Get value from list
            float num = Building2[initial_t];

            // Change bar's material to opaque (color depends on its value)
            barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
            barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
            // Translate and scale bar
            barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
            barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

            // Update hour
            hour += 1;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }
            initial_t++;
        }

        EnableBarVisibility();

        if (time < 10080)
        {
            floor = 1;
            floor2 = 3;
        }
        else
        {
            floor = 2;
            floor2 = 4;
        }
        // Update today's values
        //yield return new WaitForSeconds((float)(1));
        yield return new WaitForSeconds(2);
        while (Hologram2.activeSelf)
        {
            // Get value Building 1
            DataEntry entry11 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry12 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry13 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry14 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry15 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 2
            DataEntry entry21 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry22 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry23 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 3
            DataEntry entry31 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry32 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry33 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry34 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry35 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry36 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry37 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            if (entry11 != null && entry12 != null && entry13 != null && entry14 != null && entry15 != null)
            {
                // Accumulate the values obtained during 1 hour (1 value = 1 minute --> 1hour = 60 minutes = 60 values)
                // We read 1 value every 10 minutes (60 minutes --> 6 values read)
                value1 += entry11.Value + entry12.Value + entry13.Value + entry14.Value + entry15.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry21 != null && entry22 != null && entry23 != null)
            {
                value2 += entry21.Value + entry22.Value + entry23.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry31 != null && entry32 != null && entry33 != null && entry34 != null && entry35 != null && entry36 != null && entry37 != null)
            {
                value3 += entry31.Value + entry32.Value + entry33.Value + entry34.Value + entry35.Value + entry36.Value + entry37.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (t == 50)
            {
                // Update Bars with data from Building 1:

                // Get the bar object 
                str = "BarDay" + day.ToString();
                barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

                // Mean number of people identified in the last hour in all floors
                // CHANGE: depending on day and values obtained for number of people
                float num = Math.Max((float)((value2 + rand.Next(-200, 20)) / (6 * 3 * 902)), 0); //+ rand.Next(-30, 30)) / (6 * 5 * 30));
                num *= daysBuilding2[actual_day];
                Building2.Add(num);
                // Change bar's material to opaque (color depends on its value)
                barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
                barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
                // Translate and scale bar
                barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
                barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

                // Store Building 2 and Building 3 values for later use:
                float num1 = Math.Max((float)((value1 + rand.Next(-50, 20)) / (6 * 5 * 316)), 0);
                num1 *= daysBuilding1[actual_day];
                Building1.Add(num1);
                float num3 = Math.Max((float)((value3 + rand.Next(-200, 20)) / (6 * 7 * 856)), 0);
                num3 *= daysBuilding3[actual_day];
                Building3.Add(num3);

                value1 = 0;
                value2 = 0;
                value3 = 0;
                hour++;
            }

            // Look at values every 10 minutes (10 minutes = 10 values):
            time += 10;
            t += 10;

            // When t is higher than 6, an hour has passed and it needs to be updated
            if (t >= 60)
            {
                t = 0;
            }


            // 1 second in virtual world = 5 minutes in real world
            yield return new WaitForSeconds((float)(2));
            if (hour == 24)
            {
                UpdateGraphDays(2);
                hour = 0;
                actual_day = (actual_day + 1) % 7;
                yield return new WaitForSeconds(1);
            }
        }
        yield break;
    }

    IEnumerator Change2Building1(bool firstCall)
    {

        Debug.Log("CHANGE2BUILDING1");
        //int t = 0;
        int hour = 0;
        int day = 1;
        int floor = 1;
        int floor2;
        string str;
        GameObject barx = null;
        //double value1 = 0;
        //double value2 = 0;
        //double value3 = 0;
        int previous_days = Mathf.Min(Mathf.FloorToInt(time / 1440), 6);
        int starting_time = time % 1440;
        int initial_t = Building1.Count - previous_days * 24 - Mathf.FloorToInt(starting_time / 60);


        reScaleBarDay7();
        Debug.Log("1: Bar7 has been reescaled");
        Debug.Log("1: StartingTime: " + starting_time.ToString());
        Debug.Log("1: InitialT: " + initial_t.ToString());
        Debug.Log("1: Num. elements List = " + Building1.Count.ToString());
        Debug.Log("1: previous days: " + previous_days.ToString());
        Debug.Log("1: tryyyy: " + (Mathf.FloorToInt(starting_time / 60)).ToString());

        // Complete the previous days
        while (initial_t < Building1.Count)
        {
            // Get the bar object 
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

            // Disable its visibility
            barx.SetActive(false);

            // Get value from list
            float num = Building1[initial_t];

            // Change bar's material to opaque (color depends on its value)
            barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
            barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
            // Translate and scale bar
            barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
            barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

            // Update hour
            hour += 1;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }
            initial_t++;
        }
        // Update date axes if needed
        if (firstCall)
        {
            txt1.text = week_days[count_day % 7] + dates[count_date % 19] + month;
            txt2.text = week_days[(count_day + 1) % 7] + dates[(count_date + 1) % 19] + month;
            txt3.text = week_days[(count_day + 2) % 7] + dates[(count_date + 2) % 19] + month;
            txt4.text = week_days[(count_day + 3) % 7] + dates[(count_date + 3) % 19] + month;
            txt5.text = week_days[(count_day + 4) % 7] + dates[(count_date + 4) % 19] + month;
            txt6.text = week_days[(count_day + 5) % 7] + dates[(count_date + 5) % 19] + month;
            txt7.text = week_days[(count_day + 6) % 7] + dates[(count_date + 6) % 19] + month;
            count_day = (count_day + 1) % 7;
            count_date = (count_date + 1) % 19;
        }
        if (time < 10080)
        {
            floor = 1;
            floor2 = 3;
        }
        else
        {
            floor = 2;
            floor2 = 4;
        }

        Debug.Log("1: Bar7 has been reescaled");
        EnableBarVisibility();
        Debug.Log("1: All bars have been enabled");
        // Update today's values
        //yield return new WaitForSeconds((float)(1));
        yield return new WaitForSeconds(2);
        while (Hologram1.activeSelf)
        {
            // Get value Building 1
            DataEntry entry11 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry12 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry13 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry14 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry15 = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == floor2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 2
            DataEntry entry21 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry22 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry23 = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            // Get value Building 3
            DataEntry entry31 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry32 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry33 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry34 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry35 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry36 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);
            DataEntry entry37 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                             e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time % 1440);

            if (entry11 != null && entry12 != null && entry13 != null && entry14 != null && entry15 != null)
            {
                // Accumulate the values obtained during 1 hour (1 value = 1 minute --> 1hour = 60 minutes = 60 values)
                // We read 1 value every 10 minutes (60 minutes --> 6 values read)
                value1 += entry11.Value + entry12.Value + entry13.Value + entry14.Value + entry15.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry21 != null && entry22 != null && entry23 != null)
            {
                value2 += entry21.Value + entry22.Value + entry23.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (entry31 != null && entry32 != null && entry33 != null && entry34 != null && entry35 != null && entry36 != null && entry37 != null)
            {
                value3 += entry31.Value + entry32.Value + entry33.Value + entry34.Value + entry35.Value + entry36.Value + entry37.Value;
            }
            else
            {
                peopleText.text = "No data found";
            }

            if (t == 50)
            {
                // Update Bars with data from Building 1:

                // Get the bar object 
                str = "BarDay" + day.ToString();
                barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

                // Mean number of people identified in the last hour in all floors
                // CHANGE: depending on day and values obtained for number of people
                float num = Math.Max((float)((value1 + rand.Next(-50, 20)) / (6 * 5 * 316)), 0); //+ rand.Next(-30, 30)) / (6 * 5 * 30));
                num *= daysBuilding1[actual_day];
                Building1.Add(num);
                // Change bar's material to opaque (color depends on its value)
                barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
                barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
                // Translate and scale bar
                barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
                barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

                // Store Building 2 and Building 3 values for later use:
                float num2 = Math.Max((float)((value2 + rand.Next(-200, 20)) / (6 * 3 * 902)), 0);
                num2 *= daysBuilding2[actual_day];
                Building2.Add(num2);
                float num3 = Math.Max((float)((value3 + rand.Next(-200, 20)) / (6 * 7 * 856)), 0);
                num3 *= daysBuilding3[actual_day];
                Building3.Add(num3);

                value1 = 0;
                value2 = 0;
                value3 = 0;
                hour++;
            }

            // Look at values every 10 minutes (10 minutes = 10 values):
            time += 10;
            t += 10;

            // When t is higher than 6, an hour has passed and it needs to be updated
            if (t >= 60)
            {
                t = 0;
            }

            // 1 second in virtual world = 5 minutes in real world
            yield return new WaitForSeconds((float)(2));
            if (hour == 24)
            {
                UpdateGraphDays(1);
                hour = 0;
                actual_day = (actual_day + 1) % 7;
                yield return new WaitForSeconds(1);
            }
            Debug.Log("1: + 10 minutes ");
        }
        Debug.Log("1: Has finished");
        yield break;
    }

    void UpdateGraphDays(int building)
    {
        List<float> Building = new List<float>(); ;
        Debug.Log("UPDATEGRAPHDAYS");
        int day = 1;
        int previous_days = 6;
        int hour = 0;
        string str;
        GameObject barx;

        // Update Days_Axes
        Debug.Log("Update Date_Axes");
        Debug.Log("Week day:" + week_days[count_day].ToString());
        Debug.Log("countday: " + count_day.ToString());
        txt1.text = week_days[count_day % 7] + dates[count_date % 19] + month;
        txt2.text = week_days[(count_day + 1) % 7] + dates[(count_date + 1) % 19] + month;
        txt3.text = week_days[(count_day + 2) % 7] + dates[(count_date + 2) % 19] + month;
        txt4.text = week_days[(count_day + 3) % 7] + dates[(count_date + 3) % 19] + month;
        txt5.text = week_days[(count_day + 4) % 7] + dates[(count_date + 4) % 19] + month;
        txt6.text = week_days[(count_day + 5) % 7] + dates[(count_date + 5) % 19] + month;
        txt7.text = week_days[(count_day + 6) % 7] + dates[(count_date + 6) % 19] + month;
        count_day = (count_day + 1) % 7;
        count_date = (count_date + 1) % 19;
        //Update 3D Graph
        switch (building)
        {
            case 1:
                Building = Building1;
                break;
            case 2:
                Building = Building2;
                break;
            case 3:
                Building = Building3;
                break;
        }
        int initial_t = Building.Count - 144;
        Debug.Log("initial_t " + initial_t.ToString());

        // Complete the 6 previous days
        while (day <= previous_days)
        {
            // Get the bar object 
            str = "BarDay" + day.ToString();
            barx = GameObject.Find(str).transform.GetChild(hour).gameObject;

            // Disable its visibility
            barx.SetActive(false);

            // Get value from list
            float num = Building[initial_t];

            // Change bar's material to opaque (color depends on its value)
            barx.GetComponent<Renderer>().material.color = colors[Mathf.RoundToInt(Mathf.Min(num * 4, 4))];
            barx.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);
            // Translate and scale bar
            barx.transform.localScale = new Vector3((float)0.05, num, (float)0.05);
            barx.transform.localPosition = new Vector3((float)(-0.9300001 + 0.07 * hour), num / 2, (float)0.4720002);

            // Update hour
            hour += 1;

            // Hour == 24 means that a new day begins
            if (hour == 24)
            {
                hour = 0;
                day += 1;
            }

            initial_t++;
        }
        EnableBarVisibility();
        reScaleBarDay7();
        Debug.Log("End Udpating .....");
    }


    public void DropdownHologramSelected(int value)
    {
        if (value == 0)
        {
            Hologram1.SetActive(true);
            Hologram2.SetActive(false);
            Hologram3.SetActive(false);
            StopAllCoroutines();
            ValueAxes1.SetActive(true);
            ValueAxes2.SetActive(false);
            ValueAxes3.SetActive(false);
            StartCoroutine(Change2Building1(false));
        }
        if (value == 1)
        {
            Hologram1.SetActive(false);
            Hologram2.SetActive(true);
            Hologram3.SetActive(false);
            StopAllCoroutines();
            ValueAxes1.SetActive(false);
            ValueAxes2.SetActive(true);
            ValueAxes3.SetActive(false);
            StartCoroutine(Change2Building2());
        }
        if (value == 2)
        {
            Hologram1.SetActive(false);
            Hologram2.SetActive(false);
            Hologram3.SetActive(true);
            StopAllCoroutines();
            ValueAxes1.SetActive(false);
            ValueAxes2.SetActive(false);
            ValueAxes3.SetActive(true);
            StartCoroutine(Change2Building3());
        }

    }

}