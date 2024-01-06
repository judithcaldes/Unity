using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureDisplay : MonoBehaviour
{
    public Text temperatureText;
    private List<DataEntry> dataList;
    public GameObject Hologram1, Hologram2, Hologram3;

    //Building 1
    public Button button1_b1_f1, button2_b1_f1, button3_b1_f1, button4_b1_f1; // Botones del primer piso del primer edificio
    public Button button1_b1_f2, button2_b1_f2, button3_b1_f2, button4_b1_f2; // Botones del segundo piso del primer edificio
    public Button button1_b1_f3, button2_b1_f3, button3_b1_f3, button4_b1_f3; // Botones del tercer piso del primer edificio
    public Button button1_b1_f4, button2_b1_f4, button3_b1_f4, button4_b1_f4; // Botones del cuarto piso del primer edificio
    public Button button1_b1_f5, button2_b1_f5, button3_b1_f5, button4_b1_f5; // Botones del quinto piso del primer edificio

    //Building 2
    public Button button1_b2_f1, button2_b2_f1, button3_b2_f1, button4_b2_f1; // Botones del primer piso del segundo edificio
    public Button button1_b2_f2, button2_b2_f2, button3_b2_f2, button4_b2_f2; // Botones del segundo piso del segundo edificio
    public Button button1_b2_f3, button2_b2_f3, button3_b2_f3, button4_b2_f3; // Botones del tercer piso del segundo edificio

    //Building 3
    public Button button1_b3_f1, button2_b3_f1, button3_b3_f1, button4_b3_f1; // Botones del primer piso del tercer edificio
    public Button button1_b3_f2, button2_b3_f2, button3_b3_f2, button4_b3_f2; // Botones del segundo piso del tercer edificio
    public Button button1_b3_f3, button2_b3_f3, button3_b3_f3, button4_b3_f3; // Botones del tercer piso del tercer edificio
    public Button button1_b3_f4, button2_b3_f4, button3_b3_f4, button4_b3_f4; // Botones del cuarto piso del tercer edificio
    public Button button1_b3_f5, button2_b3_f5, button3_b3_f5, button4_b3_f5; // Botones del quinto piso del tercer edificio
    public Button button1_b3_f6, button2_b3_f6, button3_b3_f6, button4_b3_f6; // Botones del sexto piso del tercer edificio
    public Button button1_b3_f7, button2_b3_f7, button3_b3_f7, button4_b3_f7; // Botones del septimo piso del tercer edificio


    public static int currentFloor = 1; // Valor inicial de ID_Floor
    public bool Air = false;
    public bool Heat = false;
    public bool HeatRecienApagado = false;
    public bool ACRecienApagado = false;
    public double temp;
    public int i = 1;
    private IEnumerator Start()
    {
        // Carga los datos desde el archivo JSON
        yield return StartCoroutine(LoadDataFromJson());

        // Inicia la Coroutine para actualizar la temperatura
        StartCoroutine(UpdateTemperature());

        //Building 1
        button1_b1_f1.onClick.AddListener(() => SetFloor(1));
        button2_b1_f1.onClick.AddListener(() => SetFloor(1));
        button3_b1_f1.onClick.AddListener(() => SetFloor(1));
        button4_b1_f1.onClick.AddListener(() => SetFloor(1));

        button1_b1_f2.onClick.AddListener(() => SetFloor(2));
        button2_b1_f2.onClick.AddListener(() => SetFloor(2));
        button3_b1_f2.onClick.AddListener(() => SetFloor(2));
        button4_b1_f2.onClick.AddListener(() => SetFloor(2));

        button1_b1_f3.onClick.AddListener(() => SetFloor(3));
        button2_b1_f3.onClick.AddListener(() => SetFloor(3));
        button3_b1_f3.onClick.AddListener(() => SetFloor(3));
        button4_b1_f3.onClick.AddListener(() => SetFloor(3));

        button1_b1_f4.onClick.AddListener(() => SetFloor(4));
        button2_b1_f4.onClick.AddListener(() => SetFloor(4));
        button3_b1_f4.onClick.AddListener(() => SetFloor(4));
        button4_b1_f4.onClick.AddListener(() => SetFloor(4));

        button1_b1_f5.onClick.AddListener(() => SetFloor(5));
        button2_b1_f5.onClick.AddListener(() => SetFloor(5));
        button3_b1_f5.onClick.AddListener(() => SetFloor(5));
        button4_b1_f5.onClick.AddListener(() => SetFloor(5));

        //Building 2
        button1_b2_f1.onClick.AddListener(() => SetFloor(1));
        button2_b2_f1.onClick.AddListener(() => SetFloor(1));
        button3_b2_f1.onClick.AddListener(() => SetFloor(1));
        button4_b2_f1.onClick.AddListener(() => SetFloor(1));

        button1_b2_f2.onClick.AddListener(() => SetFloor(2));
        button2_b2_f2.onClick.AddListener(() => SetFloor(2));
        button3_b2_f2.onClick.AddListener(() => SetFloor(2));
        button4_b2_f2.onClick.AddListener(() => SetFloor(2));

        button1_b2_f3.onClick.AddListener(() => SetFloor(3));
        button2_b2_f3.onClick.AddListener(() => SetFloor(3));
        button3_b2_f3.onClick.AddListener(() => SetFloor(3));
        button4_b2_f3.onClick.AddListener(() => SetFloor(3));

        //Building 3
        button1_b3_f1.onClick.AddListener(() => SetFloor(1));
        button2_b3_f1.onClick.AddListener(() => SetFloor(1));
        button3_b3_f1.onClick.AddListener(() => SetFloor(1));
        button4_b3_f1.onClick.AddListener(() => SetFloor(1));

        button1_b3_f2.onClick.AddListener(() => SetFloor(2));
        button2_b3_f2.onClick.AddListener(() => SetFloor(2));
        button3_b3_f2.onClick.AddListener(() => SetFloor(2));
        button4_b3_f2.onClick.AddListener(() => SetFloor(2));

        button1_b3_f3.onClick.AddListener(() => SetFloor(3));
        button2_b3_f3.onClick.AddListener(() => SetFloor(3));
        button3_b3_f3.onClick.AddListener(() => SetFloor(3));
        button4_b3_f3.onClick.AddListener(() => SetFloor(3));

        button1_b3_f4.onClick.AddListener(() => SetFloor(4));
        button2_b3_f4.onClick.AddListener(() => SetFloor(4));
        button3_b3_f4.onClick.AddListener(() => SetFloor(4));
        button4_b3_f4.onClick.AddListener(() => SetFloor(4));

        button1_b3_f5.onClick.AddListener(() => SetFloor(5));
        button2_b3_f5.onClick.AddListener(() => SetFloor(5));
        button3_b3_f5.onClick.AddListener(() => SetFloor(5));
        button4_b3_f5.onClick.AddListener(() => SetFloor(5));

        button1_b3_f6.onClick.AddListener(() => SetFloor(6));
        button2_b3_f6.onClick.AddListener(() => SetFloor(6));
        button3_b3_f6.onClick.AddListener(() => SetFloor(6));
        button4_b3_f6.onClick.AddListener(() => SetFloor(6));

        button1_b3_f7.onClick.AddListener(() => SetFloor(7));
        button2_b3_f7.onClick.AddListener(() => SetFloor(7));
        button3_b3_f7.onClick.AddListener(() => SetFloor(7));
        button4_b3_f7.onClick.AddListener(() => SetFloor(7));

    }

    IEnumerator LoadDataFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Data_Floor_Building");
        if (jsonTextAsset == null)
        {
            Debug.LogError("JSON file 'Data_Floor_Building' not found in Resources.");
            yield break;
        }

        DataList loadedDataList = JsonUtility.FromJson<DataList>("{\"items\":" + jsonTextAsset.text + "}");
        if (loadedDataList == null)
        {
            Debug.LogError("Failed to load data from JSON.");
            yield break;
        }

        dataList = loadedDataList.items;
    }

    IEnumerator UpdateTemperature()
    {
        int time = 480;

        while (true)
        {


            if (Hologram1.activeSelf)
            {
                DataEntry entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
                temp = entry.Value;
                if (entry != null)
                {
                    if (Air == false && Heat == false)
                    {
                        if (HeatRecienApagado == true)
                        {
                            while(temp > entry.Value)
                            {
                                temp = temp - 0.1;
                                temperatureText.text = "" + temp;
                            }
                            temperatureText.text = "" + temp;
                        }
                        else if (ACRecienApagado == true)
                        {
                            while (temp > entry.Value)
                            {
                                temp = temp - 0.1;
                                temperatureText.text = "" + temp;
                            }
                            temperatureText.text = "" + temp;
                        }

                        else
                        {
                            temperatureText.text = "" + temp;
                        }
                        
                        
                        //temp = entry.Value;
                        
                    }

                    else if (Air == true && Heat == false )
                    {
                        if(temp > 16.0)
                        {
                            //temp = entry.Value;
                            temp = temp - i * 0.1;
                            temperatureText.text = "" + temp;
                            i++;
                        }
                        
                    }
                    else if (Heat == true && Air == false  )
                    {
                        if(temp < 25.0)
                        {
                            //temp = entry.Value;
                            temp = temp + i * 0.1;
                            temperatureText.text = "" + temp;
                            i++;
                        }
                    }
                }
            }
            else if (Hologram2.activeSelf)
            {
                DataEntry entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
                temp = entry.Value;
                if (entry != null)
                {
                    if (Air == false && Heat == false)
                    {
                        //temp = entry.Value;
                        temperatureText.text = "" + temp;
                    }
                    else if (Air == true && Heat == false && temp > 16.0)
                    {

                        //temp = entry.Value;
                        temp = temp - i * 0.1;
                        temperatureText.text = "" + temp;
                        i++;
                    }
                    else if (Heat == true && Air == false && temp < 25.0)
                    {

                        //temp = entry.Value;
                        temp = temp + i * 0.1;
                        temperatureText.text = "" + temp;
                        i++; ;
                    }
                }
            }
            else if (Hologram3.activeSelf)
            {
                DataEntry entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
                temp = entry.Value;
                if (entry != null)
                {
                    if (Air == false && Heat == false)
                    {
                        //temp = entry.Value;
                        temperatureText.text = "" + temp;
                    }

                    else if (Air == true && Heat == false && temp > 16.0)
                    {
                        //temp = entry.Value;
                        temp = temp - i * 0.1;
                        temperatureText.text = "" + temp;
                        i++; ;
                    }
                    else if (Heat == true && Air ==false && temp < 25.0)
                    {

                        //temp = entry.Value;
                        temp = temp + i * 0.1;
                        temperatureText.text = "" + temp;
                        i++;
                    }
                }
            }
            else
            {
                temperatureText.text = "No data found";
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


public void SetFloor(int floor)
    {
        currentFloor = floor;
    }

    public void AirON()
    {
        Air = true;
        i = 1;
        Heat = false;
        ACRecienApagado = false;
        HeatRecienApagado = false;
    }

    public void AirOFF()
    {
        Air = false;
        ACRecienApagado = true;

    }

    public void HeatOFF()
    {
        Heat = false;
        HeatRecienApagado=true;
    }
    public void HeatON()
    {
        Heat = true;
        Air = false;
        i = 1;
        ACRecienApagado = false;
        HeatRecienApagado = false;
    }
}