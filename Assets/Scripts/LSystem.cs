using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LSystem : MonoBehaviour
{
    public static int NumberOfTrees = 8;
    public static int MaxIterations = 7;

    public int treeTitle = 1;
    public int iterations = 5;
    public float length = 5f;
    public float angle = 30f;
    public float width = 2f;
    public float variance = 10f;
    public GameObject tree = null;
    public bool hasTreeChanged = false;
    
    [SerializeField] private GameObject treeParent;
    [SerializeField] private GameObject branch;
    [SerializeField] private GameObject leaf;
    [SerializeField] private UI ui;
    
    private const string axiom = "X";
    private Stack<TransformInfo> transformStack;
    private int _titleLastFrame;
    private int _iterationsLastFrame;
    private float _angleLastFrame;
    private float _widthLastFrame;
    private float _lengthLastFrame;
    private Dictionary<char, string> _rules;
    private string _currentString = string.Empty;
    private Vector3 _initialPosition = Vector3.zero;
    private float[] _randomRotationValues = new float[100];
    
    private void Start()
    {
        _titleLastFrame = treeTitle;
        _iterationsLastFrame = iterations;
        _angleLastFrame = angle;
        _widthLastFrame = width;
        _lengthLastFrame = length;

        for (int i = 0; i < _randomRotationValues.Length; i++)
        {
            _randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        
        transformStack = new Stack<TransformInfo>();

        _rules = new Dictionary<char, string>()
        {
            { 'X', "[FX][-FX][+FX]" },
            { 'F', "FF" }
        };
        
        Generate();
    }

    private void Update()
    {
        if (ui.generateOrNot || Input.GetKeyDown(KeyCode.G))
        {
            ResetRandomValues();
            ui.generateOrNot = false;
            Generate();
        }

        if (ui.resetOrNot || Input.GetKeyDown(KeyCode.R))
        {
            ResetValues();
            ui.resetOrNot = false;
            ui.Start();
            Generate();
        }

        if (_titleLastFrame != treeTitle)
        {
            if (treeTitle >= 6)
            {
                ui.rotation.gameObject.SetActive(true);
            }
            else
            {
                ui.rotation.value = 0;
                ui.rotation.gameObject.SetActive(false);
            }

            switch (treeTitle)
            {
                case 1:
                    SelectTreeOne();
                    break;
                case 2:
                    SelectTreeTwo();
                    break;
                case 3:
                    SelectTreeThree();
                    break;
                case 4:
                    SelectTreeFour();
                    break;
                case 5:
                    SelectTreeFive();
                    break;
                case 6:
                    SelectTreeSix();
                    break;
                case 7:
                    SelectTreeSeven();
                    break;
                case 8:
                    SelectTreeEight();
                    break;
                default:
                    SelectTreeOne();
                    break;
            }

            _titleLastFrame = treeTitle;
        }
        
        if (_iterationsLastFrame != iterations)
        {
            if (iterations >= 6)
            {
                ui.warning.gameObject.SetActive(true);
                StopCoroutine("TextFade");
                StartCoroutine("TextFade");
            }
            else
            {
                ui.warning.gameObject.SetActive(false);
            }
        }
        
        if (_iterationsLastFrame != iterations ||
            _angleLastFrame  != angle ||
            _widthLastFrame  != width ||
            _lengthLastFrame != length)
        {
            ResetFlags();
            Generate();
        }
    }

    private void Generate()
    {
        Destroy(tree);

        tree = Instantiate(treeParent);
        
        _currentString = axiom;

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (var c in _currentString)
            {
                stringBuilder.Append(_rules.ContainsKey(c) ? _rules[c] : c.ToString());
            }

            _currentString = stringBuilder.ToString();

            stringBuilder = new StringBuilder();
        }

        for (int i = 0; i < _currentString.Length; i++)
        {
            switch (_currentString[i])
            {
                case 'F':
                    _initialPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * length);
                    
                    GameObject fLine = _currentString[(i + 1) % _currentString.Length] == 'X' || _currentString[(i + 3) % _currentString.Length] == 'F' && _currentString[(i + 4) % _currentString.Length] == 'X' ? Instantiate(leaf) : Instantiate(branch);
                    fLine.transform.SetParent(tree.transform);
                    fLine.GetComponent<LineRenderer>().SetPosition(0, _initialPosition);
                    fLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    fLine.GetComponent<LineRenderer>().startWidth = width;
                    fLine.GetComponent<LineRenderer>().endWidth = width;
                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * _randomRotationValues[i % _randomRotationValues.Length]));
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * _randomRotationValues[i % _randomRotationValues.Length]));
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120 * (1 + variance / 100 * _randomRotationValues[i % _randomRotationValues.Length]));
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120 * (1 + variance / 100 * _randomRotationValues[i % _randomRotationValues.Length]));
                    break;
                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
                default:
                    throw new InvalidOperationException("invalid L-system operation");
            }
        }
        
        tree.transform.rotation = Quaternion.Euler(0,ui.rotation.value,0);
    }

    private void SelectTreeOne()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[F-[X+X]+F[+FX]-X]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeTwo()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[-FX][+FX][FX]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeThree()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[-FX]X[+FX][+F-FX]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeFour()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[FF[+XF-F+FX]--F+F-FX]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeFive()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[FX[+F[-FX]FX][-F-FXFX]]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeSix()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[F[+FX][*+FX][/+FX]]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeSeven()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[*+FX]X[+FX][/+F-FX]" },
            { 'F', "FF" }
        };
        
        Generate();
    }
    private void SelectTreeEight()
    {
        _rules = new Dictionary<char, string>()
        {
            { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" },
            { 'F', "FF" }
        };
        
        Generate();
    }

    private void ResetRandomValues()
    {
        for (int i = 0; i < _randomRotationValues.Length; i++)
        {
            _randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }

    private void ResetFlags()
    {
        _iterationsLastFrame = iterations;
        _angleLastFrame = angle;
        _widthLastFrame = width;
        _lengthLastFrame = length;
    }

    private void ResetValues()
    {
        iterations = 4;
        angle = 30f;
        width = 2f;
        length = 5f;
        variance = 10f;
    }
    
    IEnumerator TextFade()
    {
        Color c = ui.warning.color;

        float TOTAL_TIME = 4f;
        float FADE_DURATION = .25f;

        for (float timer = 0f; timer <= TOTAL_TIME; timer += Time.deltaTime)
        {
            if (timer > TOTAL_TIME - FADE_DURATION)
            {
                c.a = (TOTAL_TIME - timer) / FADE_DURATION;
            }
            else if (timer > FADE_DURATION)
            {
                c.a = 1f;
            }
            else
            {
                c.a = timer / FADE_DURATION;
            }

            ui.warning.color = c;

            yield return null;
        }
    }
}

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}
