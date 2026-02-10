(**

---
title: 2D Direction
category: 2D Modules
categoryindex: 3
index: 5
---


*)
(*** hide ***)

#r "../Math.Geometry/bin/Debug/net6.0/Math.Geometry.dll"
#r "../Math.Geometry/bin/Release/net6.0/Math.Geometry.dll"

#r "nuget: Math.Units"


open Math.Geometry
open Math.Units

type Cartesian = Cartesian
(***)

(**
# Direction 2D

A direction can be though of as a normalized vector or can be though of just something pointing in a particular
direction on the 2D cartesian plane. When looking at the type of a direction, you can notice that it doesn't have any
unit type, `Direction2D<'Coordinates>`. This is because a direction _always_ has a magnitude of 1 and doesn't need any
unit qualifier.
*)

(** # Builders *)

(**
`Direction2D.xy` first checks to make sure that the input components are both non-zero, then normalizes them to
guarantee a valid direction is created.
*)

Direction2D.xy 3. 4.
(***hide***)
Direction2D.xy 3. 4. |> Option.map Direction2D.toTuple
(*** include-it ***)

(**
When both values are zero, a proper direction cannot be created, so the program returns a `None` option.
(Appears as `<null>` in the terminal)
*)

Direction2D.xy 0. 0.
(*** hide ***)
Direction2D.xy 0. 0. |> Option.map Direction2D.toTuple
(*** include-it ***)

(**
You can also create a direction using similar functionality to `Direction2D.xy` but with _tuples_ as an input instead.
*)
Direction2D.fromComponents (3., 4.)
(*** hide ***)


Direction2D.positiveY |> Direction2D.toAngle
(*** include-it ***)

(** # Constants *)

Direction2D.positiveX
Direction2D.x

Direction2D.positiveY
Direction2D.y

Direction2D.negativeX

Direction2D.negativeY

(** # Accessors *)

(*** hide ***)
let direction: Direction2D<Cartesian> = Direction2D.xyUnsafe 1. 0. |> Direction2D.rotateBy (Angle.degrees 30.0)

(***)

Direction2D.toAngle direction
(*** include-it ***)

Direction2D.toTuple direction
(*** include-it ***)

Direction2D.xComponent direction
(*** include-it ***)

Direction2D.yComponent direction
(*** include-it ***)


(** # Modifiers *)

Direction2D.reverse direction
(*** include-it ***)

Direction2D.rotateClockwise direction
(*** include-it ***)

Direction2D.rotateCounterclockwise direction
(*** include-it ***)

Direction2D.perpendicularTo direction
(*** include-it ***)

Direction2D.mirrorAcross Axis2D.x direction
(*** include-it ***)

(** # Queries *)

let otherDirection = Direction2D.fromAngle (Angle.degrees 45.0)

Direction2D.equalWithin (Angle.degrees 1.0) otherDirection direction
(*** include-it ***)

Direction2D.componentIn otherDirection direction
(*** include-it ***)

Direction2D.angleFrom direction otherDirection
(*** include-it ***)

(** # Coordinate Conversions *)

let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.) |> Frame2D.rotateBy (Angle.degrees 45.0)

Direction2D.relativeTo frame direction
(*** include-it ***)

(**
Take a direction defined in local coordinates and return it expressed in global coordinates.
*)
let localDirection = Direction2D.relativeTo frame direction
Direction2D.placeIn frame localDirection
(*** include-it ***)
