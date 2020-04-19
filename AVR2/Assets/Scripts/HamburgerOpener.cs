using UnityEngine;
using UnityEngine.UI;

public class HamburgerOpener : MonoBehaviour
{
    public Animator[] buttonAnimators;
    public bool isOpen;

    void Start()
    {
        isOpen = false;
        this.GetComponent<Button>().onClick.AddListener(ToggleHamburger);
    }
    
    // turning on/off the buttons "hidden" under the hamburger button
    public void ToggleHamburger()
    {
        if (!isOpen)
        {
            foreach(Animator anim in buttonAnimators)
            {
                anim.Play("ButtonAppear");
            }
            isOpen = true;
        }
        else
        {
            foreach (Animator anim in buttonAnimators)
            {
                anim.Play("ButtonDisappear");
            }
            isOpen = false;
        }
    }
}
