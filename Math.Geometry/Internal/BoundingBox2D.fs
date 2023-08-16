module Math.Geometry.Internal.BoundingBox2D

open Math.Geometry

open Math.Units

let from
    (firstPoint: Point2D<'Units, 'Coordinates>)
    (secondPoint: Point2D<'Units, 'Coordinates>)
    : BoundingBox2D<'Units, 'Coordinates> =
    let x1 = firstPoint.X
    let y1 = firstPoint.Y
    let x2 = secondPoint.X
    let y2 = secondPoint.Y

    { MinX = Quantity.min x1 x2
      MaxX = Quantity.max x1 x2
      MinY = Quantity.min y1 y2
      MaxY = Quantity.max y1 y2 }
