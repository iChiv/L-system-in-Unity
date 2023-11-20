using UnityEngine;

public class FractalGenerator : MonoBehaviour
{
    public float childScale = 0.5f;
    public Material material;

    private PrimitiveType selectedShape = PrimitiveType.Cube;
    private LineRenderer lineRendererPrefab;
    private LineRenderer currentLineRenderer;

    public void SetSelectedShape(PrimitiveType shape)
    {
        selectedShape = shape;
    }

    void Start()
    {
        // 其他初始化代码

        lineRendererPrefab = Instantiate(new GameObject("LineRenderer")).AddComponent<LineRenderer>();
        lineRendererPrefab.material = material; // 使用相同的材质

        lineRendererPrefab = Instantiate(new GameObject("LineRenderer")).AddComponent<LineRenderer>();
        lineRendererPrefab.material = material; // 使用相同的材质
    }

    public void GenerateFractal(int maxDepth)
    {
        ClearFractal();
        GenerateFractal(Vector3.zero, Vector3.up, Quaternion.identity, maxDepth, 1f, Vector3.zero);
    }

    public void Generate2DShape(int maxDepth, float parentScale, Vector3 parentPosition)
    {
        ClearFractal();
        Generate2DShape(Vector3.zero, Vector3.up, Quaternion.identity, maxDepth, parentScale, parentPosition);
    }

    private void Generate2DShape(Vector3 position, Vector3 direction, Quaternion rotation, int depth, float parentScale, Vector3 parentPosition)
    {
        if (depth == 0)
        {
            return;
        }

        switch (selectedShape)
        {
            case PrimitiveType.Quad:
                GenerateQuad(position, direction, rotation, depth, parentScale, parentPosition);
                break;
            case PrimitiveType.Cylinder:
                GenerateCylinder(position, direction, rotation, depth, parentScale, parentPosition);
                break;
            case PrimitiveType.Sphere:
                GenerateSphere(position, direction, rotation, depth, parentScale, parentPosition);
                break;
            default:
                Debug.LogError("Unsupported 2D shape selected");
                return;
        }
    }

    private void GenerateQuad(Vector3 position, Vector3 direction, Quaternion rotation, int depth, float parentScale, Vector3 parentPosition)
    {
        if (currentLineRenderer == null)
        {
            currentLineRenderer = Instantiate(lineRendererPrefab, position, rotation);
        }

        float scale = parentScale * childScale;
        Vector3 spawnPosition = parentPosition + direction * (parentScale + scale) * 0.5f;

        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, spawnPosition);

        Vector3[] newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.right };

        foreach (var dir in newDirections)
        {
            Generate2DShape(spawnPosition, dir, Quaternion.identity, depth - 1, scale, spawnPosition);
        }
    }

    private void GenerateCylinder(Vector3 position, Vector3 direction, Quaternion rotation, int depth, float parentScale, Vector3 parentPosition)
{
    if (currentLineRenderer == null)
    {
        currentLineRenderer = Instantiate(lineRendererPrefab, position, rotation);
    }

    float scale = parentScale * childScale;
    Vector3 spawnPosition = parentPosition + direction * (parentScale + scale) * 0.5f;

    currentLineRenderer.positionCount++;
    currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, spawnPosition);

    // For a cylinder, let's consider two points at the top and bottom
    Vector3 topPoint = spawnPosition + Vector3.up * scale;
    Vector3 bottomPoint = spawnPosition - Vector3.up * scale;

    currentLineRenderer.positionCount++;
    currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, topPoint);
    currentLineRenderer.positionCount++;
    currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, bottomPoint);

    Vector3[] newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.right };

    foreach (var dir in newDirections)
    {
        Generate2DShape(spawnPosition, dir, Quaternion.identity, depth - 1, scale, spawnPosition);
    }
}

private void GenerateSphere(Vector3 position, Vector3 direction, Quaternion rotation, int depth, float parentScale, Vector3 parentPosition)
{
    if (currentLineRenderer == null)
    {
        currentLineRenderer = Instantiate(lineRendererPrefab, position, rotation);
    }

    float scale = parentScale * childScale;
    Vector3 spawnPosition = parentPosition + direction * (parentScale + scale) * 0.5f;

    currentLineRenderer.positionCount++;
    currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, spawnPosition);

    // For a sphere, let's consider points in a circular pattern around the center
    int segments = 20; // Adjust the number of segments as needed

    for (int i = 0; i < segments; i++)
    {
        float angle = i * 2 * Mathf.PI / segments;
        Vector3 point = spawnPosition + new Vector3(Mathf.Cos(angle) * scale, Mathf.Sin(angle) * scale, 0f);
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, point);
    }

    Vector3[] newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.right };

    foreach (var dir in newDirections)
    {
        Generate2DShape(spawnPosition, dir, Quaternion.identity, depth - 1, scale, spawnPosition);
    }
}


    private void GenerateFractal(Vector3 position, Vector3 direction, Quaternion rotation, int depth, float parentScale, Vector3 parentPosition)
    {
        if (depth == 0)
        {
            return;
        }

        GameObject child = GameObject.CreatePrimitive(selectedShape);
        child.GetComponent<Renderer>().material = material;

        float scale = parentScale * childScale;
        Vector3 spawnPosition = parentPosition + direction * (parentScale + scale) * 0.5f;

        child.transform.localScale = new Vector3(scale, scale, scale);
        child.transform.position = spawnPosition;
        child.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        child.tag = "FractalObject";

        Vector3[] newDirections;
        switch (selectedShape)
        {
            case PrimitiveType.Cube:
                newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.down, rotation * Vector3.right, rotation * Vector3.left, rotation * Vector3.forward, rotation * Vector3.back };
                break;
            case PrimitiveType.Sphere:
                newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.down, rotation * Vector3.right, rotation * Vector3.left, rotation * Vector3.forward, rotation * Vector3.back };
                break;
            case PrimitiveType.Cylinder:
                newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.down, rotation * Vector3.right, rotation * Vector3.left, rotation * Vector3.forward, rotation * Vector3.back };
                break;
            default:
                newDirections = new Vector3[] { rotation * Vector3.up, rotation * Vector3.right, rotation * Vector3.forward };
                break;
        }

        foreach (var dir in newDirections)
        {
            GenerateFractal(spawnPosition, dir, rotation, depth - 1, scale, spawnPosition);
        }
    }

    private void ClearFractal()
    {
        GameObject[] fractalObjects = GameObject.FindGameObjectsWithTag("FractalObject");

        foreach (var obj in fractalObjects)
        {
            Destroy(obj);
        }

        // 重置 LineRenderer
        if (currentLineRenderer != null)
        {
            Destroy(currentLineRenderer.gameObject);
            currentLineRenderer = null;
        }
    }
}
