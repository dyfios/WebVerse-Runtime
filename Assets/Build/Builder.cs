using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FiveSQD.WebVerse.Building
{
    public class Builder
    {
        public static void BuildWebGL()
        {
            BuildPlayerOptions options = new BuildPlayerOptions()
            {
                locationPathName = "../../WebGL",
                options = BuildOptions.None,
                scenes = new string[]
                {
                    "Assets/Runtime/TopLevel/Scenes/Lightweight.unity"
                },
                target = BuildTarget.WebGL
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build Successful. Build written to " + options.locationPathName);
            }
            else if (report.summary.result == BuildResult.Failed)
            {
                Debug.Log("Build Failed.");
            }
            else if (report.summary.result == BuildResult.Cancelled)
            {
                Debug.Log("Build Cancelled.");
            }
            else if (report.summary.result == BuildResult.Unknown)
            {
                Debug.Log("Unknown Build Result.");
            }
            else
            {
                Debug.Log("Unidentified Build Result.");
            }
        }
    }
}