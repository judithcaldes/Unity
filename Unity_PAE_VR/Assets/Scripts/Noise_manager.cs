using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise_manager : MonoBehaviour
{
    public GameObject verde1;
    public GameObject verde2;
    public GameObject verde3;
    public GameObject verde4;
    public GameObject verde5;
    public GameObject naranja1;
    public GameObject naranja2;
    public GameObject rojo;

    private List<DataEntry> dataList;
    private double noise;

    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;

    //50 y 83,5


    // Start is called before the first frame update

    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para la Apk2
        StartCoroutine(Noise());

        verde1.SetActive(true);
        verde2.SetActive(false);
        verde3.SetActive(false);
        verde4.SetActive(false);
        verde5.SetActive(false);
        naranja1.SetActive(false);
        naranja2.SetActive(false);
        rojo.SetActive(false);
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

    IEnumerator Noise()
    {
        int time = 480;

        while (true)
        {

            if (Hologram1.activeSelf)
            {
                DataEntry n = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 11 && e.Data == "19/10/23" && e.Time == time);

                noise = n.Value;


            }
            else if (Hologram2.activeSelf)
            {
                DataEntry n = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 11 && e.Data == "19/10/23" && e.Time == time);
                noise = n.Value;


            }
            else if (Hologram3.activeSelf)
            {
                DataEntry n = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 11 && e.Data == "19/10/23" && e.Time == time);
                noise = n.Value;

            }

            verde1.SetActive(false);
            verde2.SetActive(false);
            verde3.SetActive(false);
            verde4.SetActive(false);
            verde5.SetActive(false);
            naranja1.SetActive(false);
            naranja2.SetActive(false);
            rojo.SetActive(false);
            if (noise > 45)
            {
                verde1.SetActive(true);
                if (noise > 55)
                {
                    verde2.SetActive(true);
                    if (noise > 60)
                    {
                        verde3.SetActive(true);
                        if (noise > 64)
                        {
                            verde4.SetActive(true);
                            if (noise > 67)
                            {
                                verde5.SetActive(true);
                                if (noise > 70)
                                {
                                    naranja1.SetActive(true);
                                    if (noise > 75)
                                    {
                                        naranja2.SetActive(true);
                                        if (noise > 80)
                                        {
                                            rojo.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // Busca la entrada correspondiente en la lista de datos cargados

            //PARA QUE NO SE ACABEN LOS DATOS: (TEST)
            if (time >= 1434)
            {
                time = 0;
            }


            time += 5; // mostra 1 minut de cada 10
            yield return new WaitForSeconds(1); // Espera 1 segundo (1 segon son 10 minuts)
        }
    }
}

