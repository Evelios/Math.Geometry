module Math.Geometry.Internal.Vector2D

open Math.Geometry

open Math.Units

let componentIn (d: Direction2D<'Coordinates>) (v: Vector2D<'Units, 'Coordiantes>) : Quantity<'Units> =
    (v.X * d.X + v.Y * d.Y)

let withQuantity (a: Quantity<'Units>) (d: Direction2D<'Coordinates>) : Vector2D<'Units, 'Coordiantes> =
    { X = (a * d.X); Y = (a * d.Y) }
