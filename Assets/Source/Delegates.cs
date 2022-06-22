using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    // This file contains a bunch of delegate classes we can use for events.
    public delegate void FloatValueDelegate(float value);
    public delegate void BoolDelegate(bool value);
    public delegate void ConfirmationDelegate(object parameter, bool confirmationValue);
    public delegate void LevelManagerDelegate(LevelManager levelManager);
    public delegate void EmptyDelegate();
}