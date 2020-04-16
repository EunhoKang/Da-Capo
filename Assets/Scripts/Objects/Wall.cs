using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            CharacterManager.instance.PlayerGotoCenter();
            CharacterManager.instance.PlayerGetDamage(StageManager.instance.stagefile.wallDamage);
        }
    }
}
