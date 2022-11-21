using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Key : MonoBehaviour
{
    public const string DontHaveKey = "You need a key to open the door.";
    public const string PickKey = "to pickup the key.";
    public const string OpenDoor = "to open the door.";

    [SerializeField] private Player _currentPlayer;
    [SerializeField] private InteractiveObject _keySpot;
    [SerializeField] private InteractiveObject _firstDoubleDoor01;
    [SerializeField] private InteractiveObject _firstDoubleDoor02;
    [SerializeField] private InteractiveObject _secondDoubleDoor01;
    [SerializeField] private InteractiveObject _secondDoubleDoor02;
    
    [SerializeField] private GameObject _firstDoorLock;
    [SerializeField] private GameObject _secondDoorLock;

    [SerializeField] private float _movementAnimSpeed;
    [SerializeField] private float _animSpeed;

    private bool _inInventory;
    private bool _isFirstDoorOpen;
    private bool _isSecondDoorOpen;

    void Start()
    {
        Subscribe();
    }

    private void PickUpKeyHandler(IPlayer player)
    {
        _currentPlayer = player as Player;
        _currentPlayer.SetPickUpAnim();

        gameObject.transform.DOMove(_currentPlayer.playerHand.transform.position, _movementAnimSpeed).SetDelay(0.8f).OnComplete(() =>
        {
            gameObject.transform.SetParent(_currentPlayer.playerHand.transform);
            _currentPlayer.AddInInventory(gameObject);
            _inInventory = true;
        });
        
        UiController.Instance.DisplayTextHandler(0f, PickKey);
    }

    private void OpenDoorHandler(GameObject doorLock)
    {
        transform.Rotate(new Vector3(0,0,90));

        Sequence keySequence = DOTween.Sequence();

        keySequence.Append(transform.DOMove(doorLock.transform.position, _movementAnimSpeed)).OnComplete(() =>
        {
            gameObject.transform.SetParent(doorLock.transform);
        });
        keySequence.Append(transform.DORotate(new Vector3(180f, -90f, 90), 0.3f));
        keySequence.Append(transform.DOMove(_currentPlayer.playerHand.transform.position, _movementAnimSpeed)).SetDelay(1f)
            .OnComplete(() =>
                {
                    gameObject.transform.SetParent(_currentPlayer.playerHand.transform);
                    transform.DORotate(new Vector3(0, -90, 90), 0.1f);
                });

        keySequence.Play();
    }

    private void Subscribe()
    {
        _keySpot.onInteract += PickUpKeyHandler;
        _keySpot.onCanInteract += PickupKeyDisplay;

        _firstDoubleDoor01.onCanInteract += OpenFirstDoorDisplay;
        _secondDoubleDoor01.onCanInteract += OpenSecondDoorDisplay;

        _firstDoubleDoor01.onInteract += (p) => OpenDoorHandler(_firstDoorLock.gameObject);
        _firstDoubleDoor01.onInteract += (p) => OpenFirstDoorAnim(_firstDoubleDoor01.gameObject);
        _firstDoubleDoor02.onInteract += (p) => OpenFirstDoorAnim(_firstDoubleDoor02.gameObject);

        _secondDoubleDoor01.onInteract += (p) => OpenDoorHandler(_secondDoorLock.gameObject);
        _secondDoubleDoor01.onInteract += (p) => OpenSecondDoorAnim(_secondDoubleDoor01.gameObject);
        _secondDoubleDoor02.onInteract += (p) => OpenSecondDoorAnim(_secondDoubleDoor02.gameObject);
    }

    private void PickupKeyDisplay(bool status)
    {
        if (_inInventory) return;

        if (status)
        {
            UiController.Instance.DisplayTextHandler(1f, PickKey);
            return;
        }

        UiController.Instance.DisplayTextHandler(0f, PickKey);
    }

    private void OpenFirstDoorDisplay(bool status)
    {
        if (_isFirstDoorOpen) return;

        if (status && !_inInventory)
        {
            UiController.Instance.DisplayTextHandler(1f, DontHaveKey, false);
            return;
        }

        if (status)
        {
            UiController.Instance.DisplayTextHandler(1f, OpenDoor);
            return;
        }

        UiController.Instance.DisplayTextHandler(0f, " ", false);

    }

    private void OpenSecondDoorDisplay(bool status)
    {
        if (_isSecondDoorOpen) return;

        if (status)
        {
            UiController.Instance.DisplayTextHandler(1f, OpenDoor);
            return;
        }

        UiController.Instance.DisplayTextHandler(0f, OpenDoor);
    }

    private void OpenFirstDoorAnim(GameObject door)
    {
        if(_isFirstDoorOpen || !_inInventory) return;

        door.transform.DORotate(new Vector3(0, -90), _animSpeed).SetDelay(1.5f).OnComplete(() => _isFirstDoorOpen = true);
    }

    private void OpenSecondDoorAnim(GameObject door)
    {
        if(_isSecondDoorOpen || !_inInventory) return;

        door.transform.DORotate(new Vector3(0, -90), _animSpeed).SetDelay(1.5f).OnComplete(() =>
        {
            _isSecondDoorOpen = true;
            StartCoroutine(UiController.Instance.ObjectTip());
        });
    }

    private void UnSubscribe()
    {
        _keySpot.onInteract -= PickUpKeyHandler;
        _keySpot.onCanInteract -= PickupKeyDisplay;

        _firstDoubleDoor01.onCanInteract -= OpenFirstDoorDisplay;
        _secondDoubleDoor01.onCanInteract -= OpenSecondDoorDisplay;

        _firstDoubleDoor01.onInteract -= (p) => OpenDoorHandler(_firstDoorLock.gameObject);
        _firstDoubleDoor01.onInteract -= (p) => OpenFirstDoorAnim(_firstDoubleDoor01.gameObject);
        _firstDoubleDoor02.onInteract -= (p) => OpenFirstDoorAnim(_firstDoubleDoor02.gameObject);

        _secondDoubleDoor01.onInteract -= (p) => OpenDoorHandler(_secondDoorLock.gameObject);
        _secondDoubleDoor01.onInteract -= (p) => OpenSecondDoorAnim(_secondDoubleDoor01.gameObject);
        _secondDoubleDoor02.onInteract -= (p) => OpenSecondDoorAnim(_secondDoubleDoor02.gameObject);
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
