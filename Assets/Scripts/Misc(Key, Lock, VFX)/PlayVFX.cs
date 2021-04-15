using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{

    [SerializeField] private VisualEffect _visualEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Play")]
    public void playVFX()
    {
        _visualEffect.Play();
    }
}
