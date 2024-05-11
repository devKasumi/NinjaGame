using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            //if (collision.tag == "Enemy")
            //{
            //    FindObjectOfType<Player>().CountAttackFromPlayer();
            //}
            collision.GetComponent<Character>().OnHit(30f);
        }
    }
}
