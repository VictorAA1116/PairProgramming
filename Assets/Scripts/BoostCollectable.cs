using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoostCollectable : MonoBehaviour
{
    [SerializeField] float boostMultiplier = 2.0f;
    [SerializeField] float turnRateBoostMultiplier = 2.0f;
    [SerializeField] float boostDuration = 5.0f;

    BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();

        if (!collider) return;

        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player)) // optimized
        {
            if (!player.GetHasBoost())
            {
                player.AddBoost(boostMultiplier, boostDuration, turnRateBoostMultiplier);
                Destroy(gameObject);
            }
        }
    }
}