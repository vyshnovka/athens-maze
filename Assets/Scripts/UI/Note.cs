using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    public void ShowLevelInfo(string text) => GetComponent<TextMeshProUGUI>().text += text;
}
