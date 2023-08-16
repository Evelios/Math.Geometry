module Math.Geometry.Boolean2D

let boundingBoxAndLine
    (bbox: BoundingBox2D<'Units, 'Coordinates>)
    (line: Line2D<'Units, 'Coordinates>)
    : LineSegment2D<'Units, 'Coordinates> option =
    match Intersection2D.boundingBoxAndLine bbox line with
    | [ first; second ] -> Some(LineSegment2D.from first second)
    | _ -> None
