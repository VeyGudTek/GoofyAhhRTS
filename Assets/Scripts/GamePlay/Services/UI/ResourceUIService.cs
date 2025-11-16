using UnityEngine;
using TMPro;

public class ResourceUIService : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ResourceText;

    public void UpdateResourceText(float value)
    {
        ResourceText.text = $"Resources: {value.ToString()}";
    }
}
