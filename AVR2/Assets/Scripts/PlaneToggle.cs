using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class PlaneToggle : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    
    public Button arPlaneSwitch;

    private bool isToggledOff;

    private void Awake()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arPlaneSwitch.onClick.AddListener(TogglePlane);
        isToggledOff = false;
    }

    // method to turn on/off the ar planes
    public void TogglePlane()
    {
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(!plane.isActiveAndEnabled);
        }

        arPlaneManager.enabled = !arPlaneManager.enabled;

        isToggledOff = !isToggledOff;

        // setting the colour and the text on the plane toggle button accordingly
        if (isToggledOff)
        {
            arPlaneSwitch.GetComponent<Image>().color = new Color32(255, 70, 60, 255);
            arPlaneSwitch.transform.GetChild(1).GetComponent<Text>().text = "Planes Off";
        }
        else
        {
            arPlaneSwitch.GetComponent<Image>().color = new Color32(60, 255, 60, 255);
            arPlaneSwitch.transform.GetChild(1).GetComponent<Text>().text = "Planes On";
        }
    }
}