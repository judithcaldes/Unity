using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    Light myLight;
    bool alarm = false;
    Color red = new Color(1f, 0f, 0f, 1f);
    Color orange = new Color(1f, 0.5f, 0f, 1f);
    Color green = new Color(0f, 1f, 0f, 1f);
    Color green_dark = new Color(0f, 0.9f, 0f, 1f);
    Color white = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetColor(red);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SetColor(orange);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SetColor(green);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetColor(green_dark);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SetColor(white);
        }
    }

    void SetColor(Color color)
    {
        myLight.color = color;
    }
}
