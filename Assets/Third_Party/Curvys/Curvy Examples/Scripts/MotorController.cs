// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;
using FluffyUnderware.Curvy.Controllers;
using FluffyUnderware.DevTools;

namespace FluffyUnderware.Curvy.Examples
{
    public class MotorController : SplineController
    {
        [Section("Motor")]
        public float MaxSpeed = 30;

        protected override void Update()
        {
            Speed = Input.GetAxis("Vertical") * MaxSpeed;
            base.Update();
        }
    }
}
