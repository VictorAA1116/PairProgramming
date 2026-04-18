using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float playerTimer = 0.0f;
    private bool isGameOver = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            GameOver();
            return;
        }
        
        playerTimer += Time.deltaTime;
    }

    private void GameOver()
    {
        
    }
}
