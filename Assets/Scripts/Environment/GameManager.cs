using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance { get; private set; }

    public enum GAME_STATUS { Play, Clear, Pause, GameOver };
    public static GAME_STATUS status;

    public static int tempCoinNum;

    [SerializeField]
    TextMeshProUGUI coinNumText, resultCoinText, levelNumText;

    [SerializeField]
    GameObject clearUI, gameOverUI;

    [SerializeField]
    private List<GameObject> targetList;


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

    int levelNum; // ���݂̐i�s��
    int stageNum; // �ǂݍ��ރX�e�[�W�ԍ�

    [SerializeField]
    private LinearTreeController linearTreeController;

    private List<GameObject> collisionList;

    [SerializeField]
    Goal goal;

    [SerializeField]
    Player player;
    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        collisionList = linearTreeController.GetCollisionList();


        foreach (GameObject target in targetList)
        {
            if (target.GetComponent<IPlane>() != null)
            {
                Debug.Log("Plane�ɒǉ�����܂����B");
                planeList.Add(target.GetComponent<IPlane>());
            }
            else
            {
                Debug.Log("�ǂ�ɂ��ǉ�����܂���ł���");
            }
        }

        //foreach (GameObject target in targetList)
        //    //{
        //if (target.GetComponent<IPlane>() != null)
        //{
        //    Debug.Log("Plane�ɒǉ�����܂����B");
        //    planeList.Add(target.GetComponent<IPlane>());
        //}
        //else if (target.GetComponent<ISphere>() != null)
        //{
        //    Debug.Log("Sphere�ɒǉ�����܂����B");
        //    sphereList.Add(target.GetComponent<ISphere>());
        //}
        //else if (target.GetComponent<ICube>() != null)
        //{
        //    Debug.Log("Box�ɒǉ�����܂����B");
        //    cubeList.Add(target.GetComponent<ICube>());
        //    Debug.Log(cubeList);
        //}
        //else if (target.GetComponent<ICapsule>() != null)
        //{
        //    Debug.Log("Capsule�ɒǉ�����܂����B");
        //    capsuleList.Add(target.GetComponent<ICapsule>());
        //}
        //else
        //{
        //    Debug.Log("�ǂ�ɂ��ǉ�����܂���ł���");
        //}

    }



    void Start()
    {
        // �X�e�[�W�ԍ����[�h
        stageNum = PlayerPrefs.GetInt("stageNum", 1);

        // �����̃V�[���ł͂Ȃ��ꍇ�A���[�h������
        if (!GetLoadSceneName().Equals(SceneManager.GetActiveScene().name))
        {
            LoadScene();
            return;
        }

        // ���x���ԍ������[�h
        levelNum = PlayerPrefs.GetInt("levelNum", 1);
        levelNumText.text = "Level " + levelNum;

        // �X�e�[�W���̃R�C���̖������擾
        stageCoinNum = GameObject.FindGameObjectsWithTag("Coin").Length;

        // ����܂ł̊l���R�C���������[�h�i�����0�j
        tempCoinNum = PlayerPrefs.GetInt("coinNum", 0);

        // �X�e�[�^�X��Play��
        status = GAME_STATUS.Play;
    }

    private void FixedUpdate()
    {
        collisionList = linearTreeController.GetCollisionList();

        foreach (GameObject target in collisionList)
        {
            if (target.GetComponent<ISphere>() != null)
            {
                Debug.Log("Sphere�ɒǉ�����܂����B");
                sphereList.Add(target.GetComponent<ISphere>());
            }
            else if (target.GetComponent<ICube>() != null)
            {
                Debug.Log("Box�ɒǉ�����܂����B");
                cubeList.Add(target.GetComponent<ICube>());
                Debug.Log(cubeList);
            }
            else
            {
                Debug.Log("�ǂ�ɂ��ǉ�����܂���ł���");
            }
        }
        Debug.Log("collisionList.Count = " +  collisionList.Count);
        goal.GoalEffect(player);
    }
    void Update()
    {
        
        if (status == GAME_STATUS.Clear)
        {
            // ���݂̃X�e�[�W�Ŋl�������R�C���̖���
            int getCoinNum = tempCoinNum - PlayerPrefs.GetInt("coinNum", 0);

            resultCoinText.text = getCoinNum.ToString().PadLeft(3) + "/" + stageCoinNum; clearUI.SetActive(true);
            clearUI.SetActive(true);

            // �R�C����ۑ�
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
        Debug.Log("LoadNextScene");
        GameManager.status = GAME_STATUS.Play;
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