using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    public void ShowLevelInfo(string text)
    {
        GetComponent<TextMeshProUGUI>().text += text;
    }
}
