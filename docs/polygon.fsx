(**

---
title: 2D Polygon
category: 2D Modules
categoryindex: 3
index: 10
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

let outerPoints: Point2D<Meters, Cartesian> list =
    [ Point2D.meters 5. 0.
      Point2D.meters 7. 2.
      Point2D.meters 3. 6.
      Point2D.meters 2. 4.
      Point2D.meters 0. 2. ]

Polygon2D.singleLoop outerPoints
(***)

let innerPoints: Point2D<Meters, Cartesian> list =
    [ Point2D.meters 3. 1.; Point2D.meters 2. 2.; Point2D.meters 1. 1. ]

Polygon2D.withHoles [ innerPoints ] outerPoints
(***)

Polygon2D.convexHull
(***)

(**
# Accessors
*)

let polygon: Polygon2D<Meters, Cartesian> =
    Polygon2D.withHoles [ innerPoints ] outerPoints

Polygon2D.outerLoop polygon // or
polygon.OuterLoop
(*** include-it ***)

Polygon2D.innerLoops polygon // or
polygon.InnerLoops
(*** include-it ***)

Polygon2D.vertices polygon
(***)

Polygon2D.loopEdges (Polygon2D.vertices polygon)
(***)

Polygon2D.edges polygon
(***)

Polygon2D.perimeter polygon
(*** include-it ***)

Polygon2D.area polygon
(*** include-it ***)

Polygon2D.centroid polygon
(*** include-it ***)

Polygon2D.boundingBox polygon
(*** include-it ***)

(**
# Modifiers
*)

(**
Scale a polygon about a given center point by a given scale.
*)
Polygon2D.scaleAbout Point2D.origin 1.5 polygon
(*** include-it ***)

(**
Rotate a polygon around the given center point counterclockwise by the given angle.
*)
Polygon2D.rotateAround Point2D.origin (Angle.degrees 45.0) polygon
(*** include-it ***)

(**
Translate a polygon by the given displacement.
*)
Polygon2D.translateBy (Vector2D.meters 2. 0.) polygon
(*** include-it ***)

(**
Translate a polygon in a given direction by a given distance.
*)
Polygon2D.translateIn Direction2D.y (Length.meters 1.0) polygon
(*** include-it ***)

(**
Mirror a polygon across the given axis.
*)
Polygon2D.mirrorAcross Axis2D.x polygon
(*** include-it ***)

(**
Take a polygon defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Polygon2D.relativeTo frame polygon
(*** include-it ***)

(**
Take a polygon defined in local coordinates and return it expressed in global coordinates.
*)
Polygon2D.placeIn frame polygon
(*** include-it ***)

(**
# Queries
*)

Polygon2D.contains (Point2D.meters 2. 2.) polygon
(*** include-it ***)

Polygon2D.contains (Point2D.meters 10. 10.) polygon
(*** include-it ***)
