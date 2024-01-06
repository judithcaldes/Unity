using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLight : MonoBehaviour
{
    Light myLight;
    bool alarm = false;
    Color red = new Color(1f, 0f, 0f, 1f);
    Color orange = new Color(1f, 0.5f, 0f, 1f);
    Color green = new Color(0f, 1f, 0f, 1f);
    Color green_dark = new Color(0f, 0.9f, 0f, 1f);
    Color white = new Color(1f, 1f, 1f, 1f);
    Coroutine colorChangeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();

        //StartCoroutine(ChangeLightColorAfterDelay(5f));
    }

    IEnumerator ChangeLightColorAfterDelay(float delay, Color color_light)
    {
        yield return new WaitForSeconds(delay);
        alarm = true;

        while (alarm)
        {
            myLight.color = color_light;
            yield return new WaitForSeconds(1f);
            myLight.color = white;
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void AlarmOnRed()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
        colorChangeCoroutine = StartCoroutine(ChangeLightColorAfterDelay(0f, red));
    }

    public void AlarmOnOrange()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
        colorChangeCoroutine = StartCoroutine(ChangeLightColorAfterDelay(0f, orange));
    }

    public void AlarmOnGreen()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
        colorChangeCoroutine = StartCoroutine(ChangeLightColorAfterDelay(0f, green));
    }

    public void AlarmOnGreenDark()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
        colorChangeCoroutine = StartCoroutine(ChangeLightColorAfterDelay(0f, green_dark));
    }

    public void AlarmOff()
    {
        myLight.color = white;
        alarm = false;
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
    }
}
