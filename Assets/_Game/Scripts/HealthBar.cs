using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image imageFill;
    [SerializeField] Vector3 offset;

    float hp;
    float maxhp;

    private Transform target;

    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp/maxhp, Time.deltaTime * 5f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;
        this.maxhp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
