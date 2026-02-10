(**

---
title: 2D Vectors
category: 2D Modules
categoryindex: 3
index: 14
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
# Constructors

Vectors can be created from the __x__ and __y__ components.
*)

Vector2D.xy (Length.meters 3.) (Length.meters 4.)

(**
You can also create a vector in polar coordinates.
*)
Vector2D.rTheta (Length.meters 5.) Angle.halfPi

(**
Using a vector __direction__ you can create a vector of a given length following that direction.
*)
Vector2D.withQuantity (Length.meters 5.) (Direction2D.fromAngle Angle.halfPi)

(** # Accessors *)

let vec: Vector2D<Meters, Cartesian> =
    Vector2D.xy (Length.meters 3.) (Length.meters 4.)

vec.X = Vector2D.x vec
vec.Y = Vector2D.y vec

(***)

Vector2D.magnitude vec
(*** include-it ***)

(**
# Operators

Operators are provided for more concise syntax when dealing with vectors. All
of these symbol operators have a corresponding function that you can use as
well. These are often used when using functions like `List.map` or when used
in piping operations (`|>`). Pay attention to the order of the arguments!
Because these functions are intended to be used in pipes the order goes like
`Vector2D.add rhs lhs`.
*)


(*** hide ***)
let lhs: Vector2D<Meters, Cartesian> = Vector2D.meters 3. 4.

let rhs: Vector2D<Meters, Cartesian> = Vector2D.meters 5. 12.
(***)

lhs |> Vector2D.plus rhs = lhs + rhs
(*** include-it ***)
(**

The following table is the complete list of operators that can be used on
vector objects.

| Operator | Lhs    | Rhs    | Return Type | Example     | Function |
|----------|--------|--------|-------------|-------------|----------|
| -        | Vector |        | Vector      | `-vec`      | `Vector2D.neg` |
| +        | Vector | Vector | Vector      | `lhs + rhs` | `Vector2D.plus` |
| -        | Vector | Vector | Vector      | `lhs - rhs` | `Vector2D.minus` |
| *        | Vector | float  | Vector      | `lhs * 0.5` | `Vector2D.times` & `Vector2D.scaleBy` |
| *        | float  | Vector | Vector      | `0.5 * rhs` | `None` |
| /        | Vector | float  | Vector      | `lhs / 4.`  | `Vector2D.dividedBy` |
*)

(** # Trigonometry *)

(** $$ lhs \cdot rhs $$ *)
Vector2D.dot lhs rhs
(*** include-it ***)

(** $$ lhs \times rhs $$ *)
Vector2D.cross lhs rhs
(*** include-it ***)

(** # Modifiers *)

(**
Scale a vector to a given length.
*)
vec |> Vector2D.scaleTo (Length.meters 10.0)
(*** include-it ***)

(**
Rotate a vector counterclockwise by a given angle.
*)
vec |> Vector2D.rotateBy (Angle.degrees 90.0)
(*** include-it ***)

(**
Mirror a vector across a given axis.
*)
vec |> Vector2D.mirrorAcross Axis2D.x
(*** include-it ***)

(** # Coordinate Conversions *)

(**
Take a vector defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Vector2D.relativeTo frame vec
(*** include-it ***)

(**
Take a vector defined in local coordinates and return it expressed in global coordinates.
*)
let localVec = Vector2D.relativeTo frame vec
Vector2D.placeIn frame localVec
(*** include-it ***)
