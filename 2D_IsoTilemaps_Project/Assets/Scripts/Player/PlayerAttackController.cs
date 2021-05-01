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

    //new one
    private string currentAttackString;
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
        if (Input.GetKeyDown(KeyCode.Z) && currentAttackString.Length < maxAttackState)
        {
            currentAttackString = currentAttackString + "z";
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
        if (currentAttackString == "")
        {
            isoRenderer.SetDirection(movement);
        }
        if (curL > aniL)
        {
            animatedAttackString = currentAttackString.Substring(0, aniL + 1);
            id = attackHash.getKeyIndex(animatedAttackString);
            isoRenderer.AttackDirection(inputVector, (IsometricCharacterRenderer.States) (id+1));
            attackCounter = 0;
            Debug.Log(id);
            playerAttackHitbox.HitboxDealDamage(id);
        }
        else if (curL > 0)
        {
            id = attackHash.getKeyIndex(animatedAttackString);
            if (attackCounter > attackFrames[id])
            {
                currentAttackString = "";
                animatedAttackString = "";
                attackCounter = 0;
            }
            else
            {
                attackCounter++;
            }
        }
    }
    public bool IsAttack()
    {
        return currentAttackState != IsometricCharacterRenderer.States.none;
    }
    public bool IsAttack2()
    {
        return currentAttackString.Length > 0;
    }
}