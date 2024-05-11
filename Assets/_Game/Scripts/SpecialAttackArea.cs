using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(60f);
        }
    }
}
