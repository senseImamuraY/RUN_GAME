using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public enum GAME_STATUS { Play, Clear, Pause, GameOver };
    public static GAME_STATUS status;

    public static int tempCoinNum;

    [SerializeField]
    TextMeshProUGUI coinNumText, resultCoinText, levelNumText;

    [SerializeField]
    GameObject clearUI, gameOverUI;

    int stageCoinNum;

    const string STAGE_NAME_PREFIX = "Stage";
    const int MAX_STAGE_NUM = 1;

    int levelNum; // 現在の進行数
    int stageNum; // 読み込むステージ番号

    void Start()
    {
        // ステージ番号ロード
        stageNum = PlayerPrefs.GetInt("stageNum", 1);

        // 自分のシーンではない場合、ロードし直す
        if (!GetLoadSceneName().Equals(SceneManager.GetActiveScene().name))
        {
            LoadScene();
            return;
        }

        // レベル番号をロード
        levelNum = PlayerPrefs.GetInt("levelNum", 1);
        levelNumText.text = "Level " + levelNum;

        // ステージ内のコインの枚数を取得
        stageCoinNum = GameObject.FindGameObjectsWithTag("Coin").Length;

        // これまでの獲得コイン数をロード（初回は0）
        tempCoinNum = PlayerPrefs.GetInt("coinNum", 0);

        // ステータスをPlayに
        status = GAME_STATUS.Play;
    }

    void Update()
    {
        if (status == GAME_STATUS.Clear)
        {
            // 現在のステージで獲得したコインの枚数
            int getCoinNum = tempCoinNum - PlayerPrefs.GetInt("coinNum", 0);

            resultCoinText.text = getCoinNum.ToString().PadLeft(3) + "/" + stageCoinNum; clearUI.SetActive(true);
            clearUI.SetActive(true);

            // コインを保存
            PlayerPrefs.SetInt("coinNum", tempCoinNum);

            enabled = false;
        }
        else if (status == GAME_STATUS.GameOver)
        {
            Invoke("ShowGameOverUI", 3f);
            enabled = false;
            return;
        }

        coinNumText.text = tempCoinNum.ToString();
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private string GetLoadSceneName()
    {
        return STAGE_NAME_PREFIX + 2;
        //return STAGE_NAME_PREFIX + stageNum;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(GetLoadSceneName());
    }

    public void LoadNextScene()
    {
        PlayerPrefs.SetInt("levelNum", ++levelNum);

        stageNum = levelNum <= MAX_STAGE_NUM ? levelNum : Random.Range(1, MAX_STAGE_NUM + 1);
        PlayerPrefs.SetInt("stageNum", stageNum);

        LoadScene();
    }
    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}