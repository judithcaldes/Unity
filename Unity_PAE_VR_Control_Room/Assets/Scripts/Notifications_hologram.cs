using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications_hologram : MonoBehaviour
{

    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;
    public Material Transparent_green;
    public Material Transparent_blue;
    public Material Transparent_red;
    public Material Transparent_orange;
    Coroutine hologramNotificationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator hologramaParpadeo(GameObject holograma, Material colorNotificacion)
    {
        while (true)
        {
            if(holograma.activeSelf == true)
            {
                for (int i = 1; i <= 5; i++)
                {
                    holograma.transform.GetChild(i - 1).GetComponent<Renderer>().material = colorNotificacion;
                }
                yield return new WaitForSeconds(1f);
                for (int i = 1; i <= 5; i++)
                {
                    if (TemperatureDisplay.currentFloor == i)
                    {
                        holograma.transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_green;
                    }
                    else
                    {
                        holograma.transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_blue;
                    }

                }
                yield return new WaitForSeconds(0.7f);
            }
            
        }
    }

    public void hologramaNOParpadeo(GameObject holograma)
    {
        
        for (int i = 1; i <= 5; i++)
        {
            if (TemperatureDisplay.currentFloor == i && holograma.activeSelf == true)
            {
                holograma.transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_green;
            }
            else
            {
                holograma.transform.GetChild(i - 1).GetComponent<Renderer>().material = Transparent_blue;
            }
        }
            
        
    }

    IEnumerator floorParpadeo(GameObject holograma, int floor, Material colorNotificacion)
    {
        while (true)
        {
            holograma.transform.GetChild(floor-1).GetComponent<Renderer>().material = colorNotificacion;
            yield return new WaitForSeconds(1f);

            if (TemperatureDisplay.currentFloor == floor && holograma.activeSelf == true)
            {
                holograma.transform.GetChild(floor - 1).GetComponent<Renderer>().material = Transparent_green;
            }
            else
            {
                holograma.transform.GetChild(floor - 1).GetComponent<Renderer>().material = Transparent_blue;
            }
            
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void floorNOParpadeo(GameObject holograma, int floor)
    {

        
        if (TemperatureDisplay.currentFloor == floor && holograma.activeSelf == true)
        {
            holograma.transform.GetChild(floor - 1).GetComponent<Renderer>().material = Transparent_green;
        }
        else
        {
            holograma.transform.GetChild(floor - 1).GetComponent<Renderer>().material = Transparent_blue;
        }
        
    }

    public void incendioHolograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramNotificationCoroutine = StartCoroutine(hologramaParpadeo(Hologram1, Transparent_red));
    }

    public void HighTemperatureHolograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramNotificationCoroutine = StartCoroutine(hologramaParpadeo(Hologram1, Transparent_orange));
    }

    public void waterLeakHolograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramNotificationCoroutine = StartCoroutine(floorParpadeo(Hologram2, 2, Transparent_red));
    }

    public void water_cutoffLeakHolograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramNotificationCoroutine = StartCoroutine(floorParpadeo(Hologram2, 2, Transparent_orange));
    }

    public void peopleAlarmHolograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramNotificationCoroutine = StartCoroutine(floorParpadeo(Hologram3, 2, Transparent_red));
    }

    public void situacionNormal_holograma()
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }
        hologramaNOParpadeo(Hologram1);
    }

    public void situacionNormal_floor(int holograma, int floor)
    {
        if (hologramNotificationCoroutine != null)
        {
            StopCoroutine(hologramNotificationCoroutine);
        }

        if(holograma == 1)
        {
            floorNOParpadeo(Hologram1, floor);
        }
        else if(holograma == 2)
        {
            floorNOParpadeo(Hologram2, floor);
        }
        else if(holograma == 3)
        {
            floorNOParpadeo(Hologram3, floor);
        }
        
    }
}
