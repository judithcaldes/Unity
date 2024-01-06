using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectFloor : MonoBehaviour
{
    public GameObject Hologram1;
    public Material Transparent_green;
    public Material Transparent_blue; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaterial(int index)
    {
        for (int i = 1; i<=7; i++)
        {
            if(i == index)
            {
                //HologramList[(int)index[0]].transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_green;
                Hologram1.transform.GetChild(i-1).GetComponent<Renderer>().material = Transparent_green;
            }
            else
            {
                //HologramList[(int)index[1]].transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_blue;
                Hologram1.transform.GetChild(i-1).GetComponent<Renderer>().material = Transparent_blue;
            }
        }
    }
}
