using UnityEngine;
using UnityEngine.XR;

public class SimpleGrabAndThrow_XR : MonoBehaviour
{
    public bool rightHand = true;
    public float throwForceMultiplier = 1.5f;

    private Rigidbody hoveredRb;
    private Rigidbody grabbedRb;
    private Transform originalParent;
    private Vector3 lastPos;
    private Vector3 velocity;

    private InputDevice device;

    public Material highlightMaterial;   
    private Material originalMaterial;
    private Renderer hoveredRenderer;

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(
            rightHand ? XRNode.RightHand : XRNode.LeftHand);
    }

    void Update()
    {
        velocity = (transform.position - lastPos) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPos = transform.position;

        if (!device.isValid)
        {
            
            device = InputDevices.GetDeviceAtXRNode(
                rightHand ? XRNode.RightHand : XRNode.LeftHand);
        }

        if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            
            if (triggerValue > 0.8f && grabbedRb == null && hoveredRb != null)
            {
                grabbedRb = hoveredRb;
                originalParent = grabbedRb.transform.parent;
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false;
                grabbedRb.transform.SetParent(transform, true);
                Debug.Log("Grabbed " + grabbedRb.name);
            }

            
            if (triggerValue < 0.1f && grabbedRb != null)
            {
                grabbedRb.transform.SetParent(originalParent, true);
                grabbedRb.isKinematic = false;
                grabbedRb.useGravity = true;
                grabbedRb.linearVelocity = velocity * throwForceMultiplier;
                Debug.Log("Released");
                grabbedRb = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            hoveredRb = other.attachedRigidbody;

            // Try to highlight
            hoveredRenderer = hoveredRb.GetComponent<Renderer>();
            if (hoveredRenderer != null)
            {
                originalMaterial = hoveredRenderer.material;
                hoveredRenderer.material = highlightMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hoveredRb != null && other.attachedRigidbody == hoveredRb)
        {
            // Remove highlight
            if (hoveredRenderer != null)
            {
                hoveredRenderer.material = originalMaterial;
                hoveredRenderer = null;
            }

            hoveredRb = null;
        }
    }
}