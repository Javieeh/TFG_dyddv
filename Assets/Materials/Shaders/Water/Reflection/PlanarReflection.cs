using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanarReflection : MonoBehaviour
{
    private Vector2 Resolution;

    private Camera ReflectionCamera;
    [SerializeField] private RenderTexture ReflectionRenderTexture;
    [SerializeField] private int ReflectionResloution;
    private Transform waterTransform;

    private void Awake()
    {
        waterTransform = this.GetComponentInParent<Transform>();
        ReflectionCamera = this.GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        if(Vector3.Distance(this.transform.position, Camera.main.transform.position) < 20f)
        {
            ReflectionCamera.gameObject.SetActive(true);

            ReflectionCamera.fieldOfView = Camera.main.fieldOfView;

            ReflectionCamera.gameObject.transform.position = new Vector3(Camera.main.transform.position.x, (-Camera.main.transform.position.y + transform.position.y * 2), Camera.main.transform.position.z);
            ReflectionCamera.gameObject.transform.rotation = Quaternion.Euler(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0f);

            Resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

            ReflectionRenderTexture.Release();
            ReflectionRenderTexture.width = Mathf.RoundToInt(Resolution.x) * ReflectionResloution / Mathf.RoundToInt(Resolution.y);
            ReflectionRenderTexture.height = ReflectionResloution;
        } else ReflectionCamera.gameObject.SetActive(false);
        
    }
}
