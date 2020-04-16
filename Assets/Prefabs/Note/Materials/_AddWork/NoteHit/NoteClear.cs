using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteClear : MonoBehaviour
{
    Material material;

    bool isDissolving = false;
    float fade = 0f;
    float maxFade = 0.7f;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        //StartCoroutine(NoteFadeOut());
    }
    /*
    IEnumerator NoteFadeOut()
    {
        yield return new WaitForEndOfFrame();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
    }
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isDissolving = true;
        }
        if(isDissolving)
        {
            fade += Time.deltaTime * 1.5f;

            if(fade >= maxFade)
            {
                fade = maxFade;
                isDissolving = false;
            }

            material.SetFloat("_Fade", fade);
        }
    }
}
