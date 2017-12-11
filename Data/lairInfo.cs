using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class lairInfo
{
    public string warmbug_guid;
    public string bug_name { get; set; }
    public double location_x { get; set; }

    public double easyMagnification = 0.5f;
    public double normalMagnification = Math.Round(UnityEngine.Random.Range(0.8f, 1.2f), 2);
    public double hardMagnification = Math.Round(UnityEngine.Random.Range(1.3f, 1.7f), 2);
    public double hellMagnification = Math.Round(UnityEngine.Random.Range(2.5f, 2.9f), 2);
    public lairInfo()
    {
        warmbug_guid = Guid.NewGuid().ToString();
    }
}
