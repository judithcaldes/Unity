using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ClockController : MonoBehaviour
{
    public Text clockText; // Referencia al componente Text donde se mostrará la hora
    private TimeSpan time; // Objeto TimeSpan para manejar el tiempo del reloj

    void Start()
    {
        time = new TimeSpan(8, 0, 0); // Inicia a las 8 de la mañana
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            time = time.Add(TimeSpan.FromMinutes(5));
            clockText.text = time.ToString(@"hh\:mm");
            yield return new WaitForSeconds(1);
        }
    }
}
