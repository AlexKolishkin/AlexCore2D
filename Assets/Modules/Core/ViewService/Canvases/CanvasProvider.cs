using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.View
{
    public class CanvasProvider
    {
        private Transform _windowCanvas;
        
        public Transform WindowCanvas
        {
            get
            {
                if (!_windowCanvas) _windowCanvas = Object.FindObjectOfType<WindowCanvas>()?.Content;

                return _windowCanvas;
            }
        }
        
        public Transform GetCanvasTransform(CanvasType canvasType)
        {
            switch (canvasType)
            {
                case CanvasType.Window: return WindowCanvas;
                default:
                    throw new ArgumentOutOfRangeException(nameof(canvasType), canvasType, null);
            }
        }
    }
    
    public enum CanvasType
    {
        Window,
    }
}
