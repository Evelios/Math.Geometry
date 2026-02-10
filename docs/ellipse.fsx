(**

---
title: 2D Ellipse
category: 2D Modules
categoryindex: 3
index: 6
---

# Ellipse 2D


*)
(*** hide ***)

#r "../Math.Geometry/bin/Debug/net6.0/Math.Geometry.dll"
#r "../Math.Geometry/bin/Release/net6.0/Math.Geometry.dll"

#r "nuget: Math.Units"

open Math.Geometry
open Math.Units

type Cartesian = Cartesian

(**
# Builders
*)

Ellipse2D.from (Point2D.meters 4. 4.) Direction2D.x (Length.meters 2.) (Length.meters 4.)
(*** include-it ***)

(**
# Accessors
*)
let ellipse: Ellipse2D<Meters, Cartesian> =
    Ellipse2D.from (Point2D.meters 4. 4.) Direction2D.x (Length.meters 2.) (Length.meters 4.)

(***)

Ellipse2D.axes ellipse // or
ellipse.Axes
(*** include-it ***)

Ellipse2D.xRadius ellipse // or
ellipse.XRadius
(*** include-it ***)

Ellipse2D.yRadius ellipse // or
ellipse.YRadius
(*** include-it ***)

Ellipse2D.centerPoint ellipse
(*** include-it ***)

Ellipse2D.xAxis ellipse
(*** include-it ***)

Ellipse2D.yAxis ellipse
(*** include-it ***)

Ellipse2D.xDirection ellipse
(*** include-it ***)

Ellipse2D.yDirection ellipse
(*** include-it ***)

Ellipse2D.area ellipse
(*** include-it ***)


(**
# Modifiers
*)

(**
Scale an ellipse about a given point by a given scale.
*)
Ellipse2D.scaleAbout Point2D.origin 1.5 ellipse
(*** include-it ***)

(**
Rotate an ellipse around a given point by a given angle.
*)
Ellipse2D.rotateAround Point2D.origin (Angle.degrees 45.0) ellipse
(*** include-it ***)

(**
Translate an ellipse by a given displacement.
*)
Ellipse2D.translateBy (Vector2D.meters 2. 0.) ellipse
(*** include-it ***)

(**
Translate an ellipse in a given direction by a given distance.
*)
Ellipse2D.translateIn Direction2D.y (Length.meters 1.0) ellipse
(*** include-it ***)

(**
Mirror an ellipse across a given axis.
*)
Ellipse2D.mirrorAcross Axis2D.x ellipse
(*** include-it ***)

(**
Take an ellipse defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Ellipse2D.relativeTo frame ellipse
(*** include-it ***)

(**
Take an ellipse defined in local coordinates and return it expressed in global coordinates.
*)
let localEllipse = Ellipse2D.relativeTo frame ellipse
Ellipse2D.placeIn frame localEllipse
(*** include-it ***)
