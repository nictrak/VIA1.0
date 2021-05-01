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
    private string attackString;

    // Start is called before the first frame update
    void Start()
    {
        playerAttackHitbox = GetComponent<PlayerAttackHitbox>();
        currentAttackState = IsometricCharacterRenderer.States.none;
        animatedAttackState = IsometricCharacterRenderer.States.none;
        attackCounter = 0;
        maxAttackState = 3;
        attackString = "";
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
                attackString = "";
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
}