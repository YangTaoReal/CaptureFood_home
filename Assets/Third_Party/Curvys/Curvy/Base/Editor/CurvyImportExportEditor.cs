// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using UnityEditor;
using System.Collections;
using FluffyUnderware.Curvy;

namespace FluffyUnderware.CurvyEditor
{
    [CustomEditor(typeof(CurvyImportExport))]
    public class CurvyImportExportEditor : CurvyEditorBase<CurvyImportExport>
    {
        
        void ShowImportButton()
        {
            GUI.enabled = (!string.IsNullOrEmpty(Target.FilePath));
            if (GUILayout.Button("Import"))
            {
            }
            GUI.enabled = true;
        }

        void ShowExportButton()
        {
            GUI.enabled = (!string.IsNullOrEmpty(Target.FilePath));
            if (GUILayout.Button("Export"))
            {
            }
            GUI.enabled = true;
        }

       
    }
}
