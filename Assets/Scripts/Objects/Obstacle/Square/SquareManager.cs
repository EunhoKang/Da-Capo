using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private Square squarePrefab;
    private List<Square> squares;
    void Awake()
    {
        List<GameObject> temp=StageManager.instance.stagefile.usingObstacle;
        for(int i=0;i<temp.Count;i++){
            Square square=temp[i].GetComponent<Square>();
            if(square!=null){
                squarePrefab=square;
            }
        }
        squares=new List<Square>();
        for(int i=0; i<4; i++){
            Square obj=Instantiate(squarePrefab) as Square;
            obj.gameObject.SetActive(false);
            squares.Add(obj);
        }
    }

    public void Command_1(float time){
        StartCoroutine(SpawnSquare(time));
    }

    IEnumerator SpawnSquare(float waitTime){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        float warnTime=squarePrefab.warnTime;
        yield return new WaitForSeconds(waitTime-warnTime);
        Square temp=getSquare();
        temp.gameObject.SetActive(true);
        temp.Loom();
        yield return new WaitForSeconds(warnTime);
        temp.Appear();
        temp.transform.position=CharacterManager.instance.FindPlayerForEnemy();
    }

    public Square getSquare(){
		for(int i=0;i<squares.Count;i++){
			if(!squares[i].gameObject.activeInHierarchy){
				return squares[i];
			}
		}
		Square obj=Instantiate(squarePrefab) as Square;
		obj.gameObject.SetActive(false);
		squares.Add(obj);
		return obj;
	}
    public void EndObject(){
        StopAllCoroutines();
        for(int i=0;i<squares.Count;i++){
            Destroy(squares[i].gameObject);
        }
        squares.Clear();
        squarePrefab=null;
    }
}
