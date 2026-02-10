(**

---
title: 2D Points
category: 2D Modules
categoryindex: 3
index: 9
---


*)
(*** hide ***)


#r "../Math.Geometry/bin/Debug/net6.0/Math.Geometry.dll"
#r "../Math.Geometry/bin/Release/net6.0/Math.Geometry.dll"

#r "nuget: Math.Units"

open Math.Geometry
open Math.Units

(***)

(**
# Constructors

Points can be created from the __x__ and __y__ components.
*)

Point2D.xy (Length.meters 3.) (Length.meters 4.)

(**
You can use either `rTheta` or `polar` when using polar coordinates.
*)
Point2D.rTheta (Length.meters 5.) Angle.halfPi

Point2D.polar (Length.meters 5.) Angle.halfPi



(**
# Operators

Operators are provided for more concise syntax when dealing with points. All
of these symbol operators have a corresponding function that you can use as
well. These are often used when using functions like `List.map` or when used
in piping operations (`|>`). Pay attention to the order of the arguments!
Because these functions are intended to be used in pipes the order goes like
`Point2D.add rhs lhs`.
*)


(*** hide ***)
type Cartesian = Cartesian

let point: Point2D<Meters, Cartesian> = Point2D.meters 3. 4.
let secondPoint: Point2D<Meters, Cartesian> = Point2D.meters 8. 16.
let vec: Vector2D<Meters, Cartesian> = Vector2D.meters 5. 12.

(**
The following three statements are equivalent.
$$ Point2D + Vector2D = Point2D $$
*)

Point2D.translate vec point

point |> Point2D.plus vec

point + vec
(*** include-it ***)


(** $$ Point2D - Vector2D = Point2D $$ *)
point - vec
(*** include-it ***)


(** $$ Point2D - Point2D = Vector2D $$ *)
secondPoint - point
(*** include-it ***)
(**

The following table is the complete list of operators that can be used on
point objects.

| Operator | Lhs    | Rhs    | Return Type | Example          | Function        |
|----------|--------|--------|-------------|------------------|-----------------|
| -        | Point  |        | Point      | `-vec`      | `Point2D.neg` |
| +        | Point  | Vector | Point       | `point + vec`    | `Point2D.plus`  |
| -        | Point  | Vector | Point       | `point - vec`    | `None`          |
| -        | Point  | Point  | Vector      | `p2 - p1`        | `Point2D.minus` |
| *        | Point  | float  | Point       | `point * 0.5`    | `Point2D.times` |
| *        | float  | Point  | Point       | `0.5 * point`    | `None`          |
| /        | Point  | float  | Point       | `point / 4.`     | `Point2D.dividedBy` |

*)

(** # Accessors *)

Point2D.x point
(*** include-it ***)

Point2D.y point
(*** include-it ***)

Point2D.magnitude point
(*** include-it ***)

Point2D.coordinates point
(*** include-it ***)

(** # Centroids *)

Point2D.centroid point [ secondPoint ]
(*** include-it ***)

Point2D.centroid3 point secondPoint (Point2D.meters 0. 0.)
(*** include-it ***)

(** # Modifiers *)

point |> Point2D.scaleTo (Length.meters 10.0)
(*** include-it ***)

point |> Point2D.rotateBy (Angle.degrees 45.0)
(*** include-it ***)

point |> Point2D.rotateAround Point2D.origin (Angle.degrees 90.0)
(*** include-it ***)

point |> Point2D.translateBy (Vector2D.meters 1. 2.)
(*** include-it ***)

point |> Point2D.mirrorAcross Axis2D.x
(*** include-it ***)

point |> Point2D.scaleAbout Point2D.origin 2.0
(*** include-it ***)

(** # Queries *)

Point2D.equalWithin (Length.meters 0.001) point secondPoint
(*** include-it ***)

Point2D.distanceTo point secondPoint
(*** include-it ***)

Point2D.midpoint point secondPoint
(*** include-it ***)

Point2D.interpolateFrom point secondPoint 0.5
(*** include-it ***)

Point2D.projectOnto Axis2D.x point
(*** include-it ***)

(** # Coordinate Conversions *)

let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)

Point2D.relativeTo frame point
(*** include-it ***)

(**
Take a point defined in local coordinates and return it expressed in global coordinates.
*)
let localPoint = Point2D.relativeTo frame point
Point2D.placeIn frame localPoint
(*** include-it ***)
