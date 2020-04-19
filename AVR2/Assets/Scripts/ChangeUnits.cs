using UnityEngine;
using UnityEngine.UI;

public class ChangeUnits : MonoBehaviour
{
    public GameObject unitsMenu;
    private Animator anim;

    public Button mm, cm, dm, m;
    public Button hamburgerButton;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OpenUnitsMenu);
        anim = unitsMenu.GetComponent<Animator>();

        mm.onClick.AddListener(delegate { SetUnit("MM"); });
        cm.onClick.AddListener(delegate { SetUnit("CM"); });
        dm.onClick.AddListener(delegate { SetUnit("DM"); });
        m.onClick.AddListener(delegate { SetUnit("M"); });
    }
    
    // opening menu with unit buttons
    void OpenUnitsMenu()
    {
        anim.Play("ButtonAppear");
        hamburgerButton.GetComponent<HamburgerOpener>().ToggleHamburger();
    }

    // setting a unit
    void SetUnit(string unit)
    {
        anim.Play("ButtonDisappear");
        FindObjectOfType<PlacePoints>().ChangeUnit(unit);
    }
}
