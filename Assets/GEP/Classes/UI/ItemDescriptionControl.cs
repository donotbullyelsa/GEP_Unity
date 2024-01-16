using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionControl : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdatePosition(Vector3 v3)
    {
        text.transform.position = v3;
    }

    public void SetEnabled(bool flag)
    {
        text.enabled = flag;
    }

    public void SetString(string str)
    {
        text.text = str;
    }
}
