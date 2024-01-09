/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;//nuevo
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;
using System.IO;// para leer el código de un fichero de texto
using static UnityEngine.EventSystems.EventTrigger;
using NUnit.Framework;
using Unity.VisualScripting;

public class Window_Graph : MonoBehaviour {

    private static Window_Graph instance;

    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashContainer;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    private List<IGraphVisualObject> graphVisualObjectList;
    //private GameObject tooltipGameObject;
    private List<RectTransform> yLabelList;


    public GameObject Hologram1, Hologram2, Hologram3;
    //public int currentFloor;

    // Cached values
    private List<float> valueList;
    
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;
    private float xSize;
    private bool startYScaleAtZero;

    private int time; //PARA QUE SEA UNIVERSAL
    private int index;

    private int holograma;//PARA SABER CUAL ACTIVO
    private int hologramaNew;

    private int floor;
    private int floorNew;


    private List<float> entryH1P1;
    private List<float> entryH1P2;
    private List<float> entryH1P3;
    private List<float> entryH1P4;
    private List<float> entryH1P5;

    private List<float> entryH2P1;
    private List<float> entryH2P2;
    private List<float> entryH2P3;

    private List<float> entryH3P1;
    private List<float> entryH3P2;
    private List<float> entryH3P3;
    private List<float> entryH3P4;
    private List<float> entryH3P5;
    private List<float> entryH3P6;
    private List<float> entryH3P7;




    private void Awake()
    { //void
        instance = this;
        // Grab base objects references
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashContainer = graphContainer.Find("dashContainer").GetComponent<RectTransform>();
        dashTemplateX = dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        //tooltipGameObject = graphContainer.Find("tooltip").gameObject;

        startYScaleAtZero = true;
        gameObjectList = new List<GameObject>();
        yLabelList = new List<RectTransform>();
        graphVisualObjectList = new List<IGraphVisualObject>();
        

        IGraphVisual lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.white, new Color(1, 1, 1, 1));
        //IGraphVisual barChartVisual = new BarChartVisual(graphContainer, Color.white, .8f);
        List<DataEntry>  datalist = LoadData();

        time = 410;// de 5 en 5 y el punto 15 es el 480: 480-14*5 = 410
        index = 0;
        Boolean inicio = true;
        valueList = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };

        entryH1P1 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH1P2 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH1P3 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH1P4 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH1P5 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };

        entryH2P1 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH2P2 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH2P3 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };

        entryH3P1 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P2 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P3 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P4 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P5 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P6 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
        entryH3P7 = new List<float>() { 20, 25, 23, 21, 23, 22, 20, 20, 21, 23, 24, 21, 22, 21, 20 };
    
        FunctionPeriodic.Create(() =>
        {
            if (inicio) {
                Inicializar(datalist);// probar que primero se veo como se hacen los 15 primeros (primero vacío)
                inicio = false;
            }
            else
            {
                Actualizar(datalist);
            }
           
            ShowGraph(valueList, lineGraphVisual, -1, (int _i) => ""+ (_i ), (float _f) => Mathf.RoundToInt(_f) + "ºC");

        }, 1f);//tiempo de espera 1 seg 
       
    }
   

    private List<DataEntry> LoadData()
    {
        //COGER DATOS DE JSON
        List<DataEntry> dataList;
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Data_Floor_Building");
        if (jsonTextAsset == null)
        {
            Debug.LogError("JSON file 'Data_Floor_Building' not found in Resources.");
           
        }

        DataList loadedDataList = JsonUtility.FromJson<DataList>("{\"items\":" + jsonTextAsset.text + "}"); // Crea una lista de datos desde el archivo JSON
        if (loadedDataList == null)
        {
            Debug.LogError("Failed to load data from JSON.");
            
        }
        //yield break;
        dataList = loadedDataList.items;
        return dataList;
    }

    private void Inicializar(List<DataEntry> dataList) // al principio se se�ala uno
    {
        //mirando
        DataEntry entry = null;

        for (int i = 0; i < 15; i++)
        {

            //edificio 1
            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH1P1[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 2 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH1P2[i] = Convert.ToSingle(entry.Value) ;

            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH1P3[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 4 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH1P4[i] = Convert.ToSingle(entry.Value);
           
            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 5 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH1P5[i] = Convert.ToSingle(entry.Value);

            //edificio 2
            entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH2P1[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH2P2[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH2P3[i] = Convert.ToSingle(entry.Value);

            //edificio 3
            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P1[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P2[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P3[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P4[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P5[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P6[i] = Convert.ToSingle(entry.Value);

            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            entryH3P7[i] = Convert.ToSingle(entry.Value);

            //GUARDAR EL FLOOR Y EL HOLOGRAMA SELECCIONADOS
            floor = TemperatureDisplay.currentFloor;

            if (Hologram1.activeSelf)
            {
                holograma = 1;
                entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            }
            else if (Hologram2.activeSelf)
            {
                holograma = 2;
                entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            }
            else if (Hologram3.activeSelf)
            {
                holograma = 3;
                entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                           e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
            }

            valueList[i] = Convert.ToSingle(entry.Value);//te guardas el seleccionado


            time += 5; // mostra 1 minut de cada 10
        }
    }

    
    private void Actualizar(List<DataEntry> dataList)
    {
        DataEntry entry = null;
        time += 5; // mostra 1 minut de cada 10

        if (time >= 1439)
        {
            time = 0;
        }

        for (int i = 0; i < 14; i++)//desplazar dato m�s antiguo
        {
            entryH1P1[i] = entryH1P1[i + 1];
            entryH1P2[i] = entryH1P2[i + 1];
            entryH1P3[i] = entryH1P3[i + 1];
            entryH1P4[i] = entryH1P4[i + 1];
            entryH1P5[i] = entryH1P5[i + 1];

            entryH2P1[i] = entryH2P1[i + 1];
            entryH2P2[i] = entryH2P2[i + 1];
            entryH2P3[i] = entryH2P3[i + 1];

            entryH3P1[i] = entryH3P1[i + 1];
            entryH3P2[i] = entryH3P2[i + 1];
            entryH3P3[i] = entryH3P3[i + 1];
            entryH3P4[i] = entryH3P4[i + 1];
            entryH3P5[i] = entryH3P5[i + 1];
            entryH3P6[i] = entryH3P6[i + 1];
            entryH3P7[i] = entryH3P7[i + 1];

            valueList[i] = valueList[i + 1];
        }

        //edificio 1
        entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 1 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH1P1[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 2 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH1P2[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 3 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH1P3[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 4 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH1P4[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == 5 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH1P5[14] = Convert.ToSingle(entry.Value);

        //edificio 2
        entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 1 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH2P1[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 2 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH2P2[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == 3 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH2P3[14] = Convert.ToSingle(entry.Value);

        //edificio 3
        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 1 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P1[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 2 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P2[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 3 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P3[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 4 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P4[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 5 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P5[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 6 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P6[14] = Convert.ToSingle(entry.Value);

        entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == 7 &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        entryH3P7[14] = Convert.ToSingle(entry.Value);


        if (Hologram1.activeSelf)
        {
            hologramaNew = 1;
            entry = dataList.Find(e => e.ID_Building == 1 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);

        }
        else if (Hologram2.activeSelf)
        {
            hologramaNew = 2;
            entry = dataList.Find(e => e.ID_Building == 2 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        }
        else if (Hologram3.activeSelf)
        {
            hologramaNew = 3;
            entry = dataList.Find(e => e.ID_Building == 3 && e.ID_Floor == TemperatureDisplay.currentFloor &&
                                       e.ID_DataType == 1 && e.Data == "19/10/23" && e.Time == time);
        }



        if (holograma != hologramaNew || TemperatureDisplay.currentFloor != floor)
        {
            floor = TemperatureDisplay.currentFloor;
            holograma = hologramaNew;

            if (holograma == 1 && floor == 1)
            {
                valueList = entryH1P1;
                
            }
            else if (holograma == 1 && floor == 2)
            {
               valueList = entryH1P2;
                

            }
            else if (holograma == 1 && floor == 3)
            {
                valueList = entryH1P3;

            }
            else if (holograma == 1 && floor == 4)
            {

                valueList = entryH1P4;       

            }
            else if (holograma == 1 && floor == 5)
            {

                valueList = entryH1P5;

            }
            else if (holograma == 2 && floor == 1)
            {
                
                  valueList = entryH2P1;

            }
            else if (holograma == 2 && floor == 2)
            {
                
                  valueList = entryH2P2;
                

            }
            else if (holograma == 2 && floor == 3)
            {
               
                  valueList = entryH2P3;
                

            }
            else if (holograma == 3 && floor == 1)
            {
                
                  valueList = entryH3P1;

            }
            else if (holograma == 3 && floor == 2)
            {
                valueList = entryH3P2;
                

            }
            else if (holograma == 3 && floor == 3)
            {
                valueList = entryH3P3;
                

            }
            else if (holograma == 3 && floor == 4)
            {
                valueList = entryH3P4;
               

            }
            else if (holograma == 3 && floor == 5)
            {
                valueList = entryH3P5;
                

            }
            else if (holograma == 3 && floor == 6)
            {

                valueList = entryH3P6;

            }
            else if (holograma == 3 && floor == 7)
            {
                valueList = entryH3P7; 
            }

        }
        else
        {
            UpdateValue(14, Convert.ToSingle(entry.Value));
        }    
    }


    private void SetGetAxisLabelX(Func<int, string> getAxisLabelX) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);
    }

    private void SetGetAxisLabelY(Func<float, string> getAxisLabelY) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);
    }
   
    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void ShowGraph(List<float> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        this.valueList = valueList;
        this.graphVisual = graphVisual;
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        if (maxVisibleValueAmount <= 0) {
            // Show all if no amount specified
            maxVisibleValueAmount = valueList.Count;
        }
        if (maxVisibleValueAmount > valueList.Count) {
            // Validate the amount to show the maximum
            maxVisibleValueAmount = valueList.Count;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Math.Round(_f,2).ToString(); };
        }

        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList) {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();
        
        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum=18, yMaximum=52;
        //CalculateYScale(out yMinimum, out yMaximum);

        // Set the distance between each point on the graph 
        xSize = graphWidth / (maxVisibleValueAmount + 1);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            //string tooltipText = getAxisLabelY(valueList[i]);
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize);// tooltipText);
            graphVisualObjectList.Add(graphVisualObject);

            // Duplicate the x label template
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition+14f , 0f);//xPosition + 3.3f, 0f);4f  //-20f
            labelX.GetComponent<TextMeshProUGUI>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);
            
            // Duplicate the x dash template
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(dashContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition-5f, -0.5f);//, -0.5f);
            gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        // Set up separators on the y axis
        int separatorCount = 17;//del 10 al 30
        int numsep=0;
        for (int i = 0; i <= separatorCount; i++) {
            // Duplicate the label template
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(100f, normalizedValue * graphHeight+6f);//-1f//-0.4// * graphHeight+2f
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(yMinimum + numsep);//getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            yLabelList.Add(labelY);
            gameObjectList.Add(labelY.gameObject);

            // Duplicate the dash template
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(dashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(20f, normalizedValue * graphHeight);//-2f
            gameObjectList.Add(dashY.gameObject);

            numsep += 2;//para ir de 2 en 2
        }
    }

    private void UpdateValue(int index, float value) {
        float yMinimumBefore, yMaximumBefore;
        CalculateYScale(out yMinimumBefore, out yMaximumBefore);

        valueList[index] = value;// se cambia el valor en la lista de 15

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
  
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum) {
        // Identify y Min and Max values
        yMaximum = 0;//valueList[0];
        yMinimum = 0;// valueList[0];
        
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float value = valueList[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        if (startYScaleAtZero) {
            yMinimum = 0f; // Start the graph at zero
        }
    }



    /*
     * Interface definition for showing visual for a data point
     * */
    private interface IGraphVisual {

        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth);// string tooltipText);
        void CleanUp();

    }

    /*
     * Represents a single Visual Object in the graph
     * */
    private interface IGraphVisualObject {

        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth); //string tooltipText);
        void CleanUp();

    }


   
    /*
     * Displays data points as a Line Graph
     * */
    private class LineGraphVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor) {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastLineGraphVisualObject = null;
        }

        public void CleanUp() {
            lastLineGraphVisualObject = null;
        }


        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth)//string tooltipText) 
        {
            GameObject dotGameObject = CreateDot(graphPosition);


            GameObject dotConnectionGameObject = null;
            if (lastLineGraphVisualObject != null) {
                dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            
            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, lastLineGraphVisualObject);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth);// tooltipText);
            
            lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        private GameObject CreateDot(Vector2 anchoredPosition) {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(2, 2);//(3,3) se hace una rayita donde el sitio del punto
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            
            // Add Button_UI Component which captures UI Mouse Events
            Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            gameObject.GetComponent<Image>().raycastTarget = false;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 0.5f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }


        public class LineGraphVisualObject : IGraphVisualObject {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject dotGameObject;
            private GameObject dotConnectionGameObject;
            private LineGraphVisualObject lastVisualObject;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, LineGraphVisualObject lastVisualObject) {
                this.dotGameObject = dotGameObject;
                this.dotConnectionGameObject = dotConnectionGameObject;
                this.lastVisualObject = lastVisualObject;

                if (lastVisualObject != null) {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth)// string tooltipText)
            {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();
                
                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp() {
                Destroy(dotGameObject);
                Destroy(dotConnectionGameObject);
            }

            public Vector2 GetGraphPosition() {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection() {// intentar dhacer más pequeño
                if (dotConnectionGameObject != null) {
                    RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 2f);//3f?
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }

        }

    }

}
