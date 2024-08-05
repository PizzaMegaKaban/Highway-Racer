using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BrakeLights : MonoBehaviour
{
    /// <summary>
    /// Объекты стоп-огней.
    /// </summary>
    public GameObject[] BrakeLights;

    /// <summary>
    /// Материал для стоп-огней когда они активны.
    /// </summary>
    public Material ActiveBrakeLightsMaterial;

    /// <summary>
    /// Материал для стоп-огей когда они неактивны.
    /// </summary>
    public Material DisabledBrakeLightsMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            foreach (var brakeLight in BrakeLights)
            {
                brakeLight.GetComponent<Renderer>().material = ActiveBrakeLightsMaterial;
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            
            foreach (var brakeLight in BrakeLights)
            {
                brakeLight.GetComponent<Renderer>().material = DisabledBrakeLightsMaterial;
            }
        }
    }
}
