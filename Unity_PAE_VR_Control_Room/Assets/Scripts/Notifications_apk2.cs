using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications_apk2 : MonoBehaviour
{
    private List<DataEntry> dataList;

    public DirectionalLight red_light_alarm;

    public GameObject alert_water;
    public GameObject water_cutoff;
    public Notifications_hologram holograma_notifications;

    private double threshold_w = 300;
    private double water;

    private bool activ_1 = false;
    private bool activ_2 = false;
    private bool alarm_off = false;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para la Apk2
        StartCoroutine(Apk_2());
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

    IEnumerator Apk_2()
    {
        int time = 480;

        while (true)
        {
            
            //Leemos water del Building 2 planta 2

            DataEntry w = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                                e.ID_DataType == 3 && e.Data == "19/10/23" && e.Time == time);

            water = w.Value;


            if (water >= threshold_w && activ_1 == false)
            {
                alert_water.SetActive(true);
                red_light_alarm.AlarmOnRed();
                holograma_notifications.waterLeakHolograma();
                activ_1 = true;
            }

            else if (water == 0 && activ_2 == false)
            {
                alert_water.SetActive(false);
                water_cutoff.SetActive(true);
                red_light_alarm.AlarmOnOrange();
                holograma_notifications.water_cutoffLeakHolograma();
                activ_2 = true;
            }

            else if (time >= 1381 && alarm_off == false)
            {
                water_cutoff.SetActive(false);
                red_light_alarm.AlarmOff();
                holograma_notifications.situacionNormal_floor(2, 2);
                alarm_off = true;
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
}
