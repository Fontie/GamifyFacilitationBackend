using UnityEngine;

public class SlingshotVisual : MonoBehaviour
{
    public Transform leftPoint;         // One end of the slingshot
    public Transform rightPoint;        // Other end
    public Transform projectile;        // The projectile
    public BoxCollider detectionZone;   // The zone to detect projectile placement

    public LineRenderer idleLine;      // Line between left and right when no projectile
    private LineRenderer activeLine;    // Two lines to projectile

    public SlingshotProjectile slingshotProjectile; //To say to projectile if its connected to the line and when to let go.
    public Material lineMaterial; 

    void Start()
    {
        // Create line renderers dynamically
        idleLine = CreateLineRenderer("IdleLine", Color.black);
        activeLine = CreateLineRenderer("ActiveLine", Color.black);
        activeLine.positionCount = 4; // Two segments: point1 -> projectile, projectile -> point2
    }

    void Update()
    {
        if (!idleLine.enabled)
        {
            // Draw two lines: left to projectile, right to projectile
            activeLine.positionCount = 4;
            activeLine.SetPosition(0, leftPoint.position);
            activeLine.SetPosition(1, projectile.position);
            activeLine.SetPosition(2, projectile.position);
            activeLine.SetPosition(3, rightPoint.position);
        }
        else
        {
            idleLine.positionCount = 2;
            idleLine.SetPosition(0, leftPoint.position);
            idleLine.SetPosition(1, rightPoint.position);

            if (IsProjectileInZone())
            {
                slingshotProjectile.SetActive();
                idleLine.enabled = false;
                activeLine.enabled = true;
            }
        }
    }


    private bool IsProjectileInZone()
    {
        return detectionZone.bounds.Contains(projectile.position);
    }

    public void ResetSling()
    {
        activeLine.enabled = false;
        idleLine.enabled = true;    
    }

    private LineRenderer CreateLineRenderer(string name, Color color)
    {
        GameObject lrObj = new GameObject(name);
        lrObj.transform.parent = this.transform;
        LineRenderer lr = lrObj.AddComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Sprites/Default")); //Always on top, use for gui maybe later
        lr.material = lineMaterial;
        Shader.Find("Unlit/Color"); // Also respects depth
        lr.widthMultiplier = 0.05f;
        lr.positionCount = 2;
        lr.startColor = color;
        lr.endColor = color;
        lr.sortingOrder = 10;
        return lr;
    }
}
