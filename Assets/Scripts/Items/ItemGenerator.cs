using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> itemPrefabs; // �A�C�e���̃v���n�u

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private float xRange = 5f; // X�������̔z�u�͈�

    [SerializeField]
    private float zRange = 1000f; // Z�������̔z�u�͈�

    private List<GameObject> instantiatedItemPrefabs;

    private void Awake()
    {
        instantiatedItemPrefabs = new List<GameObject>(); // ���X�g��������

        // �w�肳�ꂽ���̃A�C�e����z�u����
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            // �����_���Ȉʒu�𐶐�
            float xPos = Random.Range(-xRange, xRange);
            float yPos = 0.5f;
            float zPos = Random.Range(-zRange, zRange);
            Vector3 position = new Vector3(xPos, yPos, zPos);

            // �A�C�e���𐶐����Ĕz�u
            GameObject tmp = Instantiate(itemPrefabs[i], position, Quaternion.identity, parent);
            instantiatedItemPrefabs.Add(tmp);
        }
    }

    public List<GameObject> GetItemPrefabs()
    {
        return instantiatedItemPrefabs;
    }
}


