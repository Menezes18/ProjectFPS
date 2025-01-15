using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput; // input system
    [SerializeField] CharacterController _characterController;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _gravity = -9.81f;
    InputAction _moveAction;
    InputAction _lookAction;
    
    float _mouseX, _mouseY;

    private Vector3 _move;
    private Transform _cam;
    private Vector2 _input;
   void Start()
    {   
        _cam = Camera.main.transform;
        //Pegando açao do inputsystem 
        _moveAction = _playerInput.actions["Move"];
        _lookAction = _playerInput.actions["Look"];
    }

    
    void Update(){

        _input= _moveAction.ReadValue<Vector2>();
        
        _mouseX += _lookAction.ReadValue<Vector2>().x;
        _mouseY += _lookAction.ReadValue<Vector2>().y;

        _mouseY = Mathf.Clamp(_mouseY, -75f, 75f);  
        
        _move = new Vector3(_input.x, 0, _input.y);
        _move = transform.rotation * _move;
        _move = _move * _speed;
        _move.y = _gravity;
        _characterController.Move(_move * _speed * Time.deltaTime);
        _cam.position = this.transform.position + Vector3.up * 0.5f;
        _cam.rotation = Quaternion.Euler(new Vector3(-_mouseY, _mouseX, 0));
        
        transform.rotation = Quaternion.Euler(new Vector3(0, _mouseX, 0));
    }
}
