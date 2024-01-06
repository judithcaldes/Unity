using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_selection_maqueta : MonoBehaviour
{
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject Hologram3;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    public void DropdownHologramSelected(int value)
    {
        if (value == 0)
        {
            Hologram1.SetActive(true);
            Hologram2.SetActive(false);
            Hologram3.SetActive(false);
        }
        if (value == 1)
        {
            Hologram1.SetActive(false);
            Hologram2.SetActive(true);
            Hologram3.SetActive(false);
        }
        if (value == 2)
        {
            Hologram1.SetActive(false);
            Hologram2.SetActive(false);
            Hologram3.SetActive(true);
        }

    }
}
