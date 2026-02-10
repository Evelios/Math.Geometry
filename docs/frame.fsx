(**

---
title: 2D Frame
category: 2D Modules
categoryindex: 3
index: 7
---


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

Frame2D.atOrigin
(***)

Frame2D.atPoint (Point2D.meters 4. 2.)
(***)

Frame2D.withXDirection (Direction2D.fromAngle (Angle.degrees 30.)) (Point2D.meters 3. 1.)
(***)

Frame2D.withYDirection (Direction2D.fromAngle (Angle.degrees -60.)) (Point2D.meters -1. 2.)
(***)

Frame2D.withAngle (Angle.degrees 20.) (Point2D.meters 2. 3.)
(***)

let axis: Axis2D<Meters, Cartesian> =
    Axis2D.through (Point2D.meters 3. 2.) (Direction2D.fromAngle Angle.pi)
(***)

Frame2D.fromXAxis axis
(***)

Frame2D.fromYAxis axis
(***)

(**
# Accessors
*)

let frame: Frame2D<Meters, Cartesian, unit> =
    Frame2D.withAngle (Angle.degrees 20.) (Point2D.meters 2. 3.)
(***)

Frame2D.originPoint frame // or

frame.Origin
(*** include-it ***)

Frame2D.xDirection frame // or

frame.XDirection
(*** include-it ***)

Frame2D.yDirection frame // or

frame.YDirection
(*** include-it ***)

Frame2D.isRightHanded frame
(*** include-it ***)

Frame2D.yAxis frame
(*** include-it ***)

Frame2D.xAxis frame
(*** include-it ***)

(**
# Modifiers
*)

(**
Reverse the X direction of a frame.
*)
Frame2D.reverseX frame
(*** include-it ***)

(**
Reverse the Y direction of a frame.
*)
Frame2D.reverseY frame
(*** include-it ***)

(**
Move a frame so that it has the given origin point.
*)
Frame2D.moveTo Point2D.origin frame
(*** include-it ***)

(**
Rotate a frame counterclockwise by a given angle around its own origin point.
*)
Frame2D.rotateBy (Angle.degrees 30.) frame
(*** include-it ***)

(**
Rotate a frame counterclockwise around a given point by a given angle.
*)
Frame2D.rotateAround (Point2D.meters -3. -2.) (Angle.degrees 70.) frame
(*** include-it ***)

(**
Translate a frame by a given displacement.
*)
Frame2D.translateBy (Vector2D.meters 4. 7.) frame
(*** include-it ***)

(**
Translate a frame in a given direction by a given distance.
*)
Frame2D.translateIn Direction2D.y (Length.meters 1.0) frame
(*** include-it ***)

(**
Translate a frame along one of its own axes.
*)
Frame2D.translateAlongOwn Frame2D.xAxis (Length.meters 2.0) frame
(*** include-it ***)

(**
Mirror a frame across a given axis.
*)
Frame2D.mirrorAcross Axis2D.x frame
(*** include-it ***)

(**
Take one frame and express it relative to another frame.
*)
let otherFrame = Frame2D.atPoint (Point2D.meters 1. 1.)
Frame2D.relativeTo otherFrame frame
(*** include-it ***)

(**
Take a frame expressed in local coordinates and express it in global coordinates.
*)
Frame2D.placeIn otherFrame frame
(*** include-it ***)
