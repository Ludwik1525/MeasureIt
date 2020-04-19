using UnityEngine;
using UnityEngine.UI;

public class ExitApp : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(QuitApp);
    }
    
    // quitting the application
    void QuitApp()
    {
        Application.Quit();
    }
}
