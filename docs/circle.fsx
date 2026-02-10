(**

---
title: 2D Circle
category: 2D Modules
categoryindex: 3
index: 4
---

# Circle 2D

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

Circle2D.atOrigin (Length.meters 2.)
(*** include-it ***)

Circle2D.atPoint (Point2D.meters 5. 2.) (Length.meters 4.)
(*** include-it ***)

Circle2D.withRadius (Length.meters 4.) (Point2D.meters 5. 2.)
(*** include-it ***)

Circle2D.throughPoints (Point2D.meters 3. 4.) (Point2D.meters -1. 2.) (Point2D.meters 4. 1.)
(*** include-it ***)

Circle2D.sweptAround Point2D.origin (Point2D.meters 3. 3.)
(*** include-it ***)


(**
#Accessors
*)

let circle: Circle2D<Meters, Cartesian> =
    Circle2D.atPoint (Point2D.meters 5. 2.) (Length.meters 4.)

(***)

Circle2D.centerPoint circle // or
circle.Center
(*** include-it ***)

Circle2D.radius circle // or
circle.Radius
(*** include-it ***)

Circle2D.diameter circle
(*** include-it ***)

Circle2D.area circle
(*** include-it ***)

Circle2D.circumference circle
(*** include-it ***)

Circle2D.toArc circle
(*** include-it ***)

Circle2D.boundingBox circle
(*** include-it ***)

(**
# Modifiers
*)

(**
Scale a circle about a given point by a given scale.
*)
Circle2D.scaleAbout Point2D.origin 1.5 circle
(*** include-it ***)

(**
Translate a circle by a given displacement.
*)
Circle2D.translateBy (Vector2D.meters 2. 0.) circle
(*** include-it ***)

(**
Translate a circle in a given direction by a given distance.
*)
Circle2D.translateIn Direction2D.y (Length.meters 1.0) circle
(*** include-it ***)

(**
Take a circle defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Circle2D.relativeTo frame circle
(*** include-it ***)

(**
Take a circle defined in local coordinates and return it expressed in global coordinates.
*)
let localCircle = Circle2D.relativeTo frame circle
Circle2D.placeIn frame localCircle
(*** include-it ***)

(**
# Queries
*)

(**
Check if a circle contains a given point.
*)
Circle2D.containsPoint (Point2D.meters 6. 2.) circle
(*** include-it ***)

(**
Check if a circle intersects a bounding box.
*)
let bbox = BoundingBox2D.from (Point2D.meters 0. 0.) (Point2D.meters 2. 2.)
Circle2D.intersectsBoundingBox bbox circle
(*** include-it ***)
