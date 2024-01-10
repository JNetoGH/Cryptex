using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CryptexController : MonoBehaviour
{
    
    [Header("Rotation (targetZRot is read-only)")]
    [SerializeField] public float targetZRotation;
    [SerializeField] private float _rotAmount = 15f;
    [SerializeField] private float _rotSpeed = 20f;
    
    [Header("Outline")]
    [SerializeField, Range(0, 10)] private float _outlineWidth = 3f;
    [SerializeField] private Color _outlineColor = Color.green;
    
    [Header("Password (Current Combination is read-only)")]
    [SerializeField] private bool _isPasswordSolved = false;
    [SerializeField] private string _password = "bbbb";
    [SerializeField] private string _currentCombination = "";
    [SerializeField] private List<char> _allLetters;
    
    private CryptexWheelController _currWheel;
    private List<CryptexWheelController> _wheels;
    
    private Animator _animator;
    private static readonly int Open = Animator.StringToHash("Open");

    public float RotAmount => _rotAmount;
    public float RotSpeed => _rotSpeed;
    public CryptexWheelController CurrWheel => _currWheel;
    public List<char> AllLetters => _allLetters;
    public string Password => _password;
    
    void Start()
    {
        _wheels = GetComponentsInChildren<CryptexWheelController>().ToList();
        _wheels.ForEach(w =>
        {
            _currentCombination += w.CurrentLetter;
            Outline outline = w.GetComponent<Outline>();
            outline.OutlineWidth = _outlineWidth;
            outline.OutlineColor = _outlineColor;
            outline.enabled = false;
        });
        _currWheel = _wheels[0];
        _currWheel.GetComponent<Outline>().enabled = true;
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        UpdateCurrentWheel();
        UpdateCurrentCombination();

        if (!_isPasswordSolved && _password.Equals(_currentCombination))
            _isPasswordSolved = true;
        
        if (_isPasswordSolved)
        {
            _animator.SetTrigger(Open);
            _isPasswordSolved = false;
            _wheels.ForEach(w => w.GetComponent<Outline>().enabled = false);
        }
    }

    private void UpdateCurrentCombination()
    {
        _currentCombination = "";
        foreach (CryptexWheelController wheel in _wheels)
            _currentCombination += wheel.CurrentLetter;
    }

    private void UpdateCurrentWheel()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_wheels.IndexOf(_currWheel) == _wheels.Count - 1)
                _currWheel = _wheels[0];
            else
                _currWheel = _wheels[_wheels.IndexOf(_currWheel) + 1];
            SyncOutline();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_wheels.IndexOf(_currWheel) == 0)
                _currWheel = _wheels[_wheels.Count - 1];
            else
                _currWheel = _wheels[_wheels.IndexOf(_currWheel) - 1];
            SyncOutline();
        }
        
    }

    private void SyncOutline()
    {
        _wheels.ForEach(w => w.GetComponent<Outline>().enabled = false);
        _currWheel.GetComponent<Outline>().enabled = true;
    }
}
