using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sun_moon : MonoBehaviour
{
    public Transform centroDeRotacion;
    private float radio = 4f;//1.2f;
    private float velocidadAngular = 0.49f;//1.2f;//0.9f;//Mathf.PI/192f; // Grados por segundo
    private float tiempoParaCambio = 180f;//146f //144f// Tiempo en segundos antes de cambiar el objeto
    //public float tiempoReinicio = 192f;
    //public GameObject Sun_Sphere;
    private bool moon = true;// primero se cambia a luna, luego a sol, luego a luna...
    //private bool reinicio = true;
    public Material moonMaterial;
    public Material sunMaterial;
    public GameObject trail;
    private bool trailActive = false;
    //public ParticleSystem nuevoParticleSystem;
    private float posY;
    private float posX;
    private float posYini;
    private float posXini;
    private float anguloini = 45f;//20f;


    void Start()
    {
    
        posXini = centroDeRotacion.position.x + radio* Mathf.Cos(+Mathf.Deg2Rad * anguloini);
        posYini = centroDeRotacion.position.y -1.8f + radio * Mathf.Sin(Mathf.Deg2Rad * anguloini); ;//1f;
        Renderer renderer = GetComponent<Renderer>();
        //ParticleSystem = GetCo
        renderer.material = sunMaterial;

        //Sun_Sphere = transform.Find("Sun_sphere").GetComponent<GameObject>();

    }
  

    private float tiempoTranscurrido = 0f;

    void Update()
    {
        //Instantiate(Sun_Sphere, transform.position, Quaternion.identity);
        // Calcular la posición en el semicírculo
        if (tiempoTranscurrido<= tiempoParaCambio)// mientras no se llegue a las 20:00 se sigue cambiando
        {
           // reinicio = true;
            float angulo = anguloini + tiempoTranscurrido * velocidadAngular;
            
            posX = centroDeRotacion.position.x + radio * Mathf.Cos(+Mathf.Deg2Rad * angulo);
            posY = centroDeRotacion.position.y - 1.8f + radio * Mathf.Sin(Mathf.Deg2Rad * angulo);

            transform.position = new Vector3(posX, posY, transform.position.z);

            if (tiempoTranscurrido >= 2f && trailActive== false)
            {
                trail.SetActive(true);
                trailActive = true;
            }

            // Mover el objeto
            
            // Verificar si es el momento de cambiar el objeto
            /*
            if (tiempoTranscurrido >= tiempoParaCambio && cambio) // tiempoTranscurrido == tiempoParaCambio??
            {
                CambiarMaterial(cambio);//cambio a luna
                cambio = false;
            }
            */

        }
        else
        {
            CambiarMaterial();
            tiempoTranscurrido = 0f;
            trail.SetActive(false);
            trailActive = false;
            transform.position = new Vector3(posXini, posYini, transform.position.z); 
           
        }
        // Actualizar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

    }

    void CambiarMaterial()// cuando cambio es true se cambiará a luna, si no a sol
    {
        //cambiar el material
        Renderer renderer = GetComponent<Renderer>();
        if (moon == true)
        {
            renderer.material = moonMaterial;
            this.moon = false;
        }
        else
        {
            renderer.material = sunMaterial;
            this.moon = true;
        }
      

    }
}
