using System;
using UnityEngine;

internal interface ITargetControlsProvider
{
    event Action<Vector3> TargetShift;
}