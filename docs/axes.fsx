(**

---
title: 2D Axis
category: 2D Modules
categoryindex: 3
index: 2
---

# Axis 2D

An axis is a line that goes through a point at a particular angle.

*)
(*** hide ***)

#r "../Math.Geometry/bin/Debug/net6.0/Math.Geometry.dll"
#r "../Math.Geometry/bin/Release/net6.0/Math.Geometry.dll"

#r "nuget: Math.Units"

open Math.Geometry
open Math.Units

type Cartesian = Cartesian

(**
# Constants
*)

Axis2D.x
Axis2D.y

(**
# Builders

For the following, the input direction that will be used is
*)
let direction: Direction2D<Cartesian> = Direction2D.fromAngle (Angle.degrees 45.)
(*** hide ***)
direction
(*** include-it ***)

Axis2D.through (Point2D.meters 3. 3.) direction
(*** include-it ***)

Axis2D.withDirection direction (Point2D.meters 3. 3.)
(*** include-it ***)

Axis2D.throughPoints (Point2D.meters 3. 3.) (Point2D.meters 5. 5.)
(*** include-it ***)

(**
# Accessors

To show the output of the accessor methods, the following axis will be used.
*)

let axis = Axis2D.through (Point2D.meters 3. 3.) direction
(*** hide ***)
axis
(*** include-it ***)


Axis2D.originPoint axis // or
axis.Origin
(*** include-it ***)

Axis2D.direction axis // or
axis.Direction
(*** include-it ***)


(**
# Modifiers
*)

(**
Reverse the direction of an axis.
*)
Axis2D.reverse axis
(*** include-it ***)

(**
Move an axis so that its origin point is the given point.
*)
Axis2D.moveTo (Point2D.meters -1. 4.) axis
(*** include-it ***)

(**
Rotate an axis counterclockwise by a given angle around a given center point.
*)
Axis2D.rotateAround Point2D.origin (Angle.degrees 90.0) axis
(*** include-it ***)

(**
Rotate an axis counterclockwise by a given angle around its own origin point.
*)
Axis2D.rotateBy (Angle.degrees 45.0) axis
(*** include-it ***)

(**
Translate an axis by a given displacement.
*)
Axis2D.translateBy (Vector2D.meters 2. 3.) axis
(*** include-it ***)

(**
Translate an axis in a given direction by a given distance.
*)
Axis2D.translateIn Direction2D.y (Length.meters 1.5) axis
(*** include-it ***)

(**
Mirror an axis across another axis.
*)
Axis2D.mirrorAcross Axis2D.x axis
(*** include-it ***)

(**
Take an axis defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Axis2D.relativeTo frame axis
(*** include-it ***)

(**
Take an axis defined in local coordinates and return it expressed in global coordinates.
*)
let localAxis = Axis2D.relativeTo frame axis
Axis2D.placeIn frame localAxis
(*** include-it ***)
