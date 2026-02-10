(**

---
title: 2D Line Segment
category: 2D Modules
categoryindex: 3
index: 8
---

# Line Segment 2D

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

LineSegment2D.from (Point2D.meters 3. 4.) (Point2D.meters -8. 2.)
(***)

LineSegment2D.fromEndpoints (Point2D.meters 3. 4., Point2D.meters -8. 2.)
(***)

LineSegment2D.safeFrom (Point2D.meters 3. 4.) (Point2D.meters -8. 2.)
(*** hide ***)
LineSegment2D.safeFrom (Point2D.meters 3. 4.) (Point2D.meters -8. 2.)
(*** include-it ***)

LineSegment2D.safeFrom (Point2D.meters 2. 1.) (Point2D.meters 2. 1.)
(*** hide ***)
LineSegment2D.safeFrom (Point2D.meters 2. 1.) (Point2D.meters 2. 1.)
(*** include-it ***)

LineSegment2D.fromPointAndVector Point2D.origin (Vector2D.meters 7. 8.)
(***)

LineSegment2D.along Axis2D.x (Length.meters 3.) (Length.meters 7.)
(***)

(**
# Accessors
*)

let segment: LineSegment2D<Meters, Cartesian> =
    LineSegment2D.from (Point2D.meters 3. 4.) (Point2D.meters -8. 2.)
(***)

LineSegment2D.start segment // or
segment.Start
(*** include-it ***)

LineSegment2D.finish segment // or
segment.Finish
(*** include-it ***)

LineSegment2D.endpoints segment
(*** include-it ***)

LineSegment2D.vector segment
(*** include-it ***)

LineSegment2D.direction segment
(*** include-it ***)

LineSegment2D.length segment
(*** include-it ***)

LineSegment2D.axis segment
(*** include-it ***)

LineSegment2D.perpendicularDirection segment
(*** include-it ***)

LineSegment2D.midpoint segment
(*** include-it ***)

(**
# Modifiers
*)

(**
Transform the start and end points of a line segment by a given function.
*)
segment |> LineSegment2D.mapEndpoints (Point2D.translate (Vector2D.meters 4. 2.))
(*** include-it ***)

(**
Reverse the direction of a line segment.
*)
LineSegment2D.reverse segment
(*** include-it ***)

(**
Scale a line segment about the given center point by the given scale.
*)
segment |> LineSegment2D.scaleAbout Point2D.origin 2.0
(*** include-it ***)

(**
Rotate a line segment counterclockwise around a given center point by a given angle.
*)
segment |> LineSegment2D.rotateAround Point2D.origin (Angle.degrees 90.0)
(*** include-it ***)

(**
Translate a line segment by a given displacement.
*)
segment |> LineSegment2D.translateBy (Vector2D.meters 1. 1.)
(*** include-it ***)

(**
Translate a line segment in a given direction by a given distance.
*)
segment |> LineSegment2D.translateIn Direction2D.y (Length.meters 2.0)
(*** include-it ***)

(**
Mirror a line segment across an axis.
*)
segment |> LineSegment2D.mirrorAcross Axis2D.x
(*** include-it ***)

(**
Project a line segment onto an axis.
*)
segment |> LineSegment2D.projectOnto Axis2D.x
(*** include-it ***)

(**
Round the endpoints of a line segment.
*)
LineSegment2D.round segment
(*** include-it ***)

(**
# Queries
*)

(**
Interpolate along a line segment based on a parameter that ranges from 0 to 1.
*)
LineSegment2D.interpolate segment 0.5
(*** include-it ***)

(**
Check if two line segments are parallel.
*)
let otherSegment = LineSegment2D.from (Point2D.meters 0. 1.) (Point2D.meters 2. 1.)
LineSegment2D.areParallel segment otherSegment
(*** include-it ***)

(**
Check if a point lies on a line segment.
*)
LineSegment2D.isPointOnSegment (Point2D.meters 0. 0.) segment
(*** include-it ***)

(**
Find the distance from a point to a line segment.
*)
LineSegment2D.distanceToPoint (Point2D.meters 5. 5.) segment
(*** include-it ***)

(**
Get the point on a line segment that is closest to the input point.
*)
LineSegment2D.pointClosestTo (Point2D.meters 5. 5.) segment
(*** include-it ***)

(**
Try to find the intersection point between two line segments.
*)
let crossingSegment = LineSegment2D.from (Point2D.meters 0. 5.) (Point2D.meters 5. 0.)
LineSegment2D.intersectionPoint segment crossingSegment
(*** include-it ***)

(**
Attempt to find the unique intersection point of a line segment with an axis.
*)
LineSegment2D.intersectionWithAxis Axis2D.x segment
(*** include-it ***)

(**
Take a line segment defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
LineSegment2D.relativeTo frame segment
(*** include-it ***)

(**
Take a line segment defined in local coordinates and return it expressed in global coordinates.
*)
let localSegment = LineSegment2D.relativeTo frame segment
LineSegment2D.placeIn frame localSegment
(*** include-it ***)

(**
Get the minimal bounding box containing a given line segment.
*)
LineSegment2D.boundingBox segment
(*** include-it ***)
