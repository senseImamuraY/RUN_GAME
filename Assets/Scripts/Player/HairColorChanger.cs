using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HairColorChanger : MonoBehaviour
{
    [SerializeField]
    Material hair;

    [SerializeField]
    Texture normalHair, redHair, greenHair, violetHair;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomHairColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRandomHairColor()
    {
        Texture randomHairTexture = GetRandomHairTexture();
        hair.SetTexture("_MainTex", randomHairTexture);
    }

    private Texture GetRandomHairTexture()
    {
        Texture[] hairTextures = { normalHair, redHair, greenHair, violetHair };
        int randomIndex = Random.Range(0, hairTextures.Length);
        return hairTextures[randomIndex];
    }
}
