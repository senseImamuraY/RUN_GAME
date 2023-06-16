using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour, IEnemy
{
    CustomCubeCollider cubeCollider;
    //private enum ColorType { Normal, Red, Green, Violet }
    //private ColorType colorType;

    //[SerializeField]
    //private List<Material> colorMaterials;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        cubeCollider = GetComponent<CustomCubeCollider>();
        cubeCollider.Enemy = this;
        Debug.Log(cubeCollider.Enemy);
    }

    void Start()
    {      
        cubeCollider = GetComponent<CustomCubeCollider>();
        //colorType = (ColorType)Random.Range(0, System.Enum.GetValues(typeof(ColorType)).Length);
        //SetColor(colorType);

    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }
    }

    IEnumerator EnterCoroutine(Player player)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            yield break;  // �R���[�`�����I�����܂��B
        }

        Debug.Log("block");
        if (player.CompareTag("Player"))
        {
            GameManager.status = GameManager.GAME_STATUS.Pause;
            // ���b�ҋ@
            yield return new WaitForSeconds(0.25f);
            player.TakeDamage();
        }
    }

    public void Enter(Player player)
    {
        //if (player.colorType != Player.ColorType.Normal && (int)player.colorType == (int)this.colorType)
        //{
        //    return;
        //}
        StartCoroutine(EnterCoroutine(player));
    }

    //private void SetColor(ColorType type)
    //{
    //    if (colorMaterials.Count == 0)
    //    {
    //        Debug.LogError("Color materials are not assigned.");
    //        return;
    //    }

    //    // �F�ɑΉ�����}�e���A����I��
    //    int materialIndex = (int)type % colorMaterials.Count;
    //    Material selectedMaterial = colorMaterials[materialIndex];

    //    // �}�e���A����ύX
    //    Renderer renderer = GetComponent<Renderer>();
    //    if (renderer != null)
    //    {
    //        renderer.material = selectedMaterial;
    //    }
    //}
}