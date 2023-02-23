using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoreAI))]
public class CoreAIEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnSceneGUI()
    {
        CoreAI fov = (CoreAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.FoVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.FoVAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.FoVAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.FoVRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.FoVRadius);


        CoreAI proximity = (CoreAI)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(proximity.transform.position, Vector3.up, Vector3.forward, 360, proximity.proximityRadius);

        Vector3 proxAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.proximityAngle / 2);
        Vector3 proxAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.proximityAngle / 2);

        Handles.color = Color.cyan;
        Handles.DrawLine(fov.transform.position, fov.transform.position + proxAngle01 * proximity.proximityRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + proxAngle02 * proximity.proximityRadius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
        if (proximity.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(proximity.transform.position, proximity.player.transform.position);
        }
    }
}
