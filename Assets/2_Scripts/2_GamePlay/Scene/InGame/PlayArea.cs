using System;
using LSH.Utils;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField] private TriggerRelay2D _ceilingTrigger;
    [SerializeField] private TriggerRelay2D _floorTrigger;
    [SerializeField] private float _despawnX = -10f;

    public float CeilingY => _ceilingTrigger.transform.position.y;
    public float FloorY => _floorTrigger.transform.position.y;
    public float TotalY => CeilingY - FloorY;
    public float DespawnX => _despawnX;

    public event Action OnOutOfBounds;

    public void Init()
    {
        _ceilingTrigger.OnTriggerEntered += HandleOutOfBounds;
        _floorTrigger.OnTriggerEntered += HandleOutOfBounds;
    }

    private void HandleOutOfBounds(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        OnOutOfBounds?.Invoke();
    }

    private void OnDestroy()
    {
        _ceilingTrigger.OnTriggerEntered -= HandleOutOfBounds;
        _floorTrigger.OnTriggerEntered -= HandleOutOfBounds;
    }
}
