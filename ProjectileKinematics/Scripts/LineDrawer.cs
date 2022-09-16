using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ProjectileLauncher))]
[ExecuteInEditMode]
public class LineDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    public int Resolution;
    public ProjectileLauncher launcher;
    public Color LineColor;

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            Vector2 ProjectilePosition = launcher.Projectile.position;
            if (!launcher) launcher = GetComponent<ProjectileLauncher>();
            launcher.CalculateProjectile(ProjectilePosition);
            launcher.currentMaxHeight = launcher.maxHeight + ProjectilePosition.y;
            launcher.initialPosition = ProjectilePosition;
            Vector2 di = ProjectilePosition;
            for (int i = 0; i <= Resolution; i++)
            {
                float simulationtime = i / (float)Resolution * launcher.elapsedTime;

                float dX = 0;

                if (launcher.airResistance == AirResistanceState.Ignore || launcher.airResistance == AirResistanceState.Negate)
                    dX = launcher.initialHorizontalVelocity * simulationtime + ProjectilePosition.x;
                else if (launcher.airResistance == AirResistanceState.Active)
                    dX = launcher.initialHorizontalVelocity * simulationtime + (launcher.airResistanceVelocity * simulationtime * simulationtime) / 2f + ProjectilePosition.x;

                float dY = launcher.initialVerticalVelocity * simulationtime + launcher.gravitationalAcceleration * simulationtime * simulationtime / 2f + ProjectilePosition.y;

                Vector2 df = new Vector2(dX, dY);
                Debug.DrawLine(di, df, LineColor);

                di = df;
            }
        }
        
    }

}
