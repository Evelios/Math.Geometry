(**

---
title: 2D Bounding Box
category: 2D Modules
categoryindex: 3
index: 3
---

# Bounding Box 2D

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

BoundingBox2D.singleton (Point2D.meters 3. 5.)
(*** include-it ***)

BoundingBox2D.from (Point2D.meters 1. -3.) (Point2D.meters -5. 2.)
(*** include-it ***)

BoundingBox2D.fromExtrema
    { MinX = Length.meters 2.
      MaxX = Length.meters 4.
      MinY = Length.meters -1.
      MaxY = Length.meters 1. }
(*** include-it ***)

BoundingBox2D.withDimensions (Length.meters 7., Length.meters 2.) (Point2D.meters -3. 5.)
(*** include-it ***)


(**
# Accessors
*)

let boundingBox: BoundingBox2D<Meters, Cartesian> =
    BoundingBox2D.fromExtrema
        { MinX = Length.meters 2.
          MaxX = Length.meters 4.
          MinY = Length.meters -1.
          MaxY = Length.meters 1. }
(***)

boundingBox.MinX = BoundingBox2D.minX boundingBox
boundingBox.MaxX = BoundingBox2D.maxX boundingBox
boundingBox.MinY = BoundingBox2D.minY boundingBox
boundingBox.MaxY = BoundingBox2D.maxY boundingBox
(***)

BoundingBox2D.corners boundingBox
(*** include-it ***)

BoundingBox2D.width boundingBox
(*** include-it ***)

BoundingBox2D.height boundingBox
(*** include-it ***)

BoundingBox2D.dimensions boundingBox
(*** include-it ***)

BoundingBox2D.midX boundingBox
(*** include-it ***)

BoundingBox2D.midY boundingBox
(*** include-it ***)

BoundingBox2D.centerPoint boundingBox
(*** include-it ***)


(**
# Modifiers
*)

(**
Get a bounding box that contains the new point. If the box does not contain the new point, the box will grow
to fit the new point.
*)
BoundingBox2D.containingPoint (Point2D.meters 10. 10.) boundingBox
(*** include-it ***)

(**
Get a bounding box that contains a list of points.
*)
BoundingBox2D.containingPoints [ Point2D.meters 10. 10.; Point2D.meters -10. -10. ] boundingBox
(*** include-it ***)

(**
Scale a bounding box about a given point by a given scale.
*)
BoundingBox2D.scaleAbout Point2D.origin 2. boundingBox
(*** include-it ***)

(**
Translate a bounding box by a given displacement.
*)
BoundingBox2D.translateBy (Vector2D.meters 1. 1.) boundingBox
(*** include-it ***)

(**
Translate a bounding box in a given direction by a given distance.
*)
BoundingBox2D.translateIn Direction2D.x (Length.meters 2.) boundingBox
(*** include-it ***)

(**
Offsets boundingBox irrespective of the resulting bounding box is valid or not.
*)
BoundingBox2D.unsafeOffsetBy (Length.meters 1.) boundingBox
(*** include-it ***)

(**
Expand or shrink the given bounding box in all the directions by the given
distance. Returns `None` if the offset is negative and large enough to cause the
bounding box to vanish.
*)
BoundingBox2D.offsetBy (Length.meters 1.) boundingBox
(*** include-it ***)

(**
Expand the given bounding box in all directions by the given offset.
*)
BoundingBox2D.expandBy (Length.meters 1.) boundingBox
(*** include-it ***)

(**
# Queries
*)

(**
Get the four line segments surrounding the bounding box.
*)
BoundingBox2D.lineSegments boundingBox
(*** include-it ***)

(**
Find the bounding box containing one or more input points.
*)
BoundingBox2D.hull (Point2D.meters 0. 0.) [ Point2D.meters 5. 5. ]
(*** include-it ***)

(**
Like `hull`, but lets you work on any kind of item as long as a point can be extracted from it.
*)
BoundingBox2D.hullOf (fun (p: Point2D<Meters, Cartesian>) -> p) (Point2D.meters 0. 0.) [ Point2D.meters 5. 5. ]
(*** include-it ***)

(**
Build a bounding box that contains all three of the given points.
*)
BoundingBox2D.hull3 (Point2D.meters 0. 0.) (Point2D.meters 5. 0.) (Point2D.meters 0. 5.)
(*** include-it ***)

(**
Construct a bounding box containing all N points in the given list.
*)
BoundingBox2D.hullN [ Point2D.meters 0. 0.; Point2D.meters 5. 5. ]
(*** include-it ***)

(**
Combination of `hullOf` and `hullN`.
*)
BoundingBox2D.hullOfN (fun (p: Point2D<Meters, Cartesian>) -> p) [ Point2D.meters 0. 0.; Point2D.meters 5. 5. ]
(*** include-it ***)

(**
Find the bounding box containing one or more input boxes.
*)
BoundingBox2D.aggregate boundingBox [ boundingBox ]
(*** include-it ***)

(**
Like `aggregate`, but lets you work on any kind of item as long as a bounding box can be extracted from it.
*)
BoundingBox2D.aggregateOf (fun b -> b) boundingBox [ boundingBox ]
(*** include-it ***)

(**
Build a bounding box that contains all three of the given bounding boxes.
*)
BoundingBox2D.aggregate3 boundingBox boundingBox boundingBox
(*** include-it ***)

(**
Test to see if the target bounding box is contained within the bounding box.
*)
BoundingBox2D.isContainedIn boundingBox boundingBox
(*** include-it ***)

(**
Test to see if the target point is contained within the bounding box.
*)
BoundingBox2D.contains (Point2D.meters 3. 0.) boundingBox
(*** include-it ***)

(**
Check if two bounding boxes intersect.
*)
let otherBox = BoundingBox2D.from (Point2D.meters 0. 0.) (Point2D.meters 5. 5.)
BoundingBox2D.intersects otherBox boundingBox
(*** include-it ***)

(**
Attempt to build a bounding box that contains all points common to both given bounding boxes.
*)
BoundingBox2D.intersection otherBox boundingBox
(*** include-it ***)

(**
Check two boxes overlap by at least the given amount.
*)
BoundingBox2D.overlappingByAtLeast (Length.meters 0.1) otherBox boundingBox
(*** include-it ***)

(**
Check if two boxes are separated by at least the given amount.
*)
BoundingBox2D.separatedByAtLeast (Length.meters 10.0) otherBox boundingBox
(*** include-it ***)

(**
Create a bounding box that contains both bounding boxes.
*)
BoundingBox2D.union otherBox boundingBox
(*** include-it ***)
