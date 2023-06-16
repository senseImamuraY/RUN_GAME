using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs; // アイテムのプレハブ

    [SerializeField]
    private float xRange = 5f; // X軸方向の配置範囲

    [SerializeField]
    private float zRange = 1000f; // Z軸方向の配置範囲

    [SerializeField]
    private Transform parent;

    private List<GameObject> instantiatedEnemyPrefabs;

    private void Awake()
    {
        instantiatedEnemyPrefabs = new List<GameObject>(); // リストを初期化

        // 指定された数のアイテムを配置する
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            // ランダムな位置を生成
            float xPos = Random.Range(-xRange, xRange);
            float yPos = 0f;
            float zPos = Random.Range(-zRange, zRange);
            Vector3 position = new Vector3(xPos, yPos, zPos);

            // アイテムを生成して配置
            GameObject tmp = Instantiate(enemyPrefabs[i], position, Quaternion.identity, parent);
            instantiatedEnemyPrefabs.Add(tmp);
        }
    }

    public List<GameObject> GetEnemyPrefabs()
    {
        return instantiatedEnemyPrefabs;
    }
}

