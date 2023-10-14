using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using System.Text;

public class ProfilerStats : MonoBehaviour
{
    ProfilerRecorder triangleRecorder;

    ProfilerRecorder drawcallsRecorder;

    ProfilerRecorder verticlesRecorder;

    public TMPro.TextMeshProUGUI statOverlay;

    private int framesCount;

    private float framesTime, lastFPS;

    // Start is called before the first frame update
    void Start()
    {
        if(statOverlay == null) {
            statOverlay = GetComponent<TMPro.TextMeshProUGUI>(); 
        }
    }

    private void OnEnable()
    {
        triangleRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        drawcallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        verticlesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
    }

    private void OnDisable()
    {
        triangleRecorder.Dispose();
        drawcallsRecorder.Dispose();
        verticlesRecorder.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        var sb = new StringBuilder(500);

        framesCount++;
        framesTime += Time.unscaledDeltaTime;
        if(framesTime > 0.5) {
            float fps = framesCount / framesTime;
            lastFPS = fps;
            framesCount = 0;
            framesTime = 0;
        }
        sb.AppendLine($"FPS:{lastFPS}");
        sb.AppendLine($"Verts:{verticlesRecorder.LastValue/1000}k");
        sb.AppendLine($"Tris:{triangleRecorder.LastValue/1000}k");
        sb.AppendLine($"Drawcalls:{drawcallsRecorder.LastValue}");

        statOverlay.text = sb.ToString();
    }
}
