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
        // GetComponentメソッドを使ってCustomColliderを取得し、Enemyプロパティを設定します
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
            yield break;  // コルーチンを終了します。
        }

        Debug.Log("block");
        if (player.CompareTag("Player"))
        {
            GameManager.status = GameManager.GAME_STATUS.Pause;
            // 数秒待機
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

    //    // 色に対応するマテリアルを選択
    //    int materialIndex = (int)type % colorMaterials.Count;
    //    Material selectedMaterial = colorMaterials[materialIndex];

    //    // マテリアルを変更
    //    Renderer renderer = GetComponent<Renderer>();
    //    if (renderer != null)
    //    {
    //        renderer.material = selectedMaterial;
    //    }
    //}
}