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

    int levelNum; // ���݂̐i�s��
    int stageNum; // �ǂݍ��ރX�e�[�W�ԍ�

    [SerializeField]
    private LinearTreeController linearTreeController;

    private List<GameObject> collisionList;

    [SerializeField]
    Goal goal;

    [SerializeField]
    Player player;

    public float startTime = 3f; // �J�E���g�_�E���J�n�b��
    private float currentTime; // �J�E���g�_�E�����ݕb��
    public bool isCounting = false; // �J�E���g�_�E�������ǂ���

    private void Awake()
    {
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
    }



    void Start()
    {
        // ���x���ԍ������[�h
        levelNum = PlayerPrefs.GetInt("levelNum", 1);
        levelNumText.text = "Level " + levelNum;

        // �X�e�[�W���̃R�C���̖������擾
        stageCoinNum = GameObject.FindGameObjectsWithTag("Coin").Length;

        // ����܂ł̊l���R�C���������[�h�i�����0�j
        tempCoinNum = PlayerPrefs.GetInt("coinNum", 0);

        // �X�e�[�^�X��Play��
        status = GAME_STATUS.Play;

        currentTime = startTime; // ���ݕb���ɊJ�n�b������
        countdownText.text = currentTime.ToString("F0"); // �e�L�X�g�Ɍ��ݕb����\��
        StartCoroutine(CountDown()); // �R���[�`�����J�n
    }

    private void FixedUpdate()
    {       
        if (status == GAME_STATUS.Clear)
        {
            // ���݂̃X�e�[�W�Ŋl�������R�C���̖���
            int getCoinNum = tempCoinNum - PlayerPrefs.GetInt("coinNum", 0);

            resultCoinText.text = getCoinNum.ToString().PadLeft(3) + "/" + stageCoinNum;
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
    // ���݂̃V�[�������[�h
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
        isCounting = true; // �J�E���g�_�E�����t���O���I���ɂ���
        player.enabled = false;
        while (currentTime > 0) // ���ݕb����0���傫���ԌJ��Ԃ�
        {
            yield return new WaitForSeconds(1f); // 1�b�҂�
            currentTime -= 1f; // ���ݕb����1���炷
            countdownText.text = currentTime.ToString("F0"); // �e�L�X�g�Ɍ��ݕb����\��
        }
        isCounting = false; // �J�E���g�_�E�����t���O���I�t�ɂ���
        countdownText.fontSize = 300f;
        countdownText.text = "Start"; // �e�L�X�g�ɃX�^�[�g�ƕ\��

        // �����ŃQ�[���̃X�^�[�g�������Ăяo���Ȃǂ���
        player.enabled = true;

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        
    }
}