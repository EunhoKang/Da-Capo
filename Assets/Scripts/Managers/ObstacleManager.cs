using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
    public List<GameObject> obstactlePrefabs;
    [Range(0f,3f)]
    public float warnTime=2.0f;
    [HideInInspector]public float rate;
    private List<bool> isSpawned;
    private List<GameObject> managers;
    public void Init(){
        rate=StageManager.instance.stagefile.metronomeRate;
        isSpawned=new List<bool>();
        managers=new List<GameObject>();
        for(int i=0;i<obstactlePrefabs.Count;i++){
            isSpawned.Add(false);
            managers.Add(null);
        }
    }

    public void GetAction(string command, float time){
        string[] temp=command.Split('^');
        int num=(int)float.Parse(temp[0]);
        if(num>=obstactlePrefabs.Count) return;
        if(!isSpawned[num]){
            managers[num]=Instantiate(obstactlePrefabs[num]);
            isSpawned[num]=true;
        }
        switch(num){
            case 0 :
                float[] array1={time,float.Parse(temp[2])};
                if(managers[num]==null) return;
                managers[num].SendMessage("Command_"+temp[1],array1);
                break;
            case 1 :
                float[] array2={time,float.Parse(temp[2]),float.Parse(temp[3]),float.Parse(temp[4]),float.Parse(temp[5])};
                if(managers[num]==null) return;
                managers[num].SendMessage("Command_"+temp[1],array2);
                break;
            case 2 :
                float[] array3={time,float.Parse(temp[2]),float.Parse(temp[3])};
                if(managers[num]==null) return;
                managers[num].SendMessage("Command_"+temp[1],array3);
                break;
            case 3 :
                if(managers[num]==null) return;
                managers[num].SendMessage("Command_"+temp[1],time);
                break;
            case 4 :
                float[] array4={time,float.Parse(temp[2]),float.Parse(temp[3]),float.Parse(temp[4]),float.Parse(temp[5])};
                if(managers[num]==null) return;
                managers[num].SendMessage("Command_"+temp[1],array4);
                break;
            case 5 :
                if(temp[1]=="1"){
                    float[] array5={time,float.Parse(temp[2]),float.Parse(temp[3]),float.Parse(temp[4])};
                    if(managers[num]==null) return;
                    managers[num].SendMessage("Command_"+temp[1],array5);
                }
                else{
                    if(managers[num]==null) return;
                    managers[num].SendMessage("Command_"+temp[1],time);
                }
                break;
            default :
                break;
        }
    }

    List<string> ParseCommand(string command){
        var values = Regex.Split(command, "^");
        List<string> temp= new List<string>();
        for(int j=0;j<values.Length;j++){
            temp.Add(values[j]);
        }
        return temp;
    }

    public void EndObstacle(){
        StopAllCoroutines();
        for(int i=0;i<managers.Count;i++){
            if(managers[i]!=null){
                managers[i].SendMessage("EndObject");
                Destroy(managers[i].gameObject);
                managers[i]=null;
            }
        }
        isSpawned.Clear();
        managers.Clear();
        //Add more
    }
}
