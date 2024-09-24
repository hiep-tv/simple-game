using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        void RegisterInputEvent()
        {
            UserInput.OnPointerDown += OnPointerDown;
            UserInput.OnPointerDrag += OnPointerDrag;
            UserInput.OnPointerUp += OnPointerUp;
        }
        void OnPointerDown(Vector2 position)
        {
            position = GetWorldPoint(position);
            SelectBoardItem(position);
        }
        void OnPointerDrag(Vector2 position)
        {
            position = GetWorldPoint(position);
            DragBoardItem(position);
        }
        void OnPointerUp(Vector2 position)
        {
            position = GetWorldPoint(position);
            ReleaseBoardItem(position);
        }
        Vector2 GetWorldPoint(Vector2 position)
        {
            return _mainCamera.ScreenToWorldPoint(position);
        }
        void LogDebug(string debug)
        {
            Debug.Log(debug);
        }
    }
}
