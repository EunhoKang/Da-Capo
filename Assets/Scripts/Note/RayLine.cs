using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Note")){
            other.gameObject.SetActive(false);
            NoteManager.instance.JudgeSend(3);
        }
    }
}
