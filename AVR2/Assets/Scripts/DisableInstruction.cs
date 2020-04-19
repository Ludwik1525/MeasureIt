using UnityEngine;
using UnityEngine.UI;

public class DisableInstruction : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(HideInstruction);
    }

    // hiding the launch instruction
    void HideInstruction()
    {
        this.gameObject.SetActive(false);
    }
}
