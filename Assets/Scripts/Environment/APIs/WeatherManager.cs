using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private GameObject rainobj;
    [SerializeField] private GameObject snowobj;
    [SerializeField] private Renderer Highcloud;
    [SerializeField] private Renderer Lowcloud;

    public void ChangeWeather(string Nowweater)
    {
        rainobj.SetActive(false);
        snowobj.SetActive(false);
        if (Nowweater == "Rain")
        {
            Highcloud.material.SetColor("_CloudColor", Color.gray);
            Highcloud.material.SetFloat("_Density", 0f);
            Lowcloud.material.SetFloat("_Density", 0f);
            rainobj.SetActive(true);
        }
        else if (Nowweater == "Snow")
        {
            Highcloud.material.SetColor("_CloudColor", Color.gray);
            Highcloud.material.SetFloat("_Density", 0f);
            Lowcloud.material.SetFloat("_Density", 0f);
            snowobj.SetActive(true);
        }
        else if (Nowweater == "Clouds")
        {
            //Highcloud.color = Color.gray;
            Highcloud.material.SetColor("_CloudColor", Color.gray);
            Highcloud.material.SetFloat("_Density", 0f);
            Lowcloud.material.SetFloat("_Density", 0f);
        }
        else
        {
            Highcloud.material.SetColor("_CloudColor", Color.white);
            Highcloud.material.SetFloat("_Density", 1.5f);
            Lowcloud.material.SetFloat("_Density", 1.5f);
        }
    }
}