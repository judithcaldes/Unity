using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications_apk3 : MonoBehaviour
{
    private List<DataEntry> dataList;

    public DirectionalLight red_light_alarm;
    public Notifications_hologram holograma_notifications;

    public GameObject notif_cleaning;

    private double threshold_people_4 = 600;
    private bool notif_activ_4 = false;

    public GameObject alert_security;
    public GameObject update_security;

    private double threshold_people_2 = 720;

    private double people_4;
    private double people_2;
    private bool activ_1 = false;
    private bool notif_activ_2 = false;


    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para la Apk3
        StartCoroutine(Apk_3());

        // Inicia la Coroutine para el final de la Apk3
        StartCoroutine(Apk_3_final());
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

    IEnumerator Apk_3()
    {
        int time = 480;

        while (true)
        {

            //Leemos people del Building 3 planta 4

            DataEntry p4 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                                e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time);

            people_4 = p4.Value;

            if (people_4 <= threshold_people_4 && time >= 810 && notif_activ_4 == false)
            {
                notif_cleaning.SetActive(true);
                red_light_alarm.AlarmOnGreen();
                notif_activ_4 = true;                
            }

            else if (time >= 990)
            {
                notif_cleaning.SetActive(false);
                red_light_alarm.AlarmOff();

            }

            //Leemos people del Building 3 planta 2

            DataEntry p2 = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                                e.ID_DataType == 5 && e.Data == "19/10/23" && e.Time == time);

            people_2 = p2.Value;

            if (people_2 > threshold_people_2 && time >= 960 && activ_1 == false)
            {
                alert_security.SetActive(true);
                red_light_alarm.AlarmOnRed();
                holograma_notifications.peopleAlarmHolograma();
                activ_1 = true;
            }

            else if (people_2 <= threshold_people_2 && notif_activ_2 == false && activ_1 == true)
            {
                update_security.SetActive(true);
                alert_security.SetActive(false);
                red_light_alarm.AlarmOnGreenDark();
                holograma_notifications.situacionNormal_floor(3, 2);
                notif_activ_2 = true;
            }

            else if (time >= 1150)
            {
                update_security.SetActive(false);
                red_light_alarm.AlarmOff();

            }


            // Busca la entrada correspondiente en la lista de datos cargados

            //PARA QUE NO SE ACABEN LOS DATOS: 
            if (time >= 1434)
            {
                time = 0;
            }


            time += 5; // mostra 1 minut de cada 10 
            yield return new WaitForSeconds(1); // Espera 1 segundo (1 segon son 10 minuts)
        }

        
    }
    
    IEnumerator Apk_3_final()
    {
        if (notif_activ_4 == true)
        {
            yield return new WaitForSeconds(30f);
            notif_cleaning.SetActive(false);
        }

        if (notif_activ_2 == true)
        {
            yield return new WaitForSeconds(30f);
            update_security.SetActive(false);
        }
    }
}