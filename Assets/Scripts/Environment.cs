using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundLayerInteraction
{
    void CheckGroundLayer(Transform current, LayerMask ground);
}

public interface IGravityChanges
{
    IEnumerator FallDown();
}