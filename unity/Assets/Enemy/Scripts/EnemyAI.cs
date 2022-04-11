using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //How fast the enemy moves
    public float Speed;
    //The distance before the enemy sees the player
    public float Sight;
    //The range before the enemy attacks
    public float AttackRange;
    //The speed at which the attack recharges
    public float AttackSpeed;
    //How fast the enemy rotates
    public float RotateSpeed;
    //Attack cooldown
    public float AttackCool;

    virtual public void Die()
    {

    }
}
