using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackArea : MonoBehaviour
{
    private bool isRedSpecialAttack;

    public void SetRedSpecialAttack(bool isRedSpecialAttack)
    {
        this.isRedSpecialAttack = isRedSpecialAttack;
    }

    public bool GetRedSpecialAttack()
    {
        return this.isRedSpecialAttack; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            float damage = isRedSpecialAttack ? 90f : 60f;
            collision.GetComponent<Character>().OnHit(damage);
        }
    }
}
