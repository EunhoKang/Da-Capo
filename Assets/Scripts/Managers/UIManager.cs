using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance=null;
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

    public List<GameObject> CanvasPrefabs;
    public Loading loadingPref;
    public Camera camUI;
    [Header("UI clip")]
    public AudioClip defaultSound;
    public AudioClip selectSound;
    private List<GameObject> Canvases=new List<GameObject>();
    private Loading loading;

    public void Init()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        float height = Screen.height;
        Screen.SetResolution((int)(height * 16 / 9), (int)height, false);
        StartCoroutine(_Init());
    }

    IEnumerator _Init()
    {
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("UIScene"));
        for (int i = 0; i < CanvasPrefabs.Count; i++)
        {
            GameObject temp = Instantiate(CanvasPrefabs[i]);
            Canvases.Add(temp);
            temp.SetActive(false);
            yield return null;
        }
        loading=Instantiate(loadingPref) as Loading;
        loading.gameObject.SetActive(false);
        ShowCanvas(0);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
    }

    public void SetFalseUICam(){
        camUI.gameObject.SetActive(false);
    }
    public void SetTrueUICam(){
        camUI.gameObject.SetActive(true);
    }

    public void DefaultSound(){
        SoundManager.instance.SFXPlay(defaultSound,0);
    }
    public void SelectSound(){
        SoundManager.instance.SFXPlay(selectSound,1);
    }

    public void RemoveCanvas(int index)
    {
        Canvases[index].SetActive(false);
    }
    public void ShowCanvas(int index)
    {
        Canvases[index].SetActive(true);
    }
    public void LoadingStart(){
        loading.gameObject.SetActive(true);
        loading.BlackOut();
    }
    public void LoadingEnd(){
        if(IsLoading()){
            loading.WhiteOut();
        }
    }
    public bool IsLoading(){
        return loading.gameObject.activeInHierarchy;
    }
}
