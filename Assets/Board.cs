using UnityEngine;

public class Board : MonoBehaviour
{
    public Snaplock[] snapZones;
    private bool hasWon = false;
    public AudioSource victorySound;

    void Update()
    {
        if (hasWon) return;

        bool allFilled = true;
        bool allCorrect = true;

        foreach (var zone in snapZones)
        {
            if (zone == null) continue;

            if (zone.currentObject == null)
            {
                allFilled = false;
                allCorrect = false;
                break;
            }

            if (!zone.IsCorrect())
            {
                allCorrect = false;
            }
        }

        // Only declare win if every zone has something and all are correct
        if (allFilled && allCorrect)
        {
            hasWon = true;
            Debug.Log("🎉 YOU WIN!");
            if (victorySound != null)
                victorySound.Play();
        }
    }
}
