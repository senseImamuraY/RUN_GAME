using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> itemPrefabs; // アイテムのプレハブ

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private float xRange = 5f; // X軸方向の配置範囲

    [SerializeField]
    private float zRange = 1000f; // Z軸方向の配置範囲

    private List<GameObject> instantiatedItemPrefabs;

    private void Awake()
    {
        instantiatedItemPrefabs = new List<GameObject>(); // リストを初期化

        // 指定された数のアイテムを配置する
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            // ランダムな位置を生成
            float xPos = Random.Range(-xRange, xRange);
            float yPos = 0.5f;
            float zPos = Random.Range(-zRange, zRange);
            Vector3 position = new Vector3(xPos, yPos, zPos);

            // アイテムを生成して配置
            GameObject tmp = Instantiate(itemPrefabs[i], position, Quaternion.identity, parent);
            instantiatedItemPrefabs.Add(tmp);
        }
    }

    public List<GameObject> GetItemPrefabs()
    {
        return instantiatedItemPrefabs;
    }
}


