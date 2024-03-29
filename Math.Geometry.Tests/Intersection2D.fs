module Math.Geometry.Tests.Intersection2D

open NUnit.Framework
open FsCheck

open Math.Geometry
open Math.Geometry.Test

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()

[<Test>]
let ``Line Segment And Line Intersection`` () =
    let segment = LineSegment2D.from (Point2D.meters 1. 4.) (Point2D.meters 4. 1.)

    let line = Line2D.through (Point2D.meters 1. 1.) (Point2D.meters 4. 4.)

    let expected = Some(Point2D.meters 2.5 2.5)

    let actual = Intersection2D.lineSegmentAndLine segment line

    Assert.AreEqual(expected, actual)

[<Test>]
let ``Line Segment And Line No Intersection`` () =
    let segment = LineSegment2D.from (Point2D.meters 1. 4.) (Point2D.meters 2. 3.)

    let line = Line2D.through (Point2D.meters 1. 1.) (Point2D.meters 4. 4.)

    let expected = None

    let actual = Intersection2D.lineSegmentAndLine segment line

    Assert.AreEqual(expected, actual)
