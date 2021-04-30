using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{

    [SerializeField] private VisualEffect _visualEffect;

    [ContextMenu("Play")]
    public void playVFX()
    {
        _visualEffect.Play();
    }
}
