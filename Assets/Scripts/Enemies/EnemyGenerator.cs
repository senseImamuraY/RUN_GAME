using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs; // �A�C�e���̃v���n�u

    [SerializeField]
    private float xRange = 5f; // X�������̔z�u�͈�

    [SerializeField]
    private float zRange = 1000f; // Z�������̔z�u�͈�

    [SerializeField]
    private Transform parent;

    private List<GameObject> instantiatedEnemyPrefabs;

    private void Awake()
    {
        instantiatedEnemyPrefabs = new List<GameObject>(); // ���X�g��������

        // �w�肳�ꂽ���̃A�C�e����z�u����
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            // �����_���Ȉʒu�𐶐�
            float xPos = Random.Range(-xRange, xRange);
            float yPos = 0f;
            float zPos = Random.Range(-zRange, zRange);
            Vector3 position = new Vector3(xPos, yPos, zPos);

            // �A�C�e���𐶐����Ĕz�u
            GameObject tmp = Instantiate(enemyPrefabs[i], position, Quaternion.identity, parent);
            instantiatedEnemyPrefabs.Add(tmp);
        }
    }

    public List<GameObject> GetEnemyPrefabs()
    {
        return instantiatedEnemyPrefabs;
    }
}

