using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = FindObjectOfType<Player>();

        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            if (collision.tag == "Player")
            {
                player.CountAttackFromPlayer();
            }
            collision.GetComponent<Character>().OnHit(30f);
        }
    }
}
