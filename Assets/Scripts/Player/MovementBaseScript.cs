using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBaseScript : MonoBehaviour
{
    [SerializeField]
    PathCreator pathCreator;

    [SerializeField]
    BonusPlayerScript player;

    [SerializeField]
    GameObject helpUI;
    // 6f
    float speed = 20f;
    Vector3 endPos;

    float moveDistance;

    void Start()
    {
        endPos = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
    }

    void Update()
    {
        // プレイ中以外は無効にする
        if (GameManagerScript.status != GameManagerScript.GAME_STATUS.Play)
        {
            helpUI.SetActive(false);
            return;
        }

        // タップ中は走る
        if (Input.GetMouseButton(0))
        {
            moveDistance += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(moveDistance, EndOfPathInstruction.Stop);
            transform.rotation = pathCreator.path.GetRotationAtDistance(moveDistance, EndOfPathInstruction.Stop);

            player.isRunning = true;
            helpUI.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            player.isRunning = false;
            helpUI.SetActive(true);
        }
    }
}