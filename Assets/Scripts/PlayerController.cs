using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 50f;
    private bool hasBoost = false;
    private int currentLap = 0;
    public static UnityEvent OnPlayerCompletedLap = new UnityEvent();
    
    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction boostAction;

    private void Awake()
    {
        if (!inputActions) return;

        moveAction = inputActions.FindAction("Player/Move");
        moveAction?.Enable();
        boostAction = inputActions.FindAction("Player/Jump");
        boostAction?.Enable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveAction != null && moveAction.ReadValue<Vector2>().magnitude > 0.1f)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            
            transform.position += transform.forward * moveInput.y * Time.deltaTime * moveSpeed;
            
            transform.Rotate(0, moveInput.x * Time.deltaTime * turnSpeed, 0);
        }
        
        if (boostAction != null && boostAction.triggered && hasBoost)
        {
            StartCoroutine(UseBoost());
        }
    }

    private IEnumerator UseBoost()
    {
        moveSpeed *= 1.5f;
        hasBoost = false;
        
        yield return new WaitForSeconds(5f);
        
        moveSpeed /= 1.5f;
    }

    public void AddBoost()
    {
        hasBoost = true;
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
}
