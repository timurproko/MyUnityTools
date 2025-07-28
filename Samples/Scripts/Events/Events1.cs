using TMPro;
using UnityEngine;

// Update UI using Events
public class Events1 : MonoBehaviour
{
    private int _score;
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Events1_EventManager.OnEnemyKilled += IncrementScore;
    }
    
    // You have to always unsubscribe from events to avoid errors
    private void OnDestroy()
    {
        Events1_EventManager.OnEnemyKilled -= IncrementScore;
    }

    void IncrementScore()
    {
        _score++;
        _scoreText.text = $"Score: {_score:00}";
    }
}