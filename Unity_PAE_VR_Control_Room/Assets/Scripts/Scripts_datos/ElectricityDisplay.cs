using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityDisplay : MonoBehaviour
{
    public Text electricityText;
    private List<DataEntry> dataList;
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;
    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para actualizar la temperatura
        StartCoroutine(UpdateElectricity());
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

    IEnumerator UpdateElectricity()
    {
        int time = 480; 

        while (true)
        {
            if (Hologram1.activeSelf){
                DataEntry entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 2 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
            {
                electricityText.text = "" + entry.Value;
            }
            else
            {
                electricityText.text = "No data found";
            }
            }
            else if (Hologram2.activeSelf){
                DataEntry entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 2 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
                {
                    electricityText.text = "" + entry.Value;
                }
                else
                {
                    electricityText.text = "No data found";
                }
            }
            else if (Hologram3.activeSelf){
                DataEntry entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                                 e.ID_DataType == 2 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
                {
                    electricityText.text = "" + entry.Value;
                }
                else
                {
                    electricityText.text = "No data found";
                }
            }
            // Para que no se acaben los datos
            if (time >= 1434)
            {
                time = 0;
            }
            time += 5; // mostra 1 minut de cada 10 

            yield return new WaitForSeconds(1); // Espera 1 segundos (1 segon son 10 minuts)
        }
    }
}