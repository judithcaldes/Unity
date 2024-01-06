using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_selected : MonoBehaviour
{
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;
    public GameObject Building1;
    public GameObject Building2;
    public GameObject Building3;
    public Material Transparent_green;
    public Material Transparent_blue;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 6; i++)
        {
            if (i < 5)
            {
                Hologram1.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_green;
            }
            if (i < 3)
            {
                Hologram2.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
            }
            Hologram3.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
        }

    }

    private void Update()
    {
        if (Building1.activeSelf)
        {
            for (int i = 0; i <= 6; i++)
            {
                if (i < 5)
                {
                    Hologram1.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_green;
                }
                if (i < 3)
                {
                    Hologram2.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
                }
                Hologram3.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
            }
        }
        if (Building2.activeSelf)
        {
            for (int i = 0; i <= 6; i++)
            {
                if (i < 5)
                {
                    Hologram1.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
                }
                if (i < 3)
                {
                    Hologram2.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_green;
                }
                Hologram3.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
            }
        }
        if (Building3.activeSelf)
        {
            for (int i = 0; i <= 6; i++)
            {
                if (i < 5)
                {
                    Hologram1.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
                }
                if (i < 3)
                {
                    Hologram2.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_blue;
                }
                Hologram3.transform.GetChild(i).GetComponent<Renderer>().material = Transparent_green;
            }
        }
    }

    
}