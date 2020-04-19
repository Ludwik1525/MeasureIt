using UnityEngine;

public class LabelRotator : MonoBehaviour
{
    private Transform target;
    private Transform parent;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // rotating the labels (measurements over the lines) towards the player constantly
    void Update()
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
    }
}
