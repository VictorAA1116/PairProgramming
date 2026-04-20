using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance; // optimized

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 50f;

    private bool hasBoostPickup = false; // optimized
    private bool isBoostActive = false; // optimized

    private float boostMultiplier = 2.0f;
    private float boostDuration = 1.0f;
    private float turnRateBoostMultiplier = 2.0f;

    private int currentLap = 0;
    public static UnityEvent OnPlayerCompletedLap = new UnityEvent();

    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction boostAction;
    private Rigidbody rb;

    private void Awake()
    {
        Instance = this; // optimized

        rb = GetComponent<Rigidbody>();

        if (!inputActions) return;

        moveAction = inputActions.FindAction("Player/Move");
        moveAction?.Enable();
        boostAction = inputActions.FindAction("Player/Jump");
        boostAction?.Enable();
    }

    void Update()
    {
        if (moveAction != null && moveAction.ReadValue<Vector2>().magnitude > 0.1f)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            transform.position += transform.forward * moveInput.y * Time.deltaTime * moveSpeed;
            transform.Rotate(0, moveInput.x * Time.deltaTime * turnSpeed, 0);
        }

        if (boostAction != null && boostAction.triggered && hasBoostPickup && !isBoostActive) // optimized
        {
            StartCoroutine(UseBoost());
        }
    }

    private IEnumerator UseBoost()
    {
        isBoostActive = true; // optimized

        hasBoostPickup = false; // optimized

        moveSpeed *= boostMultiplier;
        turnSpeed *= turnRateBoostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        moveSpeed /= boostMultiplier;
        turnSpeed /= turnRateBoostMultiplier;

        isBoostActive = false; // optimized
    }

    public void AddBoost(float boostMultiplier, float boostDuration, float turnRateBoostMultiplier)
    {
        this.boostMultiplier = boostMultiplier;
        this.boostDuration = boostDuration;
        this.turnRateBoostMultiplier = turnRateBoostMultiplier;

        hasBoostPickup = true; // optimized
    }

    public void AddLap()
    {
        currentLap++;
        OnPlayerCompletedLap.Invoke();
    }

    public int GetCurrentLap()
    {
        return currentLap;
    }

    public bool GetHasBoost()
    {
        return hasBoostPickup; // optimized
    }
}