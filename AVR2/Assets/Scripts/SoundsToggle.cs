using UnityEngine;
using UnityEngine.UI;

public class SoundsToggle : MonoBehaviour
{
    private bool isToggledOff;

    private AudioSource source;

    public AudioClip sound;

    public Button[] buttons;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
        this.GetComponent<Button>().onClick.AddListener(ToggleSounds);

        // sounds turned on on default
        isToggledOff = false;

        // assigning all the buttons in the app the method to play sounds on click
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void ToggleSounds()
    {
        isToggledOff = !isToggledOff;

        // changing the colour and the text on the sound button accordingly
        if (isToggledOff)
        {
            this.GetComponent<Image>().color = new Color32(255, 70, 60, 255);
            this.transform.GetChild(1).GetComponent<Text>().text = "Sounds Off";
        }
        else
        {
            this.GetComponent<Image>().color = new Color32(60, 255, 60, 255);
            this.transform.GetChild(1).GetComponent<Text>().text = "Sounds On";
        }
    }

    // playing the sound
    void PlaySound()
    {
        if(!isToggledOff)
        {
            source.PlayOneShot(sound);
        }
    }
}
