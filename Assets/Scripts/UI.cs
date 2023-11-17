using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public bool generateOrNot;

    public bool resetOrNot;

    public Slider rotation;
    public TextMeshProUGUI warning;

    [SerializeField] private LSystem treespawner;

    [SerializeField]private TMP_InputField title;
    [SerializeField]private TMP_InputField iteration;
    [SerializeField]private TMP_InputField angle;
    [SerializeField]private TMP_InputField width;
    [SerializeField]private TMP_InputField length;
    [SerializeField]private TMP_InputField variance;

    private int _tempInt;
    private float _tempFloat;
    // Start is called before the first frame update
    public void Start()
    {
        title.text = treespawner.treeTitle.ToString();
        iteration.text = treespawner.iterations.ToString();
        angle.text = treespawner.angle.ToString() + "°";
        length.text = treespawner.length.ToString("F1");
        width.text = treespawner.width.ToString("F1");
        variance.text = treespawner.variance.ToString() + "%";

        rotation.gameObject.SetActive(false);
        warning.gameObject.SetActive(false);
        
    }

    public void TitleUp()
    {
        if (treespawner.treeTitle < LSystem.NumberOfTrees)
        {
            treespawner.treeTitle++;
            treespawner.hasTreeChanged = true;
            title.text = treespawner.treeTitle.ToString();
        }
    }
    public void TitleDown()
    {
        if (treespawner.treeTitle > 1)
        {
            treespawner.treeTitle--;
            treespawner.hasTreeChanged = true;
            title.text = treespawner.treeTitle.ToString();
        }
    }

    public void IterationsUp()
    {
        if (treespawner.iterations < LSystem.MaxIterations)
        {
            treespawner.iterations++;
            iteration.text = treespawner.iterations.ToString();
        }
    }
    public void IterationsDown()
    {
        if (treespawner.iterations > 1)
        {
            treespawner.iterations--;
            iteration.text = treespawner.iterations.ToString();
        }
    }

    public void AngleUp()
    {        
        treespawner.angle++;
        angle.text = treespawner.angle.ToString() + "°";
    }
    public void AngleDown()
    {
        treespawner.angle--;
        angle.text = treespawner.angle.ToString() + "°";
    }

    public void LengthUp()
    {
        treespawner.length += .1f;
        length.text = treespawner.length.ToString("F1");
    }
    public void LengthDown()
    {
        if (treespawner.length > 0)
        {
            treespawner.length -= .1f;
            length.text = treespawner.length.ToString("F1");
        }
    }

    public void WidthUp()
    {
        treespawner.width += .1f;
        width.text = treespawner.width.ToString("F1");
    }
    public void WidthDown()
    {
        if (treespawner.width > 0)
        {
            treespawner.width -= .1f;
            width.text = treespawner.width.ToString("F1");
        }
    }

    public void VarianceUp()
    {
        treespawner.variance++;
        variance.text = treespawner.variance.ToString() + "%";
    }
    public void VarianceDown()
    {
        if (treespawner.variance > 0)
        {
            treespawner.variance--;
            variance.text = treespawner.variance.ToString() + "%";
        }
    }

    public void GenerateNew()
    {
        generateOrNot = true;
    }

    public void ResetValues()
    {
        resetOrNot = true;        
    }

    public void RotateTree()
    {
        treespawner.tree.transform.rotation = Quaternion.Euler(0, rotation.value, 0);
    }

    public void TitleInputOVC()
    {
        treespawner.hasTreeChanged = true;

        if (int.TryParse(title.text, out _tempInt))
        {
            treespawner.treeTitle = Mathf.Clamp(_tempInt, 1, LSystem.NumberOfTrees);
        }
    }
    public void TitleInputOEE()
    {
        title.text = treespawner.treeTitle.ToString();
    }

    public void IterationsInputOVC()
    {
        if (int.TryParse(iteration.text, out _tempInt))
        {
            treespawner.iterations = Mathf.Clamp(_tempInt, 1, LSystem.MaxIterations);
        }
    }
    public void IterationsInputOEE()
    {
        iteration.text = treespawner.iterations.ToString();
    }

    public void AngleInputOVC()
    {
        if (int.TryParse(angle.text, out _tempInt))
        {
            treespawner.angle = _tempInt;
        }
    }
    public void AngleInputOEE()
    {
        angle.text = treespawner.angle.ToString() + "°";
    }

    public void LengthInputOVC()
    {
        if (float.TryParse(length.text, out _tempFloat))
        {
            treespawner.length = _tempFloat;
        }
    }
    public void LengthInputOEE()
    {
        length.text = treespawner.length.ToString("F1");
    }

    public void WidthInputOVC()
    {
        if (float.TryParse(width.text, out _tempFloat))
        {
            treespawner.width = _tempFloat;
        }
    }
    public void WidthInputOEE()
    {
        width.text = treespawner.width.ToString("F1");
    }

    public void VarianceInputOVC()
    {
        if (int.TryParse(variance.text, out _tempInt))
        {
            treespawner.variance = _tempInt;
        }
    }
    public void VarianceInputOEE()
    {
        variance.text = treespawner.variance.ToString() + "%";
    }
}
