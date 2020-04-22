using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject realCam;
    public Rigidbody2D rb;
    public List<SpriteRenderer> eventPos1;
    public List<SpriteRenderer> eventPos2;
    public Image[] health;
    public Image healthBar;
    public Image IllustPos;
    public Text centerText;
    public Transform heartTransform;
    public Transform noteSpawnTransform;
    public Transform rayStartPoint;

    public IEnumerator CameraShake(float horizontalMagnitude, float verticalMagnitude,int timerCount){
        Vector3 originalPos = transform.localPosition;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        while(current<timerCount+initial){
            Vector3 targetCenter=Vector3.zero;
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                float x = originalPos.x + Random.Range(-1f,1f) * horizontalMagnitude;
                float y = originalPos.y + Random.Range(-1f,1f) * verticalMagnitude;
                //fix this func before use this!
            }
            rb.MovePosition(Vector3.Lerp(transform.position,targetCenter,Time.deltaTime*2));
            yield return null;
        }
        transform.localPosition = originalPos;
    }
    public IEnumerator MoveCameraToDirectionSmoothlyCoroutine(Vector3 target, float waitTime,float duration){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        Vector3 targetCenter=transform.position+target;
        Vector3 initialPos=transform.position;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        
        float spb=StageManager.instance.spb;
        float rate=CameraManager.instance.rate;
        int maxTick=(int)(duration/rate);
        float maxTickReverse=1f/maxTick;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        Vector3 first=initialPos;
        Vector3 second=initialPos;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=Vector3.Lerp(initialPos,targetCenter,Mathf.Pow((current-initial)*maxTickReverse,0.5f));
                second=Vector3.Lerp(initialPos,targetCenter,Mathf.Pow((current-initial+1)*maxTickReverse,0.5f));
                rb.MovePosition(first);
            }
            yield return null;
        }
        
        transform.position=targetCenter;
    }

    [HideInInspector]public bool isSlowMotionStop=false;
    public IEnumerator MoveCameraSlowlyCoroutine(Vector3 direction, float waitTime, float speed=0.01f){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        isSlowMotionStop=false;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        Vector3 initialPos=transform.position;
        float spb=StageManager.instance.spb;
        float rate=CameraManager.instance.rate;
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        while(!isSlowMotionStop){
            delta+=Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(initialPos+direction*speed*(current-initial),
            initialPos+direction*speed*(current-initial+1),
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt)));
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                rb.MovePosition(initialPos+direction*speed*(current-initial));
            }
            yield return null;
        }
    }

    public IEnumerator StopCameraCoroutine(float waitTime){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        isSlowMotionStop=true;
    }

    public IEnumerator SpinCameraSmoothlyCoroutine(float waitTime,float duration,float angle){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        float initialAngle=realCam.transform.rotation.eulerAngles.z;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float spb=StageManager.instance.spb;
        float rate=CameraManager.instance.rate;
        int maxTick=(int)(duration/rate);
        float maxTickReverse=1f/maxTick;
        Vector3 temp=new Vector3(0,0,0);
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float first=initialAngle;
        float second=initialAngle;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            temp.z=Mathf.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            realCam.transform.rotation=Quaternion.Euler(temp);
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=Mathf.Lerp(initialAngle,initialAngle+angle,Mathf.Pow((current-initial)*maxTickReverse,0.5f));
                second=Mathf.Lerp(initialAngle,initialAngle+angle,Mathf.Pow((current-initial+1)*maxTickReverse,0.5f));
                temp.z=first;
                realCam.transform.rotation=Quaternion.Euler(temp);
            }
            yield return null;
        }
        temp.z=initialAngle+angle;
        realCam.transform.rotation=Quaternion.Euler(temp);
    }

    public IEnumerator SpinCameraVerySmoothly(float waitTime,float duration,float maxSpinSpeed,bool isClockwise){
        while(!StageManager.instance.isGameStart){
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        float initialAngle=realCam.transform.rotation.eulerAngles.z;
        int current=TimeManager.instance.checkpoint;
        int initial=current;
        float spb=StageManager.instance.spb;
        float rate=CameraManager.instance.rate;
        int maxTick=(int)(duration/rate);
        float maxTickReverse=1f/maxTick;
        Vector3 temp=new Vector3(0,0,0);
        float delta=0;
        float targetDelta=0;
        float currentMt=1;
        float first=initialAngle;
        float second=initialAngle;
        while(current<maxTick+initial){
            delta+=Time.deltaTime;
            temp.z=Mathf.Lerp(first,second,
            (delta*targetDelta)*(TimeManager.instance.multiplier*currentMt));
            realCam.transform.rotation=Quaternion.Euler(temp);
            if(TimeManager.instance.checkpoint>current){
                while(TimeManager.instance.checkpoint>current){
                    current++;
                    
                }
                delta=0;
                currentMt=1/(TimeManager.instance.multiplier);
                targetDelta=1/(rate*spb*currentMt);
                first=first+(isClockwise? 1f : -1f)*SpinSpeed(current-initial,maxTick,maxSpinSpeed);
                second=first+(isClockwise? 1f : -1f)*SpinSpeed(current-initial+1,maxTick,maxSpinSpeed);
                temp.z=first;
                realCam.transform.rotation=Quaternion.Euler(temp);
            }
            yield return null;
        }
    }
    float SpinSpeed(float current,float maxTick,float maxSpinSpeed){
        if(current<maxTick*0.5f){
            return Mathf.Lerp(0,maxSpinSpeed,current/(float)(maxTick*0.5f));
        }else if(current>maxTick*0.75f){
            return Mathf.Lerp(maxSpinSpeed,0,(current-maxTick*0.75f)/(float)(maxTick*0.25f));
        }else{
            return maxSpinSpeed;
        }
    }

}
