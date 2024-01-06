using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataEntry2
{
    public string Data;
    public int ID_Building;
    public int ID_Data_Building;
    public int ID_TypeData;
    public int Time;
    public double Value;
}

[Serializable]
public class DataList2
{
    public List<DataEntry2> items;
}

