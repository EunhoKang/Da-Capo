using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerInside : MonoBehaviour
{
    public float damage;
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            CharacterManager.instance.PlayerGetDamage(damage);
        }
    }
}
