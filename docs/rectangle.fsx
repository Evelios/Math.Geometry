(**

---
title: 2D Rectangle
category: 2D Modules
categoryindex: 3
index: 12
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
# Rectangle 2D

A `Rectangle2D` is defined by its central axes (a `Frame2D`) and its dimensions (width and height).

# Builders

You can create an axis-aligned rectangle from two diagonally opposite points.
*)

let p1 = Point2D.meters 0. 0.
let p2 = Point2D.meters 3. 2.

Rectangle2D.fromPoints p1 p2
(***)

(**
Or by providing dimensions, an angle, and a center point.
*)

let size = Size2D.create (Length.meters 4.) (Length.meters 2.)
let center = Point2D.meters 5. 5.
let angle = Angle.degrees 45.

Rectangle2D.withDimensions size angle center
(***)

(**
# Accessors
*)

let rect = Rectangle2D.fromPoints p1 p2

Rectangle2D.centerPoint rect
(*** include-it ***)

Rectangle2D.dimensions rect
(*** include-it ***)

Rectangle2D.area rect
(*** include-it ***)

Rectangle2D.vertices rect
(*** include-it ***)

Rectangle2D.edges rect
(*** include-it ***)

(**
# Modifiers
*)

rect |> Rectangle2D.rotateAround Point2D.origin (Angle.degrees 90.)
(***)

rect |> Rectangle2D.translateBy (Vector2D.meters 1. 1.)
(***)

(**
# Queries
*)

Rectangle2D.contains (Point2D.meters 1. 1.) rect
(*** include-it ***)

Rectangle2D.boundingBox rect
(*** include-it ***)
