using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager GetInstance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    //private void Awake()
    //{
    //    instance = this;
    //}

    [SerializeField] Text coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
