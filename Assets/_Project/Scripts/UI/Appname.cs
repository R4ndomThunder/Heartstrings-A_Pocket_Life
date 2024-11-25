using TMPro;
using UnityEngine;

public class Appname : MonoBehaviour
{
    private TextMeshProUGUI titleName;
    private void Awake()
    {
        titleName = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        titleName.text = $"{Application.productName}";
    }
}
