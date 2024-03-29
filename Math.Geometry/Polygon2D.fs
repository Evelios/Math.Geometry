[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Math.Geometry.Polygon2D

open Math.Units


// ---- Builders ----

let private counterclockwiseArea (vertices_: Point2D<'Units, 'Coordinates> list) : Quantity<'Units Squared> =
    match vertices_ with
    | [] -> Quantity.zero

    | [ _ ] -> Quantity.zero

    | [ _; _ ] -> Quantity.zero

    | first :: rest ->
        let segmentArea start finish =
            Triangle2D.counterclockwiseArea (Triangle2D.from first start finish)

        let segmentAreas =
            List.pairwise rest |> List.map (fun (start, finish) -> segmentArea start finish)

        Quantity.sum segmentAreas

let private makeOuterLoop (vertices_: Point2D<'Units, 'Coordinates> list) : Point2D<'Units, 'Coordinates> list =
    if counterclockwiseArea vertices_ >= Quantity.zero then
        vertices_
    else
        List.rev vertices_


let private makeInnerLoop (vertices_: Point2D<'Units, 'Coordinates> list) : Point2D<'Units, 'Coordinates> list =
    if counterclockwiseArea vertices_ <= Quantity.zero then
        vertices_
    else
        List.rev vertices_

/// Construct a polygon with holes from one outer loop and a list of inner
/// loops. The loops must not touch or intersect each other.
///
///     let outerLoop =
///         [ Point2D.meters 0. 0.
///           Point2D.meters 3. 0.
///           Point2D.meters 3. 3.
///           Point2D.meters 0. 3.
///         ]
///     let innerLoop =
///         [ Point2D.meters 1. 1.
///           Point2D.meters 1. 2.
///           Point2D.meters 2. 2.
///           Point2D.meters 2. 1.
///         ]
///     let squareWithHole =
///         Polygon2D.withHoles [ innerLoop ] outerLoop
///
/// As with `Polygon2D.singleLoop`, the last vertex of each loop is considered to be
/// connected back to the first. Vertices of the outer loop should ideally be
/// provided in counterclockwise order and vertices of the inner loops should
/// ideally be provided in clockwise order.
let withHoles
    (givenInnerLoops: Point2D<'Units, 'Coordinates> list list)
    (givenOuterLoop: Point2D<'Units, 'Coordinates> list)
    : Polygon2D<'Units, 'Coordinates> =

    { OuterLoop = makeOuterLoop givenOuterLoop
      InnerLoops = List.map makeInnerLoop givenInnerLoops }

/// Construct a polygon without holes from a list of its vertices:
///
///     let rectangle =
///         Polygon2D.singleLoop
///             [ Point2D.meters 1. 1.
///               Point2D.meters 3. 1.
///               Point2D.meters 3. 2.
///               Point2D.meters 1. 2.
///             ]
///
/// The last vertex is implicitly considered to be connected back to the first
/// vertex (you do not have to close the polygon explicitly). Vertices should
/// ideally be provided in counterclockwise order; if they are provided in clockwise
/// order they will be reversed.
let singleLoop givenOuterLoop = withHoles [] givenOuterLoop

let private counterclockwiseAround
    (origin: Point2D<'Units, 'Coordinates>)
    (a: Point2D<'Units, 'Coordinates>)
    (b: Point2D<'Units, 'Coordinates>)
    : bool =

    let crossProduct = Vector2D.from origin a |> Vector2D.cross (Vector2D.from origin b)

    crossProduct >= Quantity.zero

let private chain (acc: Point2D<'Units, 'Coordinates> list) : Point2D<'Units, 'Coordinates> list =

    let rec chainHelp
        (acc: Point2D<'Units, 'Coordinates> list)
        (list: Point2D<'Units, 'Coordinates> list)
        : Point2D<'Units, 'Coordinates> list =

        match (acc, list) with
        | r1 :: r2 :: rs, x :: xs ->
            if counterclockwiseAround r2 r1 x then
                chainHelp (r2 :: rs) (x :: xs)

            else
                chainHelp (x :: acc) xs

        | _, x :: xs -> chainHelp (x :: acc) xs

        | _, [] -> List.tail acc

    chainHelp [] acc

/// Build the [convex hull](https://en.wikipedia.org/wiki/Convex_hull) of a list
/// of points. This is an O(n log n) operation.
let convexHull (points: Point2D<'Units, 'Coordinates> list) : Polygon2D<'Units, 'Coordinates> =
    match points with
    | [] -> singleLoop []

    | _ ->
        // See http://www.algorithmist.com/index.php/Monotone_Chain_Convex_Hull
        // for a description of the algorithm.
        let sorted = List.sort points
        let lower = chain sorted
        let upper = chain (List.rev sorted)

        singleLoop (lower @ upper)


// ---- Accessors ----

/// Get the list of vertices defining the outer loop (border) of a polygon.
/// The vertices will be in counterclockwise order.
let outerLoop (polygon: Polygon2D<'Units, 'Coordinates>) : Point2D<'Units, 'Coordinates> list = polygon.OuterLoop

/// Get the holes (if any) of a polygon, each defined by a list of vertices.
/// Each list of vertices will be in clockwise order.
let innerLoops (polygon: Polygon2D<'Units, 'Coordinates>) : Point2D<'Units, 'Coordinates> list list = polygon.InnerLoops

/// Get all vertices of a polygon; this will include vertices from the outer
/// loop of the polygon and all inner loops. The order of the returned vertices is
/// undefined.
let vertices (polygon: Polygon2D<'Units, 'Coordinates>) : Point2D<'Units, 'Coordinates> list =
    List.concat (outerLoop polygon :: innerLoops polygon)


let loopEdges (vertices_: Point2D<'Units, 'Coordinates> list) : LineSegment2D<'Units, 'Coordinates> list =
    match vertices_ with
    | [] -> []
    | first :: rest as all -> List.map2 LineSegment2D.from all (rest @ [ first ])

/// Get all edges of a polygon. This will include both outer edges and inner
/// (hole) edges.
let edges (polygon: Polygon2D<'Units, 'Coordinates>) : LineSegment2D<'Units, 'Coordinates> list =
    let outerEdges = loopEdges (outerLoop polygon)
    let innerEdges = List.map loopEdges (innerLoops polygon)
    List.concat (outerEdges :: innerEdges)

/// Get the perimeter of a polygon (the sum of the lengths of its edges). This
/// includes the outer perimeter and the perimeter of any holes.
let perimeter (polygon: Polygon2D<'Units, 'Coordinates>) : Quantity<'Units> =
    edges polygon |> List.map LineSegment2D.length |> Quantity.sum

/// Get the area of a polygon. This value will never be negative.
let area (polygon: Polygon2D<'Units, 'Coordinates>) : Quantity<'Units Squared> =
    counterclockwiseArea (outerLoop polygon)
    + (Quantity.sum (List.map counterclockwiseArea (innerLoops polygon)))

let rec private centroidHelp
    (x0: Quantity<'Units>)
    (y0: Quantity<'Units>)
    (firstPoint: Point2D<'Units, 'Coordinates>)
    (currentLoop: Point2D<'Units, 'Coordinates> list)
    (remainingLoops: Point2D<'Units, 'Coordinates> list list)
    (xSum: Quantity<'Units>)
    (ySum: Quantity<'Units>)
    (areaSum: Quantity<'Units Squared>)
    : Point2D<'Units, 'Coordinates> option =

    match currentLoop with
    | [] ->
        match remainingLoops with
        | loop :: newRemainingLoops ->
            match loop with
            | first :: _ :: _ ->
                // enqueue a new loop
                centroidHelp x0 y0 first loop newRemainingLoops xSum ySum areaSum

            | _ ->
                // skip a loop with < 2 points
                centroidHelp x0 y0 firstPoint [] newRemainingLoops xSum ySum areaSum

        | [] ->
            if areaSum > Quantity.zero then
                Some
                    { X = xSum / ((areaSum * 3.).Value + x0.Value)
                      Y = ySum / ((areaSum * 3.).Value + y0.Value) }


            else
                None

    | point1 :: currentLoopRest ->
        match currentLoopRest with
        | point2 :: _ ->
            let p1 = point1
            let p2 = point2
            let p1x = p1.X - x0
            let p1y = p1.Y - y0
            let p2x = p2.X - x0
            let p2y = p2.Y - y0
            let a = p1x * p2y - p2x * p1y
            let newXSum = xSum + (p1x + p2x) * a.Value
            let newYSum = ySum + (p1y + p2y) * a.Value
            let newAreaSum = areaSum + a
            centroidHelp x0 y0 firstPoint currentLoopRest remainingLoops newXSum newYSum newAreaSum

        | [] ->
            let p1 = point1
            let p2 = firstPoint
            let p1x = p1.X - x0
            let p1y = p1.Y - y0
            let p2x = p2.X - x0
            let p2y = p2.Y - y0
            let a = p1x * p2y - p2x * p1y
            let newXSum = xSum + (p1x + p2x) * a.Value
            let newYSum = ySum + (p1y + p2y) * a.Value
            let newAreaSum = areaSum + a

            match remainingLoops with
            | loop :: newRemainingLoops ->
                match loop with
                | first :: _ :: _ ->
                    // enqueue a new loop
                    centroidHelp x0 y0 first loop newRemainingLoops newXSum newYSum newAreaSum

                | _ ->
                    // skip a loop with < 2 points
                    centroidHelp x0 y0 firstPoint [] newRemainingLoops newXSum newYSum newAreaSum

            | [] ->
                if newAreaSum > Quantity.zero then
                    Some(
                        { X = newXSum / (newAreaSum.Value * 3.) + x0
                          Y = newYSum / (newAreaSum.Value * 3.) + y0 }
                    )

                else
                    None

/// Get the centroid of a polygon. Returns `Nothing` if the polygon has no
/// vertices or zero area.
let centroid (polygon: Polygon2D<'Units, 'Coordinates>) : Point2D<'Units, 'Coordinates> option =
    match outerLoop polygon with
    | first :: _ :: _ ->
        let offset = first

        centroidHelp
            offset.X
            offset.Y
            first
            (outerLoop polygon)
            (innerLoops polygon)
            Quantity.zero
            Quantity.zero
            Quantity.zero

    | _ -> None

/// Get the minimal bounding box containing a given polygon. Returns `None`
/// if the polygon has no vertices.
let boundingBox (polygon: Polygon2D<'Units, 'Coordinates>) : BoundingBox2D<'Units, 'Coordinates> option =
    BoundingBox2D.hullN (outerLoop polygon)


// ---- Modifiers ----

let mapVertices
    (f: Point2D<'UnitA, 'CoordinatesA> -> Point2D<'UnitB, 'CoordinatesB>)
    (invert: bool)
    (polygon: Polygon2D<'UnitA, 'CoordinatesA>)
    : Polygon2D<'UnitB, 'CoordinatesB> =
    let mappedOuterLoop = List.map f (outerLoop polygon)

    let mappedInnerLoops = List.map (List.map f) (innerLoops polygon)

    if invert then
        { OuterLoop = List.rev mappedOuterLoop
          InnerLoops = List.map List.rev mappedInnerLoops }

    else
        { OuterLoop = mappedOuterLoop
          InnerLoops = mappedInnerLoops }

/// Scale a polygon about a given center point by a given scale. If the given
/// scale is negative, the order of the polygon's vertices will be reversed so that
/// the resulting polygon still has its outer vertices in counterclockwise order and
/// its inner vertices in clockwise order.
let scaleAbout
    (point: Point2D<'Units, 'Coordinates>)
    (scale: float)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =
    mapVertices (Point2D.scaleAbout point scale) (scale < 0.) polygon


/// Rotate a polygon around the given center point counterclockwise by the given
/// angle.
let rotateAround
    (point: Point2D<'Units, 'Coordinates>)
    (angle: Angle)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =
    mapVertices (Point2D.rotateAround point angle) false polygon


/// Translate a polygon by the given displacement.
let translateBy
    (vector: Vector2D<'Units, 'Coordinates>)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =
    mapVertices (Point2D.translateBy vector) false polygon


/// Translate a polygon in a given direction by a given distance.
let translateIn
    (direction: Direction2D<'Coordinates>)
    (distance: Quantity<'Units>)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =
    translateBy (Vector2D.withQuantity distance direction) polygon


/// Mirror a polygon across the given axis. The order of the polygon's vertices
/// will be reversed so that the resulting polygon still has its outer vertices in
/// counterclockwise order and its inner vertices in clockwise order.
let mirrorAcross
    (axis: Axis2D<'Units, 'Coordinates>)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =
    mapVertices (Point2D.mirrorAcross axis) true polygon


let translate
    (amount: Vector2D<'Units, 'Coordinates>)
    (polygon: Polygon2D<'Units, 'Coordinates>)
    : Polygon2D<'Units, 'Coordinates> =

    mapVertices (Point2D.translate amount) false polygon

/// Take a polygon defined in global coordinates, and return it expressed in
/// local coordinates relative to a given reference frame. If the given frame is
/// left-handed, the order of the polygon's vertices will be reversed so that the
/// resulting polygon still has its outer vertices in counterclockwise order and its
/// inner vertices in clockwise order.
let relativeTo
    (frame: Frame2D<'Units, 'GlobalCoordinates, 'LocalCoordinates>)
    (polygon: Polygon2D<'Units, 'GlobalCoordinates>)
    : Polygon2D<'Units, 'LocalCoordinates> =
    mapVertices (Point2D.relativeTo frame) (not (Frame2D.isRightHanded frame)) polygon

/// Take a polygon considered to be defined in local coordinates relative to a
/// given reference frame, and return that polygon expressed in global coordinates.
/// If the given frame is left-handed, the order of the polygon's vertices will be
/// reversed so that the resulting polygon still has its outer vertices in
/// counterclockwise order and its inner vertices in clockwise order.
let placeIn
    (frame: Frame2D<'Units, 'GlobalCoordinates, 'LocalCoordinates>)
    (polygon: Polygon2D<'Units, 'LocalCoordinates>)
    : Polygon2D<'Units, 'GlobalCoordinates> =

    mapVertices (Point2D.placeIn frame) false polygon


// ---- Queries ----

let rec private containsPointHelp
    (edgeList: LineSegment2D<'Units, 'Coordinates> list)
    (xp: Quantity<'Units>)
    (yp: Quantity<'Units>)
    (k: int)
    : bool =
    // Based on Hao, J.; Sun, J.; Chen, Y.; Cai, Q.; Tan, L. Optimal Reliable Point-in-Polygon Test and
    // Differential Coding Boolean Operations on Polygons. Symmetry 2018, 10, 477.
    // https://www.mdpi.com/2073-8994/10/10/477/pdf
    match edgeList with
    | [] -> not (k % 2 = 0)
    | edge :: rest ->
        let p0, p1 = LineSegment2D.endpoints edge
        let xi = p0.X
        let yi = p0.Y
        let xi1 = p1.X
        let yi1 = p1.Y
        let v1 = yi - yp
        let v2 = yi1 - yp

        if
            (v1 < Quantity.zero && v2 < Quantity.zero)
            || (v1 > Quantity.zero && v2 > Quantity.zero)
        then
            // case 11 or 26
            containsPointHelp rest xp yp k

        else
            let u1 = xi - xp
            let u2 = xi1 - xp

            if v2 > Quantity.zero && v1 <= Quantity.zero then
                let f = u1 * v2 - u2 * v1

                if f > Quantity.zero then
                    // case 3 or 9
                    containsPointHelp rest xp yp (k + 1)

                else if f = Quantity.zero then
                    // case 16 or 21
                    true

                else
                    // case 13 or 24
                    containsPointHelp rest xp yp k

            else if v1 > Quantity.zero && v2 <= Quantity.zero then
                let f = u1 * v2 - u2 * v1

                if f < Quantity.zero then
                    // case 4 or 10
                    containsPointHelp rest xp yp (k + 1)

                else if f = Quantity.zero then
                    // case 19 or 20
                    true

                else
                    // case 12 or 25
                    containsPointHelp rest xp yp k

            else if v2 = Quantity.zero && v1 < Quantity.zero then
                let f = u1 * v2 - u2 * v1

                if f = Quantity.zero then
                    // case 17
                    true

                else
                    // case 7 or 14
                    containsPointHelp rest xp yp k

            else if v1 = Quantity.zero && v2 < Quantity.zero then
                let f = u1 * v2 - u2 * v1

                if f = Quantity.zero then
                    // case 18
                    true

                else
                    // case 8 or 15
                    containsPointHelp rest xp yp k

            else if v1 = Quantity.zero && v2 = Quantity.zero then
                if u2 <= Quantity.zero && u1 >= Quantity.zero then
                    // case 1
                    true

                else if u1 <= Quantity.zero && u2 >= Quantity.zero then
                    // case 2
                    true

                else
                    //  case 5, 6, 22, 23
                    containsPointHelp rest xp yp k

            else
                containsPointHelp rest xp yp k

/// Check if a polygon contains a given point.
/// This is an O(n) operation. The polygon can have holes and does not need to be convex.
let contains (point: Point2D<'Units, 'Coordinates>) (polygon: Polygon2D<'Units, 'Coordinates>) : bool =
    containsPointHelp (edges polygon) point.X point.Y 0
