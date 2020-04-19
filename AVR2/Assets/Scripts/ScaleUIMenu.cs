using UnityEngine;
using UnityEngine.UI;

public class ScaleUIMenu : MonoBehaviour
{
    // the class responsible for scaling the bottom menu part of the application to be 1/6 height of the device's screen;
    // this screen part is then blocking ar raycasting from placing the spawn marker, so that the user does change its location
    // while placing a point

    public RawImage UIMenu;

    void Start()
    {
        RectTransform rt = UIMenu.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(Screen.width, Screen.height / 6);
    }
}
