using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GetWeather : MonoBehaviour
{
    private string url = "https://api.openweathermap.org/data/2.5/weather?lat=";

    // APIキーは別ファイルに記述し、gitignoreに書き込む
    private string APIKEY = APIKeys.Key;
    private string callurl;

    // 場所はどこでもいいが、今回はスカイツリーがある東京都墨田区押上１丁目１?２付近の緯度と経度を設定
    private string lat = "35.71";
    private string lon = "139.81";

    [SerializeField]
    private SkyBoxSetter skyBoxSetter;

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

    private void Awake()
    {
        //WManager = GameObject.Find("WeatherManager");
        //skyBoxSetter = WManager.GetComponent<SkyBoxSetter>();
        Getting();
    }

    public void Getting()
    {
        callurl = url + lat + "&lon=" + lon + "&exclude=hourly,daily&appid=" + APIKEY;
        Debug.Log("呼び出し中");
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
                Debug.Log("リクエスト中");
                break;
            case UnityWebRequest.Result.Success:
                var jsontext = response.downloadHandler.text;
                Debug.Log(jsontext);
                weatherstruct = JsonUtility.FromJson<Weatherstruct>(jsontext);
                var result = weatherstruct.weather[0].main;
                skyBoxSetter.ChangeWeather(result);
                Debug.Log(weatherstruct.weather[0].main);
                break;
        }
    }
}
