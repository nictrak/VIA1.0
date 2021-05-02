using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private PlayerAttackHitbox playerAttackHitbox;
    private IsometricCharacterRenderer.States currentAttackState;
    private IsometricCharacterRenderer.States animatedAttackState;
    private int attackCounter;
    [SerializeField]
    private List<int> attackFrames;
    private int maxAttackState;
    private AttackHash attackHash;
    private Dictionary<int, IsometricCharacterRenderer.States> stateHash;

    //new one
    [SerializeField]
    private string currentAttackString;
    [SerializeField]
    private string animatedAttackString;

    // Start is called before the first frame update
    void Start()
    {
        playerAttackHitbox = GetComponent<PlayerAttackHitbox>();
        attackHash = GetComponent<AttackHash>();
        currentAttackState = IsometricCharacterRenderer.States.none;
        animatedAttackState = IsometricCharacterRenderer.States.none;
        attackCounter = 0;
        maxAttackState = 3;
        currentAttackString = "";
        animatedAttackString = "";
        setupStateHash();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void KeyAttack(IsometricCharacterRenderer isoRenderer)
    {
        if (Input.GetKeyDown(KeyCode.Z) && (int)currentAttackState < maxAttackState)
        {
            currentAttackState += 1;
        }
        playerAttackHitbox.UpdateAllHitboxOffset(isoRenderer.LastDirection);
    }
    public void KeyAttack2(IsometricCharacterRenderer isoRenderer)
    {
        if (currentAttackString.Length < maxAttackState)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentAttackString = currentAttackString + "z";
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentAttackString = currentAttackString + "x";
            }
        }
        playerAttackHitbox.UpdateAllHitboxOffset(isoRenderer.LastDirection);
    }
    public void AttackPerFrame(Vector2 inputVector, Vector2 movement, IsometricCharacterRenderer isoRenderer)
    {
        if (currentAttackState == IsometricCharacterRenderer.States.none)
        {
            isoRenderer.SetDirection(movement);
        }
        if (currentAttackState > animatedAttackState)
        {
            animatedAttackState += 1;
            isoRenderer.AttackDirection(inputVector, animatedAttackState);
            attackCounter = 0;
            playerAttackHitbox.HitboxDealDamage((int)animatedAttackState - 1);
        }
        else if (currentAttackState > 0)
        {
            if (attackCounter > attackFrames[(int)currentAttackState - 1])
            {
                currentAttackState = IsometricCharacterRenderer.States.none;
                animatedAttackState = IsometricCharacterRenderer.States.none;
                attackCounter = 0;
            }
            else
            {
                attackCounter++;
            }
        }
    }
    public void AttackPerFrame2(Vector2 inputVector, Vector2 movement, IsometricCharacterRenderer isoRenderer)
    {
        int curL = currentAttackString.Length;
        int aniL = animatedAttackString.Length;
        int id;
        if (animatedAttackString == "")
        {
            isoRenderer.SetDirection(movement);
        }
        if (curL > aniL)
        {
            animatedAttackString = currentAttackString.Substring(0, aniL + 1);
            id = attackHash.getKeyIndex(animatedAttackString);
            if(id != -1)
            {
                isoRenderer.AttackDirection(inputVector, stateHash[id]);
            }
            else
            {
                resetAttackString();
            }
            attackCounter = 0;
            if (id != -1)  playerAttackHitbox.HitboxDealDamage(id);
        }
        else if (curL > 0)
        {
            id = attackHash.getKeyIndex(animatedAttackString);
            if(id != -1)
            {
                if (attackCounter > attackFrames[id])
                {
                    resetAttackString();
                }
                else
                {
                    attackCounter++;
                }
            }
            else
            {
                resetAttackString();
            }
        }
    }
    private void resetAttackString()
    {
        currentAttackString = "";
        animatedAttackString = "";
        attackCounter = 0;
    }
    public bool IsAttack()
    {
        return currentAttackState != IsometricCharacterRenderer.States.none;
    }
    public bool IsAttack2()
    {
        return currentAttackString.Length > 0;
    }
    private void setupStateHash()
    {
        stateHash = new Dictionary<int, IsometricCharacterRenderer.States>();
        stateHash.Add(0, IsometricCharacterRenderer.States.first);
        stateHash.Add(1, IsometricCharacterRenderer.States.second);
        stateHash.Add(2, IsometricCharacterRenderer.States.third);
        stateHash.Add(3, IsometricCharacterRenderer.States.third);
    }
}