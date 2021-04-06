using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int idleFrame;
    [SerializeField]
    private int shakeFrame;
    [SerializeField]
    private int shootFrame;
    [SerializeField]
    private Vector3 shakeScale;
    [SerializeField]
    private int basicShootRateFrame;
    [SerializeField]
    private LinearBullet basicBullet;
    [SerializeField]
    private float basicBulletVelocity;
    [SerializeField]
    private int basicBulletDamage;
    [SerializeField]
    private int heavyShootRateFrame;
    [SerializeField]
    private LinearBullet heavyBullet;
    [SerializeField]
    private float heavyBulletVelocity;
    [SerializeField]
    private int heavyBulletDamage;
    [SerializeField]
    private int rapidShootRateFrame;
    [SerializeField]
    private GameObject spawner1;
    [SerializeField]
    private GameObject spawner2;
    [SerializeField]
    private List<Vector2> starDirections;

    private int frameCounter;
    private Vector3 notShakeScale;
    private bool isShakeScale;
    private int shootCounter;
    private int shootCounter2;
    private ShootState shootState;
    private GameObject player;

    public enum BossState
    {
        Idle,
        Shake,
        Shoot
    }
    public enum ShootState
    {
        Basic,
        Heavy,
        Rapid,
        Star,
        Mix
    }
    private BossState currentState; // current state of boss
    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.Idle;
        frameCounter = 0;
        isShakeScale = false;
        notShakeScale = new Vector3(1/2,1/2,1/2);
        shootCounter = 0;
        shootCounter2 = 0;
        shootState = ShootState.Basic;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        RunStateMachinePerFrame();
        ShakePerFrame();
        BasicShootPerFrame();
        HeavyShootPerFrame();
        RapidShootPerFrame();
        StarShootPerFrame();
        MixShootPerFrame();
    }
    private void RunStateMachinePerFrame()
    {
        if(currentState == BossState.Idle)
        {
            if (frameCounter >= idleFrame)
            {
                currentState = BossState.Shake;
                frameCounter = 0;
            }
        }
        else if (currentState == BossState.Shake)
        {
            if (frameCounter >= shakeFrame)
            {
                currentState = BossState.Shoot;
                transform.localScale = notShakeScale;
                frameCounter = 0;
            }
        }
        else if (currentState == BossState.Shoot)
        {
            if (frameCounter >= shootFrame)
            {
                currentState = BossState.Idle;
                NextShootState();
                frameCounter = 0;
            }
        }
        frameCounter++;
    }
    private void ShakePerFrame()
    {
        if(currentState == BossState.Shake)
        {
            if (isShakeScale) { transform.localScale = notShakeScale; isShakeScale = false; }
            else { transform.localScale = shakeScale; isShakeScale = true; }
        }
    }
    private void BasicShootPerFrame()
    {
        if(currentState == BossState.Shoot && shootState == ShootState.Basic)
        {
            if(shootCounter > basicShootRateFrame)
            {
                //shoot
                LinearBullet spawned = Instantiate<LinearBullet>(basicBullet);
                spawned.Setup(spawner1.transform.position, player.transform.position, basicBulletVelocity, basicBulletDamage);
                shootCounter = 0;
            }
            else
            {
                shootCounter++;
            }
        }
    }
    private void HeavyShootPerFrame()
    {
        if (currentState == BossState.Shoot && shootState == ShootState.Heavy)
        {
            if (shootCounter > basicShootRateFrame)
            {
                //shoot
                LinearBullet spawned = Instantiate<LinearBullet>(heavyBullet);
                spawned.Setup(spawner2.transform.position, player.transform.position, basicBulletVelocity, basicBulletDamage);
                shootCounter = 0;
            }
            else
            {
                shootCounter++;
            }
        }
    }
    private void RapidShootPerFrame()
    {
        if (currentState == BossState.Shoot && shootState == ShootState.Rapid)
        {
            if (shootCounter > rapidShootRateFrame)
            {
                //shoot
                LinearBullet spawned = Instantiate<LinearBullet>(basicBullet);
                spawned.Setup(spawner1.transform.position, player.transform.position, basicBulletVelocity, basicBulletDamage);
                shootCounter = 0;
            }
            else
            {
                shootCounter++;
            }
        }
    }
    private void StarShootPerFrame()
    {
        if (currentState == BossState.Shoot && shootState == ShootState.Star)
        {
            if (shootCounter > rapidShootRateFrame)
            {
                //shoot
                for(int i = 0; i < starDirections.Count; i++)
                {
                    LinearBullet spawned = Instantiate<LinearBullet>(basicBullet);
                    Vector2 targetPoint = (Vector2)spawner1.transform.position + starDirections[i];
                    spawned.Setup(spawner1.transform.position, targetPoint, basicBulletVelocity, basicBulletDamage);
                }
                shootCounter = 0;
            }
            else
            {
                shootCounter++;
            }
        }
    }
    private void MixShootPerFrame()
    {
        if (currentState == BossState.Shoot && shootState == ShootState.Mix)
        {
            if (shootCounter > rapidShootRateFrame)
            {
                //shoot
                LinearBullet spawned;
                spawned = Instantiate<LinearBullet>(basicBullet);
                spawned.Setup(spawner1.transform.position, player.transform.position, basicBulletVelocity, basicBulletDamage);
                for (int i = 0; i < starDirections.Count; i++)
                {
                    spawned = Instantiate<LinearBullet>(basicBullet);
                    Vector2 targetPoint = (Vector2)spawner1.transform.position + starDirections[i];
                    spawned.Setup(spawner1.transform.position, targetPoint, basicBulletVelocity, basicBulletDamage);
                }
                shootCounter = 0;
            }
            else
            {
                shootCounter++;
            }
            if (shootCounter2 > basicShootRateFrame)
            {
                //shoot
                LinearBullet spawned = Instantiate<LinearBullet>(heavyBullet);
                spawned.Setup(spawner2.transform.position, player.transform.position, basicBulletVelocity, basicBulletDamage);
                shootCounter2 = 0;
            }
            else
            {
                shootCounter2++;
            }
        }
    }
    private void NextShootState()
    {
        if((int) shootState == 4)
        {
            shootState = 0;
        }
        else
        {
            shootState++;
        }
    }
}
