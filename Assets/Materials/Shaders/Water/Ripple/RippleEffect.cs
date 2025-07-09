using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class RippleEffect : MonoBehaviour
{
    public int TextureSize = 512;
    public RenderTexture ObjectsRT;
    private RenderTexture CurrRT, PrevRT, TempRT;
    public Shader RippleShader, AddShader;
    private Material RippleMat, AddMat;
    public ParticleSystem ripple;
    public GameObject player;
    private bool isCollision = false;


    // Start is called before the first frame update
    void Start()
    {
        //Creating render textures and materials
        CurrRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RFloat);
        PrevRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RFloat);
        TempRT = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RFloat);
        RippleMat = new Material(RippleShader);
        AddMat = new Material(AddShader);

        ripple = GetComponentInChildren<ParticleSystem>();

        //Change the texture in the material of this object to the render texture calculated by the ripple shader.
        GetComponent<Renderer>().material.SetTexture("_RippleTex", CurrRT);

        StartCoroutine(ripples());
    }
    private void Update()
    {

    }
    // Update is called once per frame
    IEnumerator ripples()
    {
        //Copy the result of blending the render textures to TempRT.
        AddMat.SetTexture("_ObjectsRT", ObjectsRT);
        AddMat.SetTexture("_CurrentRT", CurrRT);
        Graphics.Blit(null, TempRT, AddMat);

        RenderTexture rt0 = TempRT;
        TempRT = CurrRT;
        CurrRT = rt0;

        //Calculate the ripple animation using ripple shader.
        RippleMat.SetTexture("_PrevRT", PrevRT);
        RippleMat.SetTexture("_CurrentRT", CurrRT);
        Graphics.Blit(null, TempRT, RippleMat);
        Graphics.Blit(TempRT, PrevRT);

        //Swap PrevRT and CurrentRT to calculate the result for the next frame.
        RenderTexture rt = PrevRT;
        PrevRT = CurrRT;
        CurrRT = rt;

        //Wait for one frame and then execute again.
        yield return null;
        StartCoroutine(ripples());
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // Verifica si la colisi�n ocurri� con el objeto espec�fico con el que quieres activar las part�culas
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // Obtiene la posici�n de la colisi�n para emitir las part�culas en ese punto
    //        Vector3 collisionPoint = collision.GetContact(0).point;

    //        // Activa el sistema de part�culas en el punto de colisi�n
    //        ripple.transform.position = collisionPoint;
    //        ripple.Play();
    //    }
    //}

}
