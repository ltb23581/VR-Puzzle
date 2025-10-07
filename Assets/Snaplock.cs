using UnityEngine;
using UnityEngine.InputSystem;

public class Snaplock : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public InputActionProperty triggerValue;
    public string requiredObjectName;

    [HideInInspector] public GameObject currentObject;

    private bool objectInside = false;
    private bool correctObject = false;

    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();

        if (trigger < 0.1f && objectInside && currentObject != null)
        {
            if (currentObject.transform.parent == null ||
                currentObject.transform.parent == transform)
            {
                currentObject.transform.position = position;
                currentObject.transform.eulerAngles = rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece 1") || other.CompareTag("Piece 2") ||
            other.CompareTag("Piece 3") || other.CompareTag("Piece 4") ||
            other.CompareTag("Piece 5") || other.CompareTag("Piece 6") ||
            other.CompareTag("Piece 7") || other.CompareTag("Piece 8") ||
            other.CompareTag("Piece 9"))
        {
            currentObject = other.gameObject;
            objectInside = true;
            correctObject = other.CompareTag(requiredObjectName);

            Debug.Log(other.name + " entered snap zone (tag = " + other.tag + ")");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentObject)
        {
            currentObject = null;
            objectInside = false;
            correctObject = false;
            Debug.Log(other.name + " left snap zone");
        }
    }

    public bool IsCorrect()
    {
        return correctObject;
    }
}
