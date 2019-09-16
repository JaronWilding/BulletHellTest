using System;
using UnityEngine;

namespace Jaron.BulletHell.InputManagement.Interfaces
{
    public interface IINputManager
    {
        void AddActionToBinding(string binding, Action action);
        float GetAxis(string axisName);
        bool GetButton(string buttonName);
        Vector2 GetMouseVector();
    }
}
