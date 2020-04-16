using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Page : MonoBehaviour
{
    public abstract void Appear(int init,int after);
    public abstract void Disappear(int init,int after);
}
