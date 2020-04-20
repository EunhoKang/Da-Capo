using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject silhouette;
    public Slider healthSlider;
    public ParticleSystem move;
    public ParticleSystem hitParticle;
    public ParticleSystem spark;
    public ParticleSystem smoke;
    public Image fill;
    public SpriteRenderer sr;
    public GameObject min;
    public GameObject hour;
    public GameObject maxDistanceBar;
    [HideInInspector]public float rate;
    private float health;
    private WaitForSeconds tptime;
    private Rigidbody2D rb;
    private bool isImmuneByMove;
    private bool isImmuneByHit;
    private float minInitRotation;
    private float hourInitRotation;
    private IEnumerator moveCoroutine;
    private bool isSilhouetteOn=false;
    private float spb;
    public void Setup(){
        rb=GetComponent<Rigidbody2D>();
        rate=StageManager.instance.stagefile.metronomeRate;
        health=StageManager.instance.stagefile.playerHealth;
        spb=StageManager.instance.spb;
        isImmuneByHit=false;
        isImmuneByMove=false;
        tptime=new WaitForSeconds(0.015625f*spb);
        healthSlider.maxValue=health;
        healthSlider.value=health;
        healthSlider.gameObject.SetActive(false);
        minInitRotation=min.transform.rotation.z;
        hourInitRotation=hour.transform.rotation.z;
        #if UNITY_ANDROID && !UNITY_EDITOR
        maxDistanceBar.SetActive(false);
        maxDistanceBar.transform.localScale*=CharacterManager.instance.dashDistance*0.4f;
        #else
        maxDistanceBar.SetActive(false);
        #endif
    }
    public void SetSilhouette(Vector3 pos){
        if(pos!=Vector3.zero){
            silhouette.transform.position=transform.position+pos;
            if(!isSilhouetteOn){
                silhouette.SetActive(true);
                isSilhouetteOn=true;
            }
        }
        else{
            silhouette.SetActive(false);
            isSilhouetteOn=false;
            silhouette.transform.position=transform.position;
        }
        
    }

    public void SpinNiddles(){
        StartCoroutine(SpinNiddlesCoroutine());
    }
    Vector3 temp=new Vector3(0,0,-180);
    public IEnumerator SpinNiddlesCoroutine(){
        float spb=StageManager.instance.spb;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        while(health>0){
            delta+=Time.deltaTime;
            min.transform.rotation=Quaternion.Euler(Vector3.Lerp(temp*rate*(current-initial),
            temp*rate*(current-initial+1),
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            hour.transform.rotation=Quaternion.Euler(Vector3.Lerp(temp*rate*(current-initial)*0.5f,
            temp*rate*(current-initial+1)*0.5f,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                min.transform.rotation=Quaternion.Euler(temp*rate*(current-initial));
                hour.transform.rotation=Quaternion.Euler(temp*rate*(current-initial)*0.5f);
            }
            ParticleSystem.MainModule tp=move.main;
            tp.simulationSpeed=TimeManager.instance.multiplier;
            yield return null;
        }
    }

    public void MovePlayer(Vector3 direction,float distance){
        if(moveCoroutine!=null){
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine=Move(direction,distance);
        StartCoroutine(moveCoroutine);
    }
    public IEnumerator Move(Vector3 direction,float distance){
		Vector3 target=transform.position+distance*direction;
        target.z=0;
        int temp=(int)(16f*Vector3.Magnitude(direction));
        float reverseTp;
        if(temp>10){
            temp=10;
        }
        if(temp>0){
            reverseTp=1f/(float)temp;
        }else{
            reverseTp=0;
        }
        isImmuneByMove=true;
		for(int i=1;i<=temp;i++){
            if(health>0){
			    rb.MovePosition(Vector3.Lerp(transform.position,target,reverseTp*i));
            }
            if(i>4 || i==temp){
                isImmuneByMove=false;
            }
			yield return tptime;
		}
        isImmuneByMove=false;
        moveCoroutine=null;
        yield return tptime;
        CharacterManager.instance.UpdatePlayerPos(transform.position);
	}
    public void PlayerDamaged(float damage){
        if(isImmuneByMove || isImmuneByHit) return;
        hitParticle.Play();
        health-=damage;
        StartImmune();
        CountManager.instance.DiscountScoreFromHit(damage);
        StageManager.instance.ingameUI.UpdateHealthSlider(health);
        if(health<=0){
            Dead();
        }
    }

    public void PlayerHealed(float amount){
        health+=amount;
        StageManager.instance.ingameUI.UpdateHealthSlider(health);
    }

    private void Dead(){
        CharacterManager.instance.PlayerDead();
        smoke.Play();
        spark.Play();
        CountManager.instance.stageCleared=false;
    }

    public void StartImmune(){
        StartCoroutine(Immune());
        StartCoroutine(ShowHealth());
    }

    public IEnumerator Immune()
    {
        Color c;
        isImmuneByHit=true;
        WaitForSeconds immuneAnim = new WaitForSeconds(0.25f*StageManager.instance.spb);
        for (int i = 0; i < 6; i++)
        {
            c = sr.color;
            c.a = 0.5f * (i % 2 + 1);
            sr.color = c;
            yield return immuneAnim;
        }
        c = sr.color;
        c.a = 1;
        sr.color = c;
        isImmuneByHit=false;
    }
    public IEnumerator ShowHealth(){
        Color c=fill.color;
        Vector3 temp=new Vector3(1,1,1);
        healthSlider.gameObject.SetActive(true);
        healthSlider.value=health;
        for(float i=0.5f;i>=0; i-=0.05f){
            c.a=i;
            fill.color=c;
            fill.gameObject.transform.localScale=temp*(2-i*2);
            yield return null;
        }
        healthSlider.gameObject.SetActive(false);
    }
}
