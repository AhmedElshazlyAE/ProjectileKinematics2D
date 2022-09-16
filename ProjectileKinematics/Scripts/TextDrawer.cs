using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    public Text timetxt;
    public Text vxtxt;
    public Text vytxt;
    ProjectileLauncher launcher;
    void Start()
    {
        launcher = GetComponent<ProjectileLauncher>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        timetxt.text = "Elapsed Time: " + launcher.timeinst.ToString("00:00");
        vxtxt.text = "Horizontal Velocity: " + launcher.initialHorizontalVelocity.ToString("00.00") + " U/s";
        vytxt.text = "Vetical Velocity: " + launcher.initialVerticalVelocity.ToString("00.00") + " U/s";
    }
}
