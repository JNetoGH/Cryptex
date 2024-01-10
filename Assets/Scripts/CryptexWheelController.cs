using System;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CryptexWheelController : MonoBehaviour
{
    
    [SerializeField] private char _initialLetter;
    private char _currentLetter;

    private CryptexController _cryptex;
    private float _TargetZRotation = 0f;
    private Quaternion _initialRotation;
    
    public char CurrentLetter => _currentLetter;

    private void Awake()
    {
        _initialRotation = transform.rotation;
        _currentLetter = _initialLetter;
    }
    
    void Start()
    {
        _cryptex = GetComponentInParent<CryptexController>();
    }
    
    void Update()
    {
        if (_cryptex == null) 
            return;
        
        if (_cryptex.CurrWheel != this)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _TargetZRotation += _cryptex.RotAmount;
            if (_cryptex.AllLetters.IndexOf(_currentLetter) == _cryptex.AllLetters.Count - 1)
                _currentLetter = _cryptex.AllLetters[0];
            else
                _currentLetter = _cryptex.AllLetters[_cryptex.AllLetters.IndexOf(_currentLetter) + 1];
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _TargetZRotation -= _cryptex.RotAmount;
            if (_cryptex.AllLetters.IndexOf(_currentLetter) == 0)
                _currentLetter = _cryptex.AllLetters[_cryptex.AllLetters.Count - 1];
            else
                _currentLetter = _cryptex.AllLetters[_cryptex.AllLetters.IndexOf(_currentLetter) - 1];
        }
        
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Quaternion targetRotation = _initialRotation * Quaternion.Euler(0f, 0f, _TargetZRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _cryptex.RotSpeed * Time.deltaTime);
        _cryptex.targetZRotation = _TargetZRotation;
    }
    
}