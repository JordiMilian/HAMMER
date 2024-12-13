using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iCutsceneable
{
    event Action onFinished;
    public void playCutscene();
    public void ForceEndCutscene();
}
