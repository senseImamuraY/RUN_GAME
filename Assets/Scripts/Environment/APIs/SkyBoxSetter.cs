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
        //rainobj.SetActive(false);
        //snowobj.SetActive(false);
        if (Nowweater == "Rain")
        {
            RenderSettings.skybox = rainySkyBox; // SkyboxÉ}ÉeÉäÉAÉãÇê›íË
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