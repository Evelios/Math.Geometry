(**

---
title: 2D Boolean Operations
category: 2D Modules
categoryindex: 3
index: 3.5
---

# Boolean 2D

The `Boolean2D` module provides functions for performing boolean-like operations on geometric shapes, such as clipping or finding shared segments.

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
# Bounding Box and Line

Find the portion of a line that lies within a bounding box.
*)

let bbox = BoundingBox2D.from (Point2D.meters 0. 0.) (Point2D.meters 2. 2.)
let line = Line2D.from (Point2D.meters -1. 1.) (Point2D.meters 3. 1.)

Boolean2D.boundingBoxAndLine bbox line
(*** include-it ***)
