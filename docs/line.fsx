(**

---
title: 2D Line
category: 2D Modules
categoryindex: 3
index: 7.8
---

# Line 2D

An infinite line in 2D space.

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
# Builders
*)

let p1 = Point2D.meters 0. 0.
let p2 = Point2D.meters 2. 2.

Line2D.through p1 p2
(***)

Line2D.fromPointAndVector p1 (Vector2D.meters 1. 0.)
(***)

(**
# Accessors
*)

let line = Line2D.through p1 p2

Line2D.direction line
(*** include-it ***)

Line2D.axis line
(*** include-it ***)

(**
# Modifiers
*)

Line2D.round line
(*** include-it ***)

(**
# Queries
*)

Line2D.pointClosestTo (Point2D.meters 0. 2.) line
(*** include-it ***)

Line2D.distanceToPoint (Point2D.meters 0. 2.) line
(*** include-it ***)

Line2D.isPointOnLine (Point2D.meters 1. 1.) line
(*** include-it ***)

let otherLine = Line2D.through (Point2D.meters 0. 2.) (Point2D.meters 2. 0.)

Line2D.areParallel line otherLine
(*** include-it ***)

Line2D.arePerpendicular line otherLine
(*** include-it ***)

Line2D.intersect line otherLine
(*** include-it ***)
