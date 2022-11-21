using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    public GameObject playerHand => _playerHand; 

    [SerializeField] private List<GameObject> _inventoryList;
    [SerializeField] private GameObject _playerHand;

    [SerializeField] private CharacterController _rigB;
    [SerializeField] private Animator _anim;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity = -9.81f;

    private int animTrigger = 0;

    private readonly string _moveT = "Move";
    private readonly string _pickupT = "Pickup";

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 velocity = dir.normalized * _moveSpeed;
        velocity.y = _gravity;
        velocity = transform.transform.TransformDirection(velocity);

        _rigB.Move(velocity * Time.deltaTime);
        animTrigger = dir == Vector3.zero ? 0 : 1;
        _anim.SetInteger(_moveT, animTrigger);
    }

    public void SetPickUpAnim()
    {
        _anim.SetTrigger(_pickupT);
    }

    public void AddInInventory(GameObject ob)
    {
        _inventoryList.Add(ob);
    }

    public void RemoveFromInventory(GameObject ob)
    {
        if (_inventoryList.Find((found) => found == ob))
        {
            _inventoryList.Remove(ob);
        }
    }
}
