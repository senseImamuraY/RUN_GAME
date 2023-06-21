using UnityEngine;
using System.Collections.Generic;

public class ObjectSearcher : MonoBehaviour
{
    [SerializeField]
    Transform athleticParentTransform;

    [SerializeField]
    string targetName = "Athletic";

    [SerializeField]
    List<string> searchNames;

    private List<GameObject> treeLists = new List<GameObject>();

    [SerializeField]
    ItemGenerator itemGenerator;

    [SerializeField]
    EnemyGenerator enemyGenerator;

    [SerializeField]
    private LinearTreeController treeController;

    private void Awake()
    {
        if (athleticParentTransform == null)
        {
            Debug.LogError("Parent transform is not assigned.");
            return;
        }
        treeLists.Clear();
        treeLists = treeController.Objects;

        // ヒエラルキーから「Athletic」と名の付いたオブジェクトを３つ探す
        List<GameObject> athleticObjects = SearchObjectsByName(athleticParentTransform, targetName, 3);

        // Athleticオブジェクトの子オブジェクトを再帰的に探索し、特定の名前を持つオブジェクトをリストに格納する
        foreach (GameObject athleticObject in athleticObjects)
        {
            RecursiveSearchChildren(athleticObject.transform);
        }

        List<GameObject> Items = itemGenerator.GetItemPrefabs();

        foreach (GameObject item in Items)
        {
            treeLists.Add(item);
        }

        List<GameObject> Enemies = enemyGenerator.GetEnemyPrefabs();

        foreach (GameObject enemy in Enemies)
        {
            treeLists.Add(enemy);
        }

        // リストの中身をログ出力する
        foreach (GameObject foundObject in treeLists)
        {
            Debug.Log("Found Object: " + foundObject.name);
        }
    }

    private List<GameObject> SearchObjectsByName(Transform currentTransform, string name, int maxCount)
    {
        List<GameObject> result = new List<GameObject>();

        if (currentTransform.name.Contains(name))
        {
            result.Add(currentTransform.gameObject);
        }

        if (result.Count >= maxCount)
        {
            return result;
        }

        for (int i = 0; i < currentTransform.childCount; i++)
        {
            Transform childTransform = currentTransform.GetChild(i);
            result.AddRange(SearchObjectsByName(childTransform, name, maxCount - result.Count));

            if (result.Count >= maxCount)
            {
                break;
            }
        }

        return result;
    }

    private void RecursiveSearchChildren(Transform currentTransform)
    {
        foreach (string searchName in searchNames)
        {
            if (currentTransform.name.Contains(searchName))
            {
                treeLists.Add(currentTransform.gameObject);
                Debug.Log("added object to treeList");
                break; // 一致したらループを抜ける
            }
        }

        for (int i = 0; i < currentTransform.childCount; i++)
        {
            Transform childTransform = currentTransform.GetChild(i);
            RecursiveSearchChildren(childTransform);
        }
    }
}
