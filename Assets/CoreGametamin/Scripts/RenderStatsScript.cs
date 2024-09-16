using System.Text;
using Unity.Profiling;
using UnityEngine;

namespace Gametamin.Core
{
    public class RenderStatsScript : MonoBehaviour
    {
        static GameObject _instance;
        static bool _showing;
        public static void Toggle()
        {
            if (_showing)
            {
                Hide();
            }
            else
            {
                Show();
            }
            _showing = !_showing;
        }
        static void Show()
        {
            if (_instance == null)
            {
                _instance = GameObjectHelper.CreateGameObject(null, typeof(RenderStatsScript).Name);
                _instance.AddComponentSafe<RenderStatsScript>();
            }
        }
        static void Hide()
        {
            _instance.SetActiveSafe(false);
        }

        string statsText;
        ProfilerRecorder setPassCallsRecorder;
        ProfilerRecorder drawCallsRecorder;
        ProfilerRecorder verticesRecorder;
        Rect _guiRect;
        GUIStyle _guiStyle;
        StringBuilder _stringBuilder;
        private void Awake()
        {
            _guiRect = new Rect(10, 30, 500, 250);
            _stringBuilder = new();
        }
        void OnEnable()
        {
            setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        }
        void OnDisable()
        {
            setPassCallsRecorder.Dispose();
            drawCallsRecorder.Dispose();
            verticesRecorder.Dispose();
        }
        void Update()
        {
            if (setPassCallsRecorder.Valid)
                _stringBuilder.AppendLine($"SetPass Calls: {setPassCallsRecorder.LastValue}");
            if (drawCallsRecorder.Valid)
                _stringBuilder.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
            if (verticesRecorder.Valid)
                _stringBuilder.AppendLine($"Vertices: {verticesRecorder.LastValue}");
            statsText = _stringBuilder.ToString();
            _stringBuilder.Clear();
        }
        void OnGUI()
        {
            if (_guiStyle == null)
            {
                _guiStyle = new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 28
                };
            }
            GUI.TextArea(_guiRect, statsText, _guiStyle);
        }
    }
}
