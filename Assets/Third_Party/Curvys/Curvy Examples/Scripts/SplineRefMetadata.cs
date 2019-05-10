// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;

namespace FluffyUnderware.Curvy.Examples
{
    public class SplineRefMetadata : MonoBehaviour, ICurvyMetadata
    {
        public CurvySpline Spline;
        public CurvySplineSegment CP;
        public string Options;
    }
}
