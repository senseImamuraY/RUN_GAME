using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSetter : MonoBehaviour
{
    [SerializeField] 
    private Material rainySkyBox; 

    [SerializeField] 
    private Material cloudySkyBox;

    [SerializeField] 
    private Material sunnySkyBox;

    [SerializeField] 
    private Material fantasySkyBox;

    public void ChangeWeather(string Nowweater)
    {
        if (Nowweater == "Rain")
        {
            RenderSettings.skybox = rainySkyBox; // Skyboxƒ}ƒeƒŠƒAƒ‹‚ğİ’è
        }
        else if (Nowweater == "Clouds")
        {
            RenderSettings.skybox = cloudySkyBox;
        }
        else if (Nowweater == "Clear")
        {
            RenderSettings.skybox = sunnySkyBox;
        }
        else
        {
            RenderSettings.skybox = fantasySkyBox;
        }
    }
}