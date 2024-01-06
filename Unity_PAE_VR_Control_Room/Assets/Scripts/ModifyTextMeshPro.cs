using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyTextMeshPro : MonoBehaviour
{

    public Text worldText;

    public string textoInicial = "Pool death";
    
    // Start is called before the first frame update
    void Start()
    {
        worldText.text = textoInicial;
    }

    // Update is called once per frame
    void Update()


    {
        
    }
}
