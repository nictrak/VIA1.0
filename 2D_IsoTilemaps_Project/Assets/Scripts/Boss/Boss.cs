using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int IdleFrame;
    [SerializeField]
    private int ShakeFrame;
    [SerializeField]
    private int ShootFrame;
    [SerializeField]
    private Vector3 ShakeScale;

    private int frameCounter;
    private Vector3 notShakeScale;
    private bool isShakeScale;
    private ShootingBullet shootingBullet;
    public enum BossState
    {
        Idle,
        Shake,
        Shoot
    }
    private BossState currentState; // current state of boss
    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.Idle;
        frameCounter = 0;
        isShakeScale = false;
        notShakeScale = new Vector3(1,1,1);
        shootingBullet = GetComponent<ShootingBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        RunStateMachinePerFrame();
        ShakePerFrame();
        ShootPerFrame();
    }
    private void RunStateMachinePerFrame()
    {
        if(currentState == BossState.Idle)
        {
            if (frameCounter >= IdleFrame)
            {
                currentState = BossState.Shake;
                frameCounter = 0;
            }
            else
            {
                frameCounter++;
            }
        }
        else if (currentState == BossState.Shake)
        {
            if (frameCounter >= ShakeFrame)
            {
                currentState = BossState.Shoot;
                transform.localScale = notShakeScale;
                frameCounter = 0;
            }
            else
            {
                frameCounter++;
            }
        }
        else if (currentState == BossState.Shoot)
        {
            if (frameCounter >= ShootFrame)
            {
                currentState = BossState.Idle;
                frameCounter = 0;
            }
            else
            {
                frameCounter++;
            }
        }
    }
    private void ShakePerFrame()
    {
        if(currentState == BossState.Shake)
        {
            if (isShakeScale) { transform.localScale = notShakeScale; isShakeScale = false; }
            else { transform.localScale = ShakeScale; isShakeScale = true; }
        }
    }
    private void ShootPerFrame()
    {
        if(currentState == BossState.Shoot)
        {
            shootingBullet.SpawnBullets();
        }
    }
}
