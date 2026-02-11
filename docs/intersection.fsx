(**

---
title: 2D Intersection
category: 2D Modules
categoryindex: 3
index: 7.5
---

# Intersection 2D

The `Intersection2D` module provides functions to find points where different geometric objects cross each other.

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
# Line Segment and Line
*)

let segment : LineSegment2D<Meters, Cartesian> = LineSegment2D.from (Point2D.meters 0. 0.) (Point2D.meters 2. 2.)
let line : Line2D<Meters, Cartesian> = Line2D.through (Point2D.meters 0. 2.) (Point2D.meters 2. 0.)

Intersection2D.lineSegmentAndLine segment line
(*** include-it ***)

(**
# Bounding Box and Line
*)

let bbox : BoundingBox2D<Meters, Cartesian> = BoundingBox2D.from (Point2D.meters 0. 0.) (Point2D.meters 2. 2.)
Intersection2D.boundingBoxAndLine bbox line
(*** include-it ***)
