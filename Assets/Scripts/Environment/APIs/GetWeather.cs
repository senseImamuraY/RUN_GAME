using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GetWeather : MonoBehaviour
{
    private string url = "https://api.openweathermap.org/data/2.5/weather?lat=";

    // API�L�[�͕ʃt�@�C���ɋL�q���Agitignore�ɏ�������
    private string APIKEY = APIKeys.Key;
    private string callurl;

    // �ꏊ�͂ǂ��ł��������A����̓X�J�C�c���[�����铌���s�n�c�扟��P���ڂP?�Q�t�߂̈ܓx�ƌo�x��ݒ�
    private string lat = "35.71";
    private string lon = "139.81";

    private WeatherManager manager;
    private GameObject WManager;

    [Serializable]
    public struct Weatherstruct
    {

        public Weathermain[] weather;

        [Serializable]
        public struct Weathermain
        {
            public int id;
            public string main;
            public string description;
            public string icon;
        }
    }

    private void Start()
    {
        WManager = GameObject.Find("WeatherManager");
        manager = WManager.GetComponent<WeatherManager>();
    }

    public void Getting()
    {
        callurl = url + lat + "&lon=" + lon + "&exclude=hourly,daily&appid=" + APIKEY;
        Debug.Log("�Ăяo����");
        Debug.Log(callurl);
        StartCoroutine("GetData", callurl);
    }

    private IEnumerator GetData(string URL)
    {
        Weatherstruct weatherstruct = new Weatherstruct();
        UnityWebRequest response = UnityWebRequest.Get(URL);
        yield return response.SendWebRequest();

        switch (response.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;
            case UnityWebRequest.Result.Success:
                var jsontext = response.downloadHandler.text;
                Debug.Log(jsontext);
                weatherstruct = JsonUtility.FromJson<Weatherstruct>(jsontext);
                var result = weatherstruct.weather[0].main;
                manager.ChangeWeather(result);
                Debug.Log(weatherstruct.weather[0].main);
                break;
        }
    }
}