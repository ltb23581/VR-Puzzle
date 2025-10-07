using UnityEngine;

public class Combine : MonoBehaviour
{
    public Transform snapTarget; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            other.transform.position = snapTarget.position;
            other.transform.rotation = snapTarget.rotation;
            other.GetComponent<Rigidbody>().isKinematic = true; // lock in puzzle pieces in place
        }
    }
}
