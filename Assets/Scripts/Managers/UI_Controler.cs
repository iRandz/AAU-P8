using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UI_Controler : MonoBehaviour
{

    [SerializeField] private GameObject blackness;
    [SerializeField] private GameObject thanksText;
    

    // Start is called before the first frame update
    void Start()
    {
        thanksText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [ContextMenu("THIS")]
    public void FadeToBlack()
    {
        StartCoroutine(FadeToFromBlack(true));
    }
    public IEnumerator FadeToFromBlack(bool fadeToBlack, int fadeSpeed = 2)
    {
        Color objectColor = blackness.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            thanksText.SetActive(true);
            while (blackness.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackness.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            thanksText.SetActive(false);
            while (blackness.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackness.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
}
