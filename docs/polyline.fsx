(**

---
title: 2D Poly-Line
category: 2D Modules
categoryindex: 3
index: 11
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
# Polyline 2D

A `Polyline2D` represents a series of connected line segments.

# Builders

You can create a polyline from a list of points.
*)

let vertices : Point2D<Meters, Cartesian> list =
    [ Point2D.meters 0. 0.
      Point2D.meters 1. 1.
      Point2D.meters 2. 0.
      Point2D.meters 3. 1. ]

let polyline : Polyline2D<Meters, Cartesian> = Polyline2D.fromVertices vertices
(***)

(**
# Accessors
*)

Polyline2D.vertices polyline
(*** include-it ***)

Polyline2D.segments polyline
(*** include-it ***)

Polyline2D.length polyline
(*** include-it ***)

Polyline2D.centroid polyline
(*** include-it ***)

(**
# Modifiers
*)

polyline |> Polyline2D.translateBy (Vector2D.meters 1. 0.)
(***)

polyline |> Polyline2D.rotateAround Point2D.origin (Angle.degrees 90.)
(***)

(**
# Queries
*)

Polyline2D.boundingBox polyline
(*** include-it ***)
