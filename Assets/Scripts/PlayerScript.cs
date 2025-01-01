using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    InputAction _move;
    InputAction _look;

    private float _x, _y;
    float _mouseX, _mouseY;
   void Start()
    {
        _move = _playerInput.actions["Move"];
        _look = _playerInput.actions["Look"];
    }

    
    void Update()
    {
        _x = _move.ReadValue<Vector2>().x;
        _y = _move.ReadValue<Vector2>().y;
        
        _mouseX = _look.ReadValue<Vector2>().x;
        _mouseY = _look.ReadValue<Vector2>().y;
    }
}
