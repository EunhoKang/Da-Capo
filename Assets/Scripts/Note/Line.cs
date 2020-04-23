using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public GameObject rayPoint;
    
    public void OnEnable()
    {
        rayPoint.transform.localPosition=
        NoteManager.instance.rayStartPoint.position-NoteManager.instance.heartTransform.position;
    }
}
