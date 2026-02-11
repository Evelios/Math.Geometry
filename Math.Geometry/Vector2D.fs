[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Math.Geometry.Vector2D

open FSharp.Json

open Math.Units

// ---- Builders ----

/// <summary> Construct a Vector2D object from the x and y lengths. </summary>
/// <category>Builders</category>
let xy (x: Quantity<'Units>) (y: Quantity<'Units>) : Vector2D<'Units, 'Coordinates> = { X = x; Y = y }

/// <summary>
/// Construct a vector given its local components within a particular frame
/// </summary>
/// <category>Builders</category>
let xyIn
    (frame: Frame2D<'Units, 'Coordinates, 'Defines>)
    (x: Quantity<'Units>)
    (y: Quantity<'Units>)
    : Vector2D<'Units, 'Coordinates> =
    let i = frame.XDirection
    let j = frame.YDirection
    xy (x * i.X + y * j.X) (x * i.Y + y * j.Y)

/// <summary>
/// Construct a vector from two points, pointing from the first point to the second point.
/// </summary>
/// <category>Builders</category>
let from (p1: Point2D<'Units, 'Coordinates>) (p2: Point2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    p2 - p1

/// <summary>
/// Construct a vector with the given length in the given direction.
/// </summary>
/// <category>Builders</category>
let withQuantity (a: Quantity<'Units>) (d: Direction2D<'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    Internal.Vector2D.withQuantity a d

/// Alias for `withQuantity`
/// <category>Builders</category>
let withLength (a: Quantity<'Units>) (d: Direction2D<'Coordinates>) : Vector2D<'Units, 'Coordinates> = withQuantity a d

/// Construct a vector using polar coordinates coordinates given a length and angle
/// <category>Builders</category>
let rTheta (r: Quantity<'Units>) (theta: Angle) : Vector2D<'Units, 'Coordinates> =
    xy (r * (Angle.cos theta)) (r * (Angle.sin theta))

/// Construct a vector given its local polar components within a particular
/// <category>Builders</category>
let rThetaIn
    (frame: Frame2D<'Units, 'Coordinates, 'Defines>)
    (r: Quantity<'Units>)
    (theta: Angle)
    : Vector2D<'Units, 'Coordinates> =
    let i = frame.XDirection
    let j = frame.YDirection
    let cosTheta = Angle.cos theta
    let sinTheta = Angle.sin theta
    xy (r * (cosTheta * i.X + sinTheta * j.X)) (r * (cosTheta * i.Y + sinTheta * j.Y))

/// Alias for `rTheta`
/// <category>Builders</category>
let polar (r: Quantity<'Units>) (theta: Angle) : Vector2D<'Units, 'Coordinates> = rTheta r theta

/// <category>Builders</category>
let zero<'Units, 'Coordinates> : Vector2D<'Units, 'Coordinates> =
    xy Quantity.zero Quantity.zero


// ---- Helper Builders ----

let private fromUnit conversion (x: float) (y: float) : Vector2D<Meters, 'Coordinates> =
    xy (conversion x) (conversion y)

/// <category>Builders</category>
let meters (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.meters x y
/// <category>Builders</category>
let pixels (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.cssPixels x y
/// <category>Builders</category>
let millimeters (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.millimeters x y
/// <category>Builders</category>
let centimeters (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.centimeters x y
/// <category>Builders</category>
let inches (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.inches x y
/// <category>Builders</category>
let feet (x: float) (y: float) : Vector2D<Meters, 'Coordinates> = fromUnit Length.feet x y



// ---- Accessors ----

/// <category>Accessor</category>
let magnitude (v: Vector2D<'Units, 'Coordinates>) : Quantity<'Units> =
    let largestComponent = max (Quantity.abs v.X) (Quantity.abs v.Y)

    if largestComponent = Quantity.zero then
        Quantity.zero

    else
        let scaledX = v.X / largestComponent
        let scaledY = v.Y / largestComponent

        let scaledQuantity = sqrt (scaledX * scaledX + scaledY * scaledY)

        scaledQuantity * largestComponent

/// Alias for `Vector2D.magnitude`
/// <category>Accessor</category>
let length = magnitude

/// Get the X and Y components of a vector as a tuple.
/// <category>Accessor</category>
let components (v: Vector2D<'Units, 'Coordinates>) : Quantity<'Units> * Quantity<'Units> = (v.X, v.Y)

/// <category>Accessor</category>
let x (v: Vector2D<'Units, 'Coordinates>) : Quantity<'Units> = v.X

/// <category>Accessor</category>
let y (v: Vector2D<'Units, 'Coordinates>) : Quantity<'Units> = v.Y

// ---- Operators ----

/// This function is designed to be used in piping operators.
/// <category>Operator</category>
let plus (rhs: Vector2D<'Units, 'Coordinates>) (lhs: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    lhs + rhs

/// Find the sum of a list of vectors.
/// <category>Operator</category>
let sum (vectors: Vector2D<'Units, 'Coordiante> list) : Vector2D<'Units, 'Coordiantes> =
    let rec sumHelp sumX sumY vectors =
        match vectors with
        | v: Vector2D<'Units, 'Coordinate> :: rest -> sumHelp (sumX + v.X) (sumY + v.Y) rest
        | [] -> xy sumX sumY

    sumHelp Quantity.zero Quantity.zero vectors



/// <category>Operator</category>
let minus (rhs: Vector2D<'Units, 'Coordinates>) (lhs: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    lhs - rhs

/// <category>Operator</category>
let times (rhs: float) (lhs: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = lhs * rhs


/// Alias for `Vector2D.times`
/// <category>Operator</category>
let scaleBy = times

/// <category>Operator</category>
let dividedBy (rhs: float) (lhs: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = lhs / rhs

/// <category>Operator</category>
let neg (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = -v

/// Shorthand for `Vector2D.scaleBy 2`.
/// <category>Operator</category>
let twice (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = scaleBy 2. v

/// Shorthand for `Vector2D.scaleBy 0.5`.
/// <category>Operator</category>
let half (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = scaleBy 0.5 v


// ---- Modifiers ----

/// Scale a vector to a given length.
/// <category>Modifier</category>
let scaleTo (scale: Quantity<'Units>) (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    let largestComponent = max (Quantity.abs v.X) (Quantity.abs v.Y)

    if largestComponent = Quantity.zero then
        zero

    else
        let scaledX = v.X / largestComponent
        let scaledY = v.Y / largestComponent

        let scaledQuantity = sqrt (scaledX * scaledX + scaledY * scaledY)

        xy (scale * scaledX / scaledQuantity) (scale * scaledY / scaledQuantity)


/// Rotate a vector counterclockwise by a given angle.
/// <category>Modifier</category>
let rotateBy (a: Angle) (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    xy (Angle.cos a * v.X - Angle.sin a * v.Y) (Angle.sin a * v.X + Angle.cos a * v.Y)

/// Rotate a vector counterClockwise by a given angle. Alias for `rotateBy`
/// <category>Modifier</category>
let rotateByCounterClockwise = rotateBy

/// Rotate a vector clockwise by a given angle.
/// <category>Modifier</category>
let rotateByClockwise (a: Angle) (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = rotateBy -a v

/// Rotate the given vector 90 degrees counterclockwise;
///     Vector2D.rotateCounterclockwise vector
/// is equivalent to
///     Vector2D.rotateBy (Angle.degrees 90) vector
/// but is more efficient.
/// <category>Modifier</category>
let rotateClockwise (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = xy -v.Y v.X

/// Rotate the given vector 90 degrees clockwise;
///     Vector2D.rotateClockwise vector
/// is equivalent to
///     Vector2D.rotateBy (Angle.degrees -90) vector
/// but is more efficient.
/// <category>Modifier</category>
let rotateCounterclockwise (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = xy v.Y -v.X

/// <category>Modifier</category>
/// <summary>
/// Construct a vector perpendicular to the given vector, by rotating the given
/// vector 90 degrees counterclockwise.
/// </summary> 
let perpendicularTo (givenVector: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    rotateCounterclockwise givenVector

/// <category>Modifier</category>
let normalize (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    scaleTo (Quantity.create<'Units> 1.) v

/// Round the vector to the internal precision.
/// (Default is 8 digits past the decimal point)
/// <category>Modifier</category>
let round (v: Vector2D<'Units, 'Coordinates>) =
    xy (Quantity.round v.X) (Quantity.round v.Y)

/// Round the vector to a specified number of digits
/// <category>Modifier</category>
let roundTo (digits: int) (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> =
    xy (Quantity.roundTo digits v.X) (Quantity.roundTo digits v.Y)

/// Find the component of a vector in an arbitrary direction, for example
/// <example>
///     Vector2D.componentIn (Direction2D.degrees 30) (Vector2D.meters 3. 4.)
/// </example>
/// <category>Modifier</category>
let componentIn (d: Direction2D<'Coordinates>) (v: Vector2D<'Units, 'Coordiantes>) : Quantity<'Units> =
    Internal.Vector2D.componentIn d v

/// Reverse the direction of a vector, negating its components.
/// <category>Modifier</category>
let reverse (v: Vector2D<'Units, 'Coordinates>) : Vector2D<'Units, 'Coordinates> = xy -v.X -v.Y

/// Alias for `Vector2D.reverse`
/// <category>Modifier</category>
let negate = reverse

/// Mirror a vector across a given axis.
/// The position of the axis doesn't matter, only its orientation:
/// <category>Modifier</category>
let mirrorAcross
    (axis: Axis2D<'AxisUnit, 'Coordinates>)
    (v: Vector2D<'Units, 'Coordinates>)
    : Vector2D<'Units, 'Coordinates> =
    let d = axis.Direction
    let a = 1. - 2. * d.Y * d.Y
    let b = 2. * d.X * d.Y
    let c = 1. - 2. * d.X * d.X
    xy (a * v.X + b * v.Y) (b * v.X + c * v.Y)

/// Find the projection of a vector in a particular direction. Conceptually,
/// this means splitting the original vector into a portion parallel to the given
/// direction and a portion perpendicular to it, then returning the parallel
/// portion.
/// <category>Modifier</category>
let projectionIn (d: Direction2D<'Coordinates>) (v: Vector2D<'Units, 'Coordiantes>) : Vector2D<'Units, 'Coordinates> =
    let projectedQuantity = v.X * d.X + v.Y * d.Y

    xy (projectedQuantity * d.X) (projectedQuantity * d.Y)

/// Project a vector onto an axis.
/// <category>Modifier</category>
let projectOnto
    (axis: Axis2D<'Units, 'Coordinates>)
    (v: Vector2D<'Units, 'Coordinates>)
    : Vector2D<'Units, 'Coordinates> =
    let d = axis.Direction

    let projectedQuantity = v.X * d.X + v.Y * d.Y

    xy (projectedQuantity * d.X) (projectedQuantity * d.Y)

/// Take a vector defined in global coordinates, and return it expressed in
/// local coordinates relative to a given reference frame.
/// <category>Modifier</category>
let relativeTo
    (frame: Frame2D<'Units, 'GlobalCoordinates, 'LocalCoordinates>)
    (v: Vector2D<'Units, 'GlobalCoordinates>)
    : Vector2D<'Units, 'LocalCoordinates> =
    let dx = frame.XDirection
    let dy = frame.YDirection
    xy (v.X * dx.X + v.Y * dx.Y) (v.X * dy.X + v.Y * dy.Y)

/// Take a vector defined in local coordinates relative to a given reference
/// frame, and return that vector expressed in global coordinates.
/// <category>Modifier</category>
let placeIn
    (frame: Frame2D<'Units, 'GlobalCoordinates, 'LocalCoordinates>)
    (v: Vector2D<'Units, 'LocalCoordinates>)
    : Vector2D<'Units, 'GlobalCoordinates> =
    let dx = frame.XDirection
    let dy = frame.YDirection
    xy (v.X * dx.X + v.Y * dy.X) (v.X * dx.Y + v.Y * dy.Y)


// ---- Queries ----

/// Get the distance between two vectors squared. This function can be used to
/// optimize some algorithms because you remove a square root call from the
/// calculation which can be an expensive operation.
/// <category>Query</category>
let distanceSquaredTo
    (p1: Vector2D<'Units, 'Coordinates>)
    (p2: Vector2D<'Units, 'Coordinates>)
    : Quantity<'Units Squared> =
    let dx = (p1.X - p2.X)
    let dy = (p1.Y - p2.Y)
    dx * dx + dy * dy

/// <category>Query</category>
let distanceTo p1 p2 : Quantity<'Units> =
    distanceSquaredTo p1 p2 |> Quantity.sqrt

/// <summary>
/// Get the vector that is the average of two vectors.
/// </summary>
/// <category>Query</category>
let midVector
    (p1: Vector2D<'Units, 'Coordinates>)
    (p2: Vector2D<'Units, 'Coordinates>)
    : Vector2D<'Units, 'Coordinates> =
    xy ((p1.X + p2.X) / 2.) ((p1.Y + p2.Y) / 2.)

/// <category>Query</category>
let dot (lhs: Vector2D<'Units, 'Coordinates>) (rhs: Vector2D<'Units, 'Coordinates>) : Quantity<'Units Squared> =
    ((lhs.X * rhs.X) + (lhs.Y * rhs.Y))

/// <category>Query</category>
let cross (lhs: Vector2D<'Units, 'Coordinates>) (rhs: Vector2D<'Units, 'Coordinates>) : Quantity<'Units Squared> =
    (lhs.X * rhs.Y) - (lhs.Y * rhs.X)

/// <summary>Get the direction the a vector is facing.</summary>
/// <category>Query</category>
let direction (v: Vector2D<'Units, 'Coordinates>) : Direction2D<'Coordinates> option = Direction2D.xyQuantity v.X v.Y

/// <summary>
/// Compare two vectors within a tolerance. Returns true if the difference
/// between the two given vectors has magnitude less than the given tolerance.
/// </summary>
/// <category>Query</category>
let equalWithin
    (tolerance: Quantity<'Units>)
    (lhs: Vector2D<'Units, 'Coordinates>)
    (rhs: Vector2D<'Units, 'Coordinates>)
    : bool =
    if tolerance > Quantity.zero then
        let nx = (rhs.X - lhs.X) / tolerance
        let ny = (rhs.Y - lhs.Y) / tolerance
        nx * nx + ny * ny <= 1.

    else if tolerance = Quantity.zero then
        lhs.X = rhs.X && lhs.Y = rhs.Y

    else
        false

// ---- Json ----

/// <category>Json</category>
let fromList (list: float list) : Vector2D<'Units, 'Coordinates> option =
    match list with
    | [ x; y ] -> xy (Quantity<'Units>.create x) (Quantity<'Units>.create y) |> Some

    | _ -> None

/// <category>Json</category>
let toList (vector: Vector2D<'Units, 'Coordinates>) : float list = [ vector.X.Value; vector.Y.Value ]


// ---- Json transformations ----

type Transform() =
    interface ITypeTransform with
        member this.targetType() = (fun _ -> typeof<float32 list>) ()

        member this.toTargetType value =
            toList (value :?> Vector2D<obj, obj>) :> obj

        member this.fromTargetType value =
            value :?> float list
            |> fromList
            |> Option.defaultValue (xy Quantity.zero Quantity.zero)
            :> obj

type ListTransform() =
    interface ITypeTransform with
        member this.targetType() = (fun _ -> typeof<float list list>) ()

        member this.toTargetType value =
            value :?> Vector2D<obj, obj> list |> List.map toList :> obj

        member this.fromTargetType value =
            value :?> float list list
            |> List.map (fromList >> Option.defaultValue (xy Quantity.zero Quantity.zero))
            :> obj
