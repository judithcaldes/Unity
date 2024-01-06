using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilacionReloj : MonoBehaviour
{
    public float velocidadOscilacion = 1.0f;
    public float amplitudOscilacion = 0.5f;

    private Vector3 posicionInicial;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float desplazamientoVertical = Mathf.Sin(Time.time * velocidadOscilacion) * amplitudOscilacion;

        transform.position = posicionInicial + new Vector3(0.0f, desplazamientoVertical, 0.0f);
    }
}
