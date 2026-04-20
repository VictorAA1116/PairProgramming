using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player)) // optimized
        {
            player.AddLap();
        }
    }
}
