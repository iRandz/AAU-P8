using UnityEngine;

public class EmissionController : MonoBehaviour
{
    private Material _material;
    
    // Start is called before the first frame update
    void Start()
    {
        _material = gameObject.GetComponent<Renderer>().material;
    }

    public void ChangeEmissionIntensity(float emission)
    {
       _material.EnableKeyword("_EMISSION");
       _material.SetFloat("_EmissionIntensity", emission);
    }
}
