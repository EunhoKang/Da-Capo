using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLazer : MonoBehaviour
{
    protected float damage;
    bool isHitPlayer;
    protected void OnEnable(){
        damage=StageManager.instance.stagefile.lazerDamage;
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
