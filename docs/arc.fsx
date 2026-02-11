(**

---
title: 2D Arc
category: 2D Modules
categoryindex: 3
index: 1
---

# Arc 2D

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

Arc2D.from (Point2D.meters 1. 3.) (Point2D.meters 5. 7.) (Angle.degrees 30.)

(***)

Arc2D.withCenterPoint (Point2D.meters 5. 5.) (Length.meters 3.) (Angle.degrees 30.) (Angle.degrees 120.)

(***)

Arc2D.sweptAround (Point2D.meters 0. 0.) (Angle.degrees 180.) (Point2D.meters 5. 0.)

(***)

Arc2D.throughPoints (Point2D.meters 5. 0.) (Point2D.meters 3. 0.) (Point2D.meters -5. 0.)

(***)

Arc2D.withRadius (Length.meters 5.) SweptAngle.largePositive (Point2D.meters 5. 0.) (Point2D.meters -5. 0.)

(***)

Arc2D.withSweptAngle (Point2D.meters 0. 0.) (Length.meters 5.) Angle.zero (Angle.degrees 180.)

(***)

(**
# Accessors

For showing what the different accessors will return, the following arc will be used.
*)

let arc: Arc2D<Meters, Cartesian> =
    Arc2D.from (Point2D.meters 1. 3.) (Point2D.meters 5. 7.) (Angle.degrees 30.)

(***)

Arc2D.centerPoint arc
(*** include-it ***)

Arc2D.radius arc
(*** include-it ***)

Arc2D.sweptAngle arc
(*** include-it ***)

Arc2D.pointOn arc 0.3
(*** include-it ***)

Arc2D.startPoint arc
(*** include-it ***)

Arc2D.midpoint arc
(*** include-it ***)

Arc2D.endPoint arc
(*** include-it ***)


Arc2D.boundingBox arc
(*** include-it ***)

Arc2D.firstDerivative arc 0.8
(*** include-it ***)


(**
# Modifiers
*)

(**
Reverse the direction of an arc.
*)
Arc2D.reverse arc
(*** include-it ***)

(**
Scale an arc about a given point by a given scale.
*)
Arc2D.scaleAbout Point2D.origin 1.5 arc
(*** include-it ***)

(**
Rotate an arc around a given point by a given angle.
*)
Arc2D.rotateAround Point2D.origin (Angle.degrees 45.0) arc
(*** include-it ***)

(**
Translate an arc by a given displacement.
*)
Arc2D.translateBy (Vector2D.meters 2. 0.) arc
(*** include-it ***)

(**
Translate an arc in a given direction by a given distance.
*)
Arc2D.translateIn Direction2D.y (Length.meters 1.0) arc
(*** include-it ***)

(**
Mirror an arc across a given axis.
*)
Arc2D.mirrorAcross Axis2D.x arc
(*** include-it ***)

(**
Take an arc defined in global coordinates and return it expressed in local coordinates relative to a given frame.
*)
let frame : Frame2D<Meters, Cartesian, unit> = Frame2D.atPoint (Point2D.meters 1. 1.)
Arc2D.relativeTo frame arc
(*** include-it ***)

(**
Take an arc defined in local coordinates and return it expressed in global coordinates.
*)
let localArc = Arc2D.relativeTo frame arc
Arc2D.placeIn frame localArc
(*** include-it ***)

(**
# Non-Degenerative
*)

(*** hide ***)
let nondegenerate = arc

(***)

Arc2D.nondegenerate arc
(*** include-it ***)

Arc2D.fromNondegenerate nondegenerate

(***)

Arc2D.tangentDirection nondegenerate 0.2
(*** include-it ***)

Arc2D.sample nondegenerate 0.4
(*** include-it ***)
