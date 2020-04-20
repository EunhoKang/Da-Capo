using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerInside : MonoBehaviour
{
    [HideInInspector] public float damage;
    bool isHitPlayer;
    public void OnEnable(){
        isHitPlayer=false;
    }
    protected void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            if(isHitPlayer){
                return;
            }
            CharacterManager.instance.PlayerGetDamage(damage);
            isHitPlayer=true;
        }
    }
}
