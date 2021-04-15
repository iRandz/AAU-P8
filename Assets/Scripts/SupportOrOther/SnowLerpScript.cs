using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLerpScript : MonoBehaviour
{
    [SerializeField] private List<Material> mat;
    [SerializeField] private float targetOpacity;
    [SerializeField] private float snowRampTime;
    [SerializeField] private ParticleSystem snow;
    
    private static readonly int SnowOpacity = Shader.PropertyToID("Vector1_970c929efc2a48c9ba2244bd13dfa8aa");

    private void OnApplicationQuit()
    {
        foreach (var instanceMat in mat)
        {
            instanceMat.SetFloat(SnowOpacity, 0);
        }
    }

    // Private functions
    private IEnumerator SnowStart()
    {
        snow.Play();
        float t = 0f;
        float timeAdd = 1/snowRampTime;
        while (t < 1)
        {
            foreach (var instanceMat in mat)
            {
                instanceMat.SetFloat(SnowOpacity, Mathf.Lerp(0, targetOpacity, t));
            }
            t += timeAdd * Time.deltaTime;
            yield return null;
        }
    }
    
    // Public functions
    [ContextMenu("TestSnow")]
    private void StartSnow()
    {
        StartCoroutine(SnowStart());
    }
}
