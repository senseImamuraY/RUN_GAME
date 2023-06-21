using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public enum GAME_STATUS { Play, Clear, Pause, GameOver };
    public static GAME_STATUS status;

    public static int tempCoinNum;
    private int SceneNum = 1;

    [SerializeField]
    TextMeshProUGUI coinNumText, resultCoinText, levelNumText, countdownText;

    [SerializeField]
    TextMeshProUGUI speedNumText, gravityNumText, jumpNumText, slideNumText;

    [SerializeField]
    GameObject clearUI, gameOverUI;

    [SerializeField]
    private List<GameObject> targetList;

    [SerializeField]
    private List<GameObject> athletics;

    private List<ISphere> sphereList = new List<ISphere>();
    private List<ICube> cubeList = new List<ICube>();
    private List<ICapsule> capsuleList = new List<ICapsule>();
    private List<IPlane> planeList = new List<IPlane>();

    public List<ISphere> GetSphereList { get { return sphereList; } }
    public List<ICube> GetCubeList { get { return cubeList; } }
    public List<ICapsule> GetCapsuleList { get { return capsuleList; } }

    public List<IPlane> GetPlaneList { get { return planeList; } }
    int stageCoinNum;

    const string STAGE_NAME_PREFIX = "Stage";
    const int MAX_STAGE_NUM = 1;

    int levelNum; // 現在の進行数
    int stageNum; // 読み込むステージ番号

    [SerializeField]
    private LinearTreeController linearTreeController;

    private List<GameObject> collisionList;

    [SerializeField]
    Goal goal;

    [SerializeField]
    Player player;

    public float startTime = 3f; // カウントダウン開始秒数
    private float currentTime; // カウントダウン現在秒数
    public bool isCounting = false; // カウントダウン中かどうか

    private void Awake()
    {
        collisionList = linearTreeController.GetCollisionList();

        foreach (GameObject target in targetList)
        {
            if (target.GetComponent<IPlane>() != null)
            {
                Debug.Log("Planeに追加されました。");
                planeList.Add(target.GetComponent<IPlane>());
            }
            else
            {
                Debug.Log("どれにも追加されませんでした");
            }
        }
    }



    void Start()
    {
        // レベル番号をロード
        levelNum = PlayerPrefs.GetInt("levelNum", 1);
        levelNumText.text = "Level " + levelNum;

        // ステージ内のコインの枚数を取得
        stageCoinNum = GameObject.FindGameObjectsWithTag("Coin").Length;

        // これまでの獲得コイン数をロード（初回は0）
        tempCoinNum = PlayerPrefs.GetInt("coinNum", 0);

        // ステータスをPlayに
        status = GAME_STATUS.Play;

        currentTime = startTime; // 現在秒数に開始秒数を代入
        countdownText.text = currentTime.ToString("F0"); // テキストに現在秒数を表示
        StartCoroutine(CountDown()); // コルーチンを開始
    }

    private void FixedUpdate()
    {       
        if (status == GAME_STATUS.Clear)
        {
            // 現在のステージで獲得したコインの枚数
            int getCoinNum = tempCoinNum - PlayerPrefs.GetInt("coinNum", 0);

            resultCoinText.text = getCoinNum.ToString().PadLeft(3) + "/" + stageCoinNum;
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

        collisionList = linearTreeController.GetCollisionList();

        foreach (GameObject target in collisionList)
        {
            if (target.GetComponent<ISphere>() != null)
            {
                sphereList.Add(target.GetComponent<ISphere>());
            }
            else if (target.GetComponent<ICube>() != null)
            {
                cubeList.Add(target.GetComponent<ICube>());
            }
        }

        Debug.Log("collisionList.Count = " + collisionList.Count);
        goal.GoalEffect(player);

        coinNumText.text = tempCoinNum.ToString();

        speedNumText.text = player.GetSpeedNum().ToString();
        gravityNumText.text = player.GetGravityNum().ToString();
        jumpNumText.text = player.GetJumpPowerNum().ToString();
        slideNumText.text = player.GetSlideNum().ToString();

    }
    // 現在のシーンをロード
    public void LoadCurrentScene()
    {
        Debug.Log("LoadNextScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private string GetLoadSceneName()
    {
        return STAGE_NAME_PREFIX + SceneNum;
    }

    private string GetLoadNextSceneName()
    {       
        if (SceneNum < 2)
        {
            SceneNum++;
        }
        return STAGE_NAME_PREFIX + SceneNum;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(GetLoadNextSceneName());
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(GetLoadSceneName());
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }

    IEnumerator CountDown()
    {
        isCounting = true; // カウントダウン中フラグをオンにする
        player.enabled = false;
        while (currentTime > 0) // 現在秒数が0より大きい間繰り返す
        {
            yield return new WaitForSeconds(1f); // 1秒待つ
            currentTime -= 1f; // 現在秒数を1減らす
            countdownText.text = currentTime.ToString("F0"); // テキストに現在秒数を表示
        }
        isCounting = false; // カウントダウン中フラグをオフにする
        countdownText.fontSize = 300f;
        countdownText.text = "Start"; // テキストにスタートと表示

        // ここでゲームのスタート処理を呼び出すなどする
        player.enabled = true;

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        
    }
}