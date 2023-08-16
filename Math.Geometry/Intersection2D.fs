﻿module Math.Geometry.Intersection2D

open FSharp.Extensions

/// Try to find the intersection between a line segment and a line. If the lines are parallel (even if they are
/// overlapping) then no intersection is returned.
let lineSegmentAndLine
    (first: LineSegment2D<'Units, 'Coordinates>)
    (second: Line2D<'Units, 'Coordinates>)
    : Point2D<'Units, 'Coordinates> option =

    let areParallel =
        match LineSegment2D.direction first, Line2D.direction second with
        | Some d1, Some d2 -> d1 = d2 || Direction2D.reverse d1 = d2
        | _ -> false

    if areParallel then
        None
    else
        // http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        let p = first.Start
        let q = second.Start
        let r = first.Finish - first.Start
        let s = second.Finish - second.Start

        let t = Vector2D.cross (q - p) s / Vector2D.cross r s

        if (0.0 <= t && t <= 1.0) then p + (t * r) |> Some else None

let lineAndLineSegment line segment = lineSegmentAndLine segment line

/// Get all the intersection points between a bounding box and a line
let boundingBoxAndLine
    (bbox: BoundingBox2D<'Units, 'Coordinates>)
    (line: Line2D<'Units, 'Coordinates>)
    : Point2D<'Units, 'Coordinates> list =
    BoundingBox2D.lineSegments bbox
    |> List.filterMap (lineAndLineSegment line)
    |> List.distinct
