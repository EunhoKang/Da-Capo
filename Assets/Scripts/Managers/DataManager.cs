using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}
	public JClass _data;
	public void Start(){
		DataLoad();
		SoundManager.instance.Init();
		UIManager.instance.Init();
	}
	public void OnApplicationQuit()
	{
		DataSave();
	}
	public void StageCleared(bool isCleared,int stageNum,int score,bool isNoMiss,bool isFullCombo){
		if(isCleared){
			_data.stages[stageNum-1].isCleared=true;
			if(score>_data.stages[stageNum-1].highestScore){
				_data.stages[stageNum-1].highestScore=score;
			}
			_data.stages[stageNum-1].isNoMiss=isNoMiss;
			_data.stages[stageNum-1].isFullCombo=isFullCombo;
			if(stageNum<_data.stages.Count){
				_data.stages[stageNum].isOpened=true;
			}
			DataSave();
		}
	}
	public void Setting(float bgm,float sfx,bool vib){
		_data.BGMvolume=bgm;
		_data.SFXvolume=sfx;
		_data.isVibOn=vib;
		DataSave();
	}
	public void Offsetting(float offset){
		_data.offset=offset;
		DataSave();
	}
	public void PrologueCount(int num){
		_data.prologues[num]=true;
		DataSave();
	}
	string ObjectToJson(object obj)
	{
		return JsonUtility.ToJson(obj);
	}
	T JsonToOject<T>(string jsonData)
	{
		return JsonUtility.FromJson<T>(jsonData);
	}
	void CreateJsonFile(string createPath, string fileName, string jsonData)
	{
		FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
		byte[] data = Encoding.UTF8.GetBytes(jsonData);
		fileStream.Write(data, 0, data.Length);
		fileStream.Close();
	}
	T LoadJsonFile<T>(string loadPath, string fileName)
	{
		FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
		byte[] data = new byte[fileStream.Length];
		fileStream.Read(data, 0, data.Length);
		fileStream.Close();
		string jsonData = Encoding.UTF8.GetString(data);
		return JsonUtility.FromJson<T>(jsonData);
	}
	public void DataLoad(){
		JClass jtc;
		try{
			jtc = LoadJsonFile<JClass>(Application.persistentDataPath, "epdlxjvkdlf");
			_data=jtc;
		}catch(IOException ex){
			Debug.Log(ex);
			CreateJsonFile(Application.persistentDataPath, "epdlxjvkdlf", ObjectToJson(new JClass(true,11)));
			jtc = LoadJsonFile<JClass>(Application.persistentDataPath, "epdlxjvkdlf");
			_data=jtc;
		}
	}
	public void DataSave(){
		if(_data==null) return;
		string jsonData = ObjectToJson(_data);
		if(jsonData==null) return;
		CreateJsonFile(Application.persistentDataPath, "epdlxjvkdlf", jsonData);
	}
	public void DeleteData(){
		JClass jc = new JClass(true,11);
		string jsonData = ObjectToJson(jc);
		CreateJsonFile(Application.persistentDataPath, "epdlxjvkdlf", jsonData);
	}
}
