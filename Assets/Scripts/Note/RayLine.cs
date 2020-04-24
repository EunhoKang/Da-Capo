using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Note")){
            other.GetComponent<Note>().NoteHitted(3);
            NoteManager.instance.JudgeSend(3);
        }
    }
}
