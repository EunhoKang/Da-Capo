using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject spriteMask;
    public float gridWidth;
    public float gridHeight;
    public void InitAnimation(){
        StartCoroutine(ExpandMask());
    }
    IEnumerator ExpandMask(){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        spriteMask.transform.localScale=Vector3.zero;
        Vector3 tp=new Vector3(0.3f,0.3f,0.3f);
        for(int i=0;i<=84;i++){
            spriteMask.transform.localScale+=tp;
            yield return null;
        }
    }

    public void MoveToTargetDirection(Vector3 playerPos){
        Vector3 tp=Vector3.zero;
        if(playerPos.x-transform.position.x>gridWidth){
            tp.x=gridWidth;
        }else if(playerPos.x-transform.position.x<-1f*gridWidth){
            tp.x=-1f*gridWidth;
        }
        if(playerPos.y-transform.position.y>gridHeight){
            tp.y=gridHeight;
        }else if(playerPos.y-transform.position.y<-1f*gridHeight){
            tp.y=-1f*gridHeight;
        }
        transform.localPosition+=tp;
    }
}
