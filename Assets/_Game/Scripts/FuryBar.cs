using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuryBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    float furyAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(furyAmount, imageFill.fillAmount, Time.deltaTime * 5f);
    }

    public void OnInit(float furyAmount)
    {
        imageFill.fillAmount = 0;
        this.furyAmount = furyAmount;
    }
}
