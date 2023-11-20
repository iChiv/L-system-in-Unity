using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShapeSelector : MonoBehaviour
{
    public FractalGenerator fractalGenerator;
    public TMP_Dropdown shapeDropdown;
    public Slider depthSlider;
    public Toggle is3DToggle; // 新增的Toggle用于切换2D和3D形状

    private void Start()
    {
        OnShapeSelected();
        OnDepthValueChanged();
        OnToggleValueChanged();
    }

    public void OnShapeSelected()
    {
        int selectedShapeIndex = shapeDropdown.value;

        switch (selectedShapeIndex)
        {
            case 0:
                fractalGenerator.SetSelectedShape(PrimitiveType.Cube);
                break;
            case 1:
                fractalGenerator.SetSelectedShape(PrimitiveType.Sphere);
                break;
            case 2:
                fractalGenerator.SetSelectedShape(PrimitiveType.Cylinder);
                break;
            default:
                break;
        }

        GenerateFractal();
    }

    public void OnDepthValueChanged()
    {
        GenerateFractal();
    }

    public void OnToggleValueChanged()
    {
        GenerateFractal();
    }

    private void GenerateFractal()
    {
        int maxDepth = (int)depthSlider.value;
        float parentScale = 1f;
        Vector3 parentPosition = Vector3.zero;

        if (is3DToggle.isOn)
        {
            fractalGenerator.GenerateFractal(maxDepth);
        }
        else
        {
            fractalGenerator.Generate2DShape(maxDepth, parentScale, parentPosition);
        }
    }
}