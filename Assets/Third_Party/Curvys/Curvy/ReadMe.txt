// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

GETTING STARTED
===============
Visit https://curvyeditor.com to access documentation, tutorials and references

EXAMPLE SCENES
==============
Checkout the example scenes at  "Packages/Curvy Examples/Scenes"!

NEED FURTHER HELP
=================
Visit our support forum at https://forum.curvyeditor.com

VERSION HISTORY
===============
2.2.1
	[CHANGE] Modified the UI of the CG module "Create Mesh" to avoid confusion regarding the "Make Static" option:
		- "Make Static" is now not editable in play mode, since its Unity equivalent (GameObject.IsStatic) is an editor only property.
		- When "Make Static" is true, the other options are not editable while in play mode. This is to reflect the behaviour of the "Create Mesh" module, which is to not update the mesh while under those conditions, to avoid overriding the optimizations Unity do to static game objects'meshs.
	[FIX] When combining multiple Volumes having different values for the "Generate UV" setting, the created mesh has invalid UVs
	[FIX] "Mesh.normals is out of bounds" error when Generating a mesh that has Caps while using the Combine option
	[FIX] Convex property, in CG module Create Mesh, not applied on generated mesh collider
	[FIX] Negative SwirlTurns are ignored
	[FIX] Orientation interpolated the wrong way (Lerping instead of SLerping)
	[FIX] Cross's "Reverse Normal" in "Shape Extrusion" module is ignored when a "Volume Hollow" is set
	[FIX] Crash on IOS when using code stripping on Unity 2017.2 and above.
	[Optimization] Various optimizations, the most important ones are related to "Shape Extrusion"'s normals computations and Orientation computation
	[API] Added a new GetNearestPointTF overload that also returns the nearestSegment and the nearestSegmentF
	[API] Made CrossReverseNormals, HollowInset and HollowReverseNormals properties public in BuildShapeExtrusion
2.2.0
	[NEW] Addressed Unity 2017.3 incompatibilities
	[NEW] Added a RendererEnabled option to the CreateMesh CG module. Useful if you generate a mesh for collider purposes only.
	[FIX] Error when using pooling with Unity 2017.2 and above
	[FIX] Incompatibility with UWP10 build
	[FIX] SceneSwitcher.cs causing issues with the global namespace of Scene Switcher being occupied by the PS4's SDK
	[FIX] Curvy crashing when compiled with the -checked compiler option
	[FIX] TRSShape CG module not updating properly the shape's normals
	[FIX] ReverseNormals not reversing normals in some cases
	      Note: You might have ticked "Reverse Normals" in some of your Curvy Generators, but didn't notice it because of the bug. Now that the bug is fixed, those accidental "Reverse Normals" will get activated.
	[FIX] Split meshes not having the correct normals
	[CHANGE] Replaced website, documentation and forum URLs with the new ones.
	[Optimization] Various optimizations, the most important ones are related to mesh generation (UVs, normals and tangents computation)
2.1.3
	[FIX] TimeScale affects controller movement when Animate is off
	[FIX] Reverse spline movement going wrong under some rare conditions
2.1.2
	[NEW] Added CreatePathLineRenderer CG module
	[NEW] Addressed Unity 5.5 incompatibilities
	[FIX] SplineController.AdaptOnChange failing under some conditions
	[FIX] Selecting a spline while the Shape wizard is open immediately changes it's shape
	[FIX] ModifierMixShapes module not generating normals
	[CHANGE] Changed 20_CGPath example to showcase CreatePathLineRenderer module
2.1.1
	[NEW] Added CurvySplineBase.GetApproximationPoints
	[NEW] Added Offsetting and offset speed compensation to CurvyController
	[FIX] ImportExport toolbar button ignoring ShowGlobalToolbar option
	[FIX] Assigning CGDataReference to VolumeController.Volume and PathController.Path fails at runtime
	[CHANGE] OrientationModeEnum and OrientationAxisEnum moved from CurvyController to FluffyUnderware.Curvy namespace
	[CHANGE] ImportExport Wizard now cuts text and logs a warning if larger then allowed by Unity's TextArea
2.1.0
	[NEW] More options for the Mesh Triangulation wizard
	[NEW] Improved Spline2Mesh and SplinePolyLine classes for better triangulator support
	[NEW] BuildVolumeCaps performance heavily improved
	[NEW] Added preference option to hide _CurvyGlobal_ GameObject
	[NEW] Import/Export API & wizard for JSON serialization of Splines and Control Points (Catmull-Rom & Bezier)
	[NEW] Added 22_CGClonePrefabs example scene
	[NEW] Windows Store compatiblity (Universal 8.1, Universal 10)
	[FIX] BuildVolumeMesh.KeepAspect not working properly
	[FIX] CreateMesh.SaveToScene() not working properly
	[FIX] NRE when using CreateMesh module's Mesh export option
	[FIX] Spline layer always resets to default spline layer
	[FIX] CurvySpline.TFToSegmentIndex returning wrong values
	[FIX] SceneSwitcher helper script raise errors at some occasions
	[CHANGE] Setting CurvyController.Speed will only change movement direction if it had a value of 0 before
	[CHANGE] Dropped poly2tri in favor of LibTessDotNet for triangulation tasks
	[CHANGE] Removed all legacy components from Curvy 1.X
	[CHANGE] New Control Points now use the spline's layer
2.0.5
	[NEW] Added CurvyGenerator.FindModule<T>()
	[NEW] Added InputSplineShape.SetManagedShape()
	[NEW] Added 51_InfiniteTrack example scene
	[NEW] Added CurvyController.Pause()
	[NEW] Added CurvyController.Apply()
	[NEW] Added CurvyController.OnAnimationEnd event
	[NEW] Added option to select Connection GameObject to Control Point inspector
	[FIX] UV2 calculation not working properly
	[FIX] CurvyController.IsInitialized becoming true too early
	[FIX] Controller Damping not working properly when moving backwards
	[FIX] Control Point pool keeps invalid objects after scene load
	[FIX] _CurvyGlobal_ frequently causes errors in editor when switching scenes
	[FIX] Curve Gizmo drawing allocating memory unnecessarily
	[FIX] SplineController allocates memory at some occasions
	[FIX] CurvyDefaultEventHandler.UseFollowUp causing Stack Overflow/Unity crashing
	[FIX] _CurvyGlobal_ GameObject disappearing by DontDestroyOnLoad bug introduced by Unity 5.3
	[CHANGE] UITextSplineController resets state when you disable it
	[CHANGE] CurvyGenerator.OnRefresh() now returns the first changed module in CGEventArgs.Module
	[CHANGE] Renamed CurvyControlPointEventArgs.AddMode to ModeEnum, changed content to "AddBefore","AddAfter","Delete","None"
2.0.4
	[FIX] Added full Unity 5.3 compatibility
2.0.3
	[NEW] Added Pooling example scene
	[NEW] Added CurvyGLRenderer.Add() and CurvyGLRenderer.Delete()
	[FIX] CG graph not refreshing properly
	[FIX] CG module window background rendering transparent under Unity 5.2 at some occasions
	[FIX] Precise Movement over connections causing position warps
	[FIX] Fixed Curvy values resetting to default editor settings on upgrade
	[FIX] Control Points not pooled when deleting spline
	[FIX] Pushing Control Points to pool at runtime causing error
	[FIX] Bezier orientation not updated at all occasions
	[FIX] MetaCGOptions: Explicit U unable to influence faces on both sides of hard edges
	[FIX] Changed UITextSplineController to use VertexHelper.Dispose() instead of VertexHelper.Clear()
	[FIX] CurvySplineSegment.ConnectTo() fails at some occasions
2.0.2
	[NEW] Added range option to InputSplinePath / InputSplineShape modules
	[NEW] CG editor improvements
	[NEW] Added more Collider options to CreateMesh module
	[NEW] Added Renderer options to CreateMesh module
	[NEW] Added CurvySpline.IsPlanar(CurvyPlane) and CurvySpline.MakePlanar(CurvyPlane)
	[NEW] Added CurvyController.DampingDirection and CurvyController.DampingUp
	[FIX] Shift ControlPoint Toolbar action fails with some Control Points
	[FIX] IOS deployment code stripping (link.xml)
	[FIX] Controller Inspector leaking textures
	[FIX] Controllers refreshing when Speed==0
	[FIX] VolumeController not using individual faces at all occasions
	[FIX] Unity 5.2.1p1 silently introduced breaking changes in IMeshModifier
	[CHANGE] CurvyController.OrientationDamping now obsolete!
2.0.1
	[NEW] CG path rasterization now has a dedicated angle threshold
	[NEW] Added CurvyController.ApplyTransformPosition() and CurvyController.ApplyTransformRotation()
	[FIX] CG not refreshing as intended in the editor
	[FIX] CG not refreshing when changing used splines
	[FIX] Controllers resets when changing inspector while playing
	A few minor fixes and improvements
2.0.0 Initial Curvy 2 release


THIRD PARTY SOFTWARE USED BY CURVY
=======================================

==== DevTools ====
Copyright (c) 2013-2018 FluffyUnderware, http://www.fluffyunderware.com

All rights reserved.


==== LibTessDotNet ====

SGI FREE SOFTWARE LICENSE B (Version 2.0, Sept. 18, 2008)
Copyright 2000, Silicon Graphics, Inc. All Rights Reserved.  
Copyright 2012, Google Inc. All Rights Reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice including the dates of first publication and
either this permission notice or a reference to http://oss.sgi.com/projects/FreeB/
shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
SILICON GRAPHICS, INC. BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Except as contained in this notice, the name of Silicon Graphics, Inc. shall not
be used in advertising or otherwise to promote the sale, use or other dealings
in this Software without prior written authorization from Silicon Graphics, Inc.

Original Code. The Original Code is: OpenGL Sample Implementation,
Version 1.2.1, released January 26, 2000, developed by Silicon Graphics,
Inc. The Original Code is Copyright (c) 1991-2000 Silicon Graphics, Inc.
Copyright in any portions created by third parties is as indicated
elsewhere herein. All Rights Reserved.
