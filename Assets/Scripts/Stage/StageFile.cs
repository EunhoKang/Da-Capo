using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFile : MonoBehaviour
{
    public int stageNum;
    [Header("Song")]
    public AudioClip songFile;
    public float BPM=120;
    public float runTime=180;
    public float metronomeRate=0.0625f;
    public float offset=0.05f;

    [Header("BackGround")]
    public Sprite backgroundIllust;

    [Header("Note & Lyrics")]
    public TextAsset noteFile;
    public TextAsset lyrics;
    [Header("Player")]
    public Vector3 initPos=new Vector3(0,-3,0);
    public float dashLength=5;
    public float playerHealth=100;

    [Header("Enemy & Obstacle")]
    public float enemyBulletSpeed=8;
    public float bulletDamage=2;
    public float enemyLazerDuration=0.5f;
    public float lazerDamage=4;
    public float fixTime=0.125f;
    public float wallDamage=10f;
    public float obstacleDamage=8f;
    public GameObject bulletTypePrefab;
    public GameObject lazerTypePrefab;
    public enemyBullet bulletPrefab;
    public List<GameObject> usingObstacle;
}
