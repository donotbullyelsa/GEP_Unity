using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCount : MonoBehaviour
{
    private TextMeshProUGUI text_component;
    private int count;

    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
        text_component.enabled = false;
    }

    void Update()
    {
        // Sometimes Start() is not called for unknown reason
        text_component = GetComponent<TextMeshProUGUI>();
        
        text_component.enabled = (count >= 2);
        text_component.text = count.ToString();
    }

    public void SetCount(int n)
    {
        count = n;
    }
}
