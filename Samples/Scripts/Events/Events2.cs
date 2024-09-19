using TMPro;
using UnityEngine;

// Update UI using Events
public class Events2 : MonoBehaviour
{
    private int _score;
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    // So subscribe/unsubscribe from UnityEvent use AddListener/RemoveListener
    // UnityEvents unsubscribe automatically OnDestroy()
    private void Start()
    {
        Events2_EventManager.OnEnemyKilled.AddListener(IncrementScore);
        
        // You can use lambda for anonymous methods invocation
        // It will also automatically unsubscribe from lambda OnDestroy()
        // Events2_EventManager.OnEnemyKilled.AddListener(() =>
        // {
        //     _score++;
        //     _scoreText.text = $"Score: {_score:00}";
        // });
    }

    void IncrementScore()
    {
        _score++;
        _scoreText.text = $"Score: {_score:00}";
    }
}