using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataEntry
{
    public string Data;
    public int ID_Building;
    public int ID_Data;
    public int ID_DataType;
    public int ID_Floor;
    public int Time;
    public double Value;
}

[Serializable]
public class DataList
{
    public List<DataEntry> items;
}
