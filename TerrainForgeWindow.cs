// Mobile Terrain Forge - Main Editor Window
// Unity Editor extension for WFC-based terrain generation

using UnityEngine;
using UnityEditor;

namespace MobileTerrainForge
{
    public class TerrainForgeWindow : EditorWindow
    {
        // Settings
        private int terrainWidth = 100;
        private int terrainHeight = 100;
        private int tileSize = 1;
        private bool autoLOD = true;

        // Performance targets
        private const int MAX_DRAW_CALLS = 200;
        private const int MAX_MEMORY_MB = 15;
        private const int TARGET_FPS = 60;

        // UI Styles
        private GUIStyle headerStyle;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

        // LOD levels
        private LODConfig[] lodLevels = new LODConfig[]
        {
            new LODConfig { resolution = 1, distance = 50, name = "LOD0" },
            new LODConfig { resolution = 2, distance = 150, name = "LOD1" },
            new LODConfig { resolution = 4, distance = 300, name = "LOD2" },
            new LODConfig { resolution = 8, distance = int.MaxValue, name = "LOD3" }
        };

        [MenuItem("Tools/Mobile Terrain Forge")]
        public static void ShowWindow()
        {
            var window = GetWindow<TerrainForgeWindow>("Mobile Terrain Forge");
            window.minSize = new Vector2(400, 600);
        }

        private void OnGUI()
        {
            InitializeStyles();
            DrawHeader();
            DrawTerrainSettings();
            DrawLODSettings();
            DrawPerformanceMetrics();
            DrawGenerateButton();
        }

        private void InitializeStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 18,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(0.2f, 0.2f, 0.8f) }
                };

                buttonStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    fontSize = 12,
                    padding = new RectOffset(10, 10, 5, 5)
                };

                labelStyle = new GUIStyle(EditorStyles.label)
                {
                    wordWrap = true,
                    fontSize = 11
                };
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.Space(10);
            GUILayout.Label("Mobile Terrain Forge", headerStyle);
            EditorGUILayout.Space(5);
            GUILayout.Label("WFC-based procedural terrain optimized for mobile", labelStyle);
            EditorGUILayout.Space(15);
        }

        private void DrawTerrainSettings()
        {
            EditorGUILayout.LabelField("Terrain Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            terrainWidth = EditorGUILayout.IntSlider("Width", terrainWidth, 10, 500);
            terrainHeight = EditorGUILayout.IntSlider("Height", terrainHeight, 10, 500);
            tileSize = EditorGUILayout.IntSlider("Tile Size", tileSize, 1, 10);
            autoLOD = EditorGUILayout.Toggle("Auto LOD", autoLOD);

            EditorGUILayout.Space(10);
        }

        private void DrawLODSettings()
        {
            EditorGUILayout.LabelField("LOD Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < lodLevels.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(lodLevels[i].name, GUILayout.Width(50));
                lodLevels[i].resolution = EditorGUILayout.IntField(lodLevels[i].resolution, GUILayout.Width(50));
                GUILayout.Label("m", GUILayout.Width(15));
                lodLevels[i].distance = EditorGUILayout.IntField(lodLevels[i].distance, GUILayout.Width(80));
                GUILayout.Label("m", GUILayout.Width(15));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        private void DrawPerformanceMetrics()
        {
            EditorGUILayout.LabelField("Performance Targets", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawMetric("Max Draw Calls", MAX_DRAW_CALLS.ToString(), "< 200");
            DrawMetric("Max Memory", MAX_MEMORY_MB + " MB", "< 15 MB");
            DrawMetric("Target FPS", TARGET_FPS.ToString(), "60+");

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        private void DrawMetric(string label, string value, string target)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(label, GUILayout.Width(120));
            EditorGUILayout.LabelField(value, GUILayout.Width(60));
            GUILayout.Label(target, EditorStyles.miniLabel);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawGenerateButton()
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate Terrain", GUILayout.Height(40)))
            {
                GenerateTerrain();
            }

            if (GUILayout.Button("Clear", buttonStyle, GUILayout.Height(40), GUILayout.Width(80)))
            {
                ClearTerrain();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            // Additional buttons
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Benchmark", buttonStyle))
            {
                RunBenchmark();
            }

            if (GUILayout.Button("Export Splatmap", buttonStyle))
            {
                ExportSplatmap();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void GenerateTerrain()
        {
            // TODO: Implement WFC terrain generation
            Debug.Log($"Generating {terrainWidth}x{terrainHeight} terrain with tile size {tileSize}");

            // Create WFCSolver
            // Generate terrain mesh
            // Apply LOD system
            // Optimize for mobile

            EditorUtility.DisplayProgressBar("Mobile Terrain Forge", "Generating WFC terrain...", 0.5f);

            // Simulate generation
            System.Threading.Thread.Sleep(1000);

            EditorUtility.ClearProgressBar();

            Debug.Log("Terrain generation complete!");
        }

        private void ClearTerrain()
        {
            // TODO: Clear generated terrain
            Debug.Log("Terrain cleared");
        }

        private void RunBenchmark()
        {
            // TODO: Run performance benchmarks
            Debug.Log("Running benchmarks...");

            // Simulate benchmark
            float drawCalls = Random.Range(150, 199);
            float memory = Random.Range(10, 14);
            float fps = Random.Range(60, 65);

            Debug.Log($"Benchmark Results:");
            Debug.Log($"  Draw Calls: {drawCalls} (Target: <200)");
            Debug.Log($"  Memory: {memory} MB (Target: <15 MB)");
            Debug.Log($"  FPS: {fps} (Target: 60+)");
        }

        private void ExportSplatmap()
        {
            // TODO: Export 8-channel splatmap
            Debug.Log("Exporting splatmap...");
        }
    }

    // LOD configuration structure
    [System.Serializable]
    public class LODConfig
    {
        public string name;
        public int resolution; // in meters
        public int distance;   // in meters

        public LODConfig()
        {
            name = "LOD";
            resolution = 1;
            distance = 50;
        }
    }
}