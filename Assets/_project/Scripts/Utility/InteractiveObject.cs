using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public enum InteractionType
{
    Trigger, Collision
}
public class InteractiveObject : MonoBehaviour
{
    public Action<IPlayer> onInteract;
    public Action<bool> onCanInteract;

    [field:SerializeField] [CanBeNull] public IPlayer PlayerRef { get; private set; }

    [SerializeField] private InteractionType _interactionType;

    [SerializeField] private DetectionArea _interactArea;

    [SerializeField] private bool _canInteract;

    private void Start()
    {
        Initializations();
    }

    private void Update()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.F))
        {
            onInteract?.Invoke(PlayerRef);
        }
    }

    private void Initializations()
    {
        switch (_interactionType)
        {
            case InteractionType.Trigger:
                _interactArea.onDetectionTriggerStart += (status, player) =>
                {
                    _canInteract = status;
                    PlayerRef = player;
                    onCanInteract?.Invoke(status);
                };
                break;
            case InteractionType.Collision:
                _interactArea.onDetectionCollisionStart += (status, player) =>
                {
                    _canInteract = status;
                    PlayerRef = player;
                    onCanInteract?.Invoke(status);
                };
                break;
        }
    }
}
