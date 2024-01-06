using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeDetectorDisplay : MonoBehaviour
{
    public Text smokedetectorText;
    private List<DataEntry2> dataList;
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;
    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para actualizar la temperatura
        StartCoroutine(UpdateSmokeDetector());
    }

    IEnumerator LoadDataFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Building_Data");
        if (jsonTextAsset == null)
        {
            Debug.LogError("JSON file 'Building_Data' not found in Resources.");
            yield break;
        }
        
        DataList2 loadedDataList = JsonUtility.FromJson<DataList2>("{\"items\":" + jsonTextAsset.text + "}"); // Crea una lista de datos desde el archivo JSON
        if (loadedDataList == null)
        {
            Debug.LogError("Failed to load data from JSON.");
            yield break;
        }

        dataList = loadedDataList.items;
    }

    IEnumerator UpdateSmokeDetector()
    {
        int time = 0; 

        while (true)
        {
            if (Hologram1.activeSelf){
                DataEntry2 entry = dataList.Find(e => e.ID_Building == 1 &&
                                                 e.ID_TypeData == 15 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
            {
                smokedetectorText.text = "" + entry.Value;
            }
            else
            {
                smokedetectorText.text = "No data found";
            }
            }
            else if (Hologram2.activeSelf){
                DataEntry2 entry = dataList.Find(e => e.ID_Building == 2 &&
                                                 e.ID_TypeData == 15 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
                {
                    smokedetectorText.text = "" + entry.Value;
                }
                else
                {
                    smokedetectorText.text = "No data found";
                }
            }
            else if (Hologram3.activeSelf){
                DataEntry2 entry = dataList.Find(e => e.ID_Building == 3 &&
                                                 e.ID_TypeData == 15 && e.Data == "19/10/23" && e.Time == time);
                if (entry != null)
                {
                    smokedetectorText.text = "" + entry.Value;
                }
                else
                {
                    smokedetectorText.text = "No data found";
                }
            }
            // Busca la entrada correspondiente en la lista de datos cargados
            //PARA QUE NO SE ACABEN LOS DATOS: 
            if (time >= 1434)
            {
                time = 0;
            }
            time += 5; // mostra 1 minut de cada 10 

            yield return new WaitForSeconds(1); // Espera 1 segundos (1 segon son 10 minuts)
        }
    }
}