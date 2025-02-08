using System;
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
    [SerializeField] private float _sensitivity = 5.0f;
    [SerializeField] private float _jump = 10f;
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _jumpAction;
    
    float _mouseX, _mouseY;
    private float _x, _y;
    
    private Vector3 _move;
    private Vector2 _input;
    private Vector3 _inertia;
    private float _inertiaCap;
    
    private Transform _cam;
    private PlayerStates _state;
   void Start()
    {   
        _cam = Camera.main.transform;
        _state = PlayerStates.Default;
        //Pegando açao do inputsystem 
        _moveAction = _playerInput.actions["Move"];
        _lookAction = _playerInput.actions["Look"];
        _jumpAction = _playerInput.actions["Jump"];

        _jumpAction.performed += JumpActionOnperformed;
    }
    void Update(){

        ReadInput();
        //
        DetectAerialState();
        if (_state == PlayerStates.Dash){
            
        }
        if (_state == PlayerStates.Ascend || _state == PlayerStates.Descend){
            float vertical = _move.y;
            
            Vector3 horizontal = new Vector3(_x, 0, _y);

            float airSpeed = 8f;
            _inertia += horizontal * airSpeed * Time.deltaTime;
            _move = _inertia;
            if(_inertia.magnitude > airSpeed)
                _inertia = Vector3.ClampMagnitude(_inertia, _inertiaCap);
            _move.y = vertical;

        }
        if (_state == PlayerStates.Default){
            float vertical = _move.y;
            Vector3 horizontal = new Vector3(_x, 0, _y);
            horizontal = this.transform.rotation * horizontal;
            
            horizontal *= _speed;
            _move = horizontal;
            _move.y = vertical;
        }
       
        
        _move += Vector3.up * _gravity * Time.deltaTime;
        _characterController.Move(_move * _speed * Time.deltaTime);
        if (_characterController.isGrounded){
            _move.y = _gravity;
        }
        _cam.position = this.transform.position + Vector3.up * 0.5f;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, _mouseX, 0));
    }
    private void LateUpdate(){
        _cam.rotation = Quaternion.Euler(new Vector3(-_mouseY, _mouseX, 0));
    }
    private void ReadInput(){
        _input= _moveAction.ReadValue<Vector2>();
        _x = _input.x;
        _y = _input.y;
        Vector2 mouse = _lookAction.ReadValue<Vector2>() * _sensitivity;
        _mouseX += mouse.x;
        _mouseY += mouse.y;

        _mouseY = Mathf.Clamp(_mouseY, -75f, 75f);
    }

    public void DetectAerialState(){
        if (_characterController.isGrounded){
            _state = PlayerStates.Default;
            return;
        }
        if (_move.y > 0){
            _state = PlayerStates.Ascend;
            return;
        }
        if (_move.y >= -1) return;
            _state = PlayerStates.Descend;
        
    }
    #region Event
    private void JumpActionOnperformed(InputAction.CallbackContext obj){
        if (_characterController.isGrounded){
            Debug.LogError("Jump");
            float limit = 5f;
            _inertia = new Vector3(_move.x, 0, _move.z);
            
            _inertiaCap = Mathf.Clamp(_inertia.magnitude, limit, 99 );
            
            _move.y = _jump;
        }
    }

    

    #endregion
}
