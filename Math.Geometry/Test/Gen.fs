namespace Math.Geometry.Test

open Math.Units.Test

type 'a Positive = Positive of 'a

module internal Tuple2 =
    let pair x y = x, y
    let map f (x, y) = f x y


module Gen =
    open System
    open FsCheck

    open Math.Units
    open Math.Geometry
    open FSharp.Extensions

    let map7 fn a b c d e f g =
        Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.map fn a) b) c) d) e) f) g

    /// Generates a random number from [0.0, 1.0]
    let rand =
        Gen.choose (0, Int32.MaxValue)
        |> Gen.map (fun x -> float x / (float Int32.MaxValue))

    let intBetween (low: int) (high: int) : Gen<int> = Gen.choose (low, high)

    let floatBetween (low: float) (high: float) : Gen<float> =
        Gen.map (fun scale -> (low + (high - low)) * scale) rand

    let float: Gen<float> = Arb.generate<NormalFloat> |> Gen.map float

    let positiveFloat: Gen<float> = Gen.map abs float

    let private epsilonLength () = Quantity.create Float.Epsilon

    let angle: Gen<Angle> = Gen.map Angle.radians float

    let length: Gen<Length> = Gen.map Length.meters float

    let positiveLength: Gen<Length Positive> =
        Gen.map (Length.meters >> Positive) positiveFloat

    let lengthBetween (a: Quantity<'Units>) (b: Quantity<'Units>) : Gen<Quantity<'Units>> =
        Gen.map Quantity.create<'Units> (floatBetween a.Value b.Value)

    let direction2D<'Coordinates> : Gen<Direction2D<'Coordinates>> =
        Gen.two float
        |> Gen.where (fun (x, y) -> x <> 0. || y <> 0.)
        |> Gen.map (fun (x, y) ->
            let magnitude = sqrt ((x * x) + (y * y))
            { X = x / magnitude; Y = y / magnitude })

    let vector2D<'Units, 'Coordinates> : Gen<Vector2D<'Units, 'Coordinates>> =
        Gen.map2 Vector2D.xy Gen.quantity Gen.quantity

    let vector2DWithinRadius (radius: Quantity<'Units>) : Gen<Vector2D<'Units, 'Coordinates>> =
        Gen.map2 Vector2D.polar (lengthBetween Quantity.zero radius) angle

    let twoCloseVector2D<'Units, 'Coordinates> : Gen<Vector2D<'Units, 'Coordinates> * Vector2D<'Units, 'Coordinates>> =
        Gen.map2 (fun first offset -> (first, first + offset)) vector2D (vector2DWithinRadius (epsilonLength ()))

    let point2D<'Units, 'Coordinates> : Gen<Point2D<'Units, 'Coordinates>> =
        Gen.map2 Point2D.xy Gen.quantity Gen.quantity

    let point2DWithinOffset
        (radius: Quantity<'Units>)
        (point: Point2D<'Units, 'Coordinates>)
        : Gen<Point2D<'Units, 'Coordinates>> =

        Gen.map (fun offset -> point + offset) (vector2DWithinRadius radius)

    /// Generate two points that are within Epsilon of each other
    let twoClosePoint2D<'Units, 'Coordinates> : Gen<Point2D<'Units, 'Coordinates> * Point2D<'Units, 'Coordinates>> =
        Gen.map2 (fun first offset -> (first, first + offset)) point2D (vector2DWithinRadius (epsilonLength ()))

    let axis2D<'Units, 'Coordinates> : Gen<Axis2D<'Units, 'Coordinates>> =
        Gen.map2 Axis2D.through point2D direction2D

    let frame2D<'Units, 'Coordiantes> : Gen<Frame2D<'Units, 'Coordinates, TestDefines>> =
        Gen.map2 Frame2D.withAngle angle point2D

    let line2D<'Units, 'Coordianates> : Gen<Line2D<'Units, 'Coordinates>> =
        Gen.map2 Tuple2.pair point2D point2D
        |> Gen.filter (fun (p1, p2) -> p1 <> p2)
        |> Gen.map (Tuple2.map Line2D.through)

    let lineSegment2D<'Units, 'Coordianates> : Gen<LineSegment2D<'Units, 'Coordinates>> =
        Gen.map2 Tuple2.pair point2D point2D
        |> Gen.filter (fun (p1, p2) -> p1 <> p2)
        |> Gen.map (Tuple2.map LineSegment2D.from)

    let boundingBox2D<'Units, 'Coordinates> : Gen<BoundingBox2D<Meters, 'Coordinates>> =
        Gen.map2 BoundingBox2D.from point2D point2D

    let point2DInBoundingBox2D<'Units, 'Coordinates>
        (bbox: BoundingBox2D<'Units, 'Coordinates>)
        : Gen<Point2D<'Units, 'Coordinates>> =
        Gen.map2 Point2D.xy (lengthBetween bbox.MinX bbox.MaxX) (lengthBetween bbox.MinY bbox.MaxY)

    let lineSegment2DInBoundingBox2D<'Units, 'Coordinates> (bbox: BoundingBox2D<'Units, 'Coordinates>) =
        Gen.two (point2DInBoundingBox2D bbox)
        |> Gen.where (fun (a, b) -> a <> b)
        |> Gen.map (Tuple2.map LineSegment2D.from)

    let sweptAngle: Gen<SweptAngle> =
        Gen.oneof
            [ Gen.constant SweptAngle.smallPositive
              Gen.constant SweptAngle.smallNegative
              Gen.constant SweptAngle.largePositive
              Gen.constant SweptAngle.largeNegative ]

    let arc2D<'Units, 'Coordinates> : Gen<Arc2D<'Units, 'Coordinates>> =
        Gen.map3 Arc2D.from point2D point2D angle

    let polygon2D<'Units, 'Coordinates> : Gen<Polygon2D<'Units, 'Coordinates>> =
        let boundingBox =
            { MinX = Quantity.create -10.
              MaxX = Quantity.create 10.
              MinY = Quantity.create -10.
              MaxY = Quantity.create 10. }

        let genPoint2D: Gen<Point2D<'Units, 'Coordinates>> =
            point2DInBoundingBox2D boundingBox

        let genPolygonPointList =
            Gen.map3
                (fun first second rest -> first :: second :: rest)
                genPoint2D
                genPoint2D
                (Gen.nonEmptyListOf genPoint2D)

        let genPointListList = Gen.listOf genPolygonPointList

        Gen.map2 Polygon2D.withHoles genPointListList genPolygonPointList

    type ArbGeometry =
        static member Float() = Arb.fromGen float
        static member Angle() = Arb.fromGen angle
        static member SweptAngle() = Arb.fromGen sweptAngle
        static member Arc2D() = Arb.fromGen arc2D
        static member Length() = Arb.fromGen length
        static member PositiveLength() = Arb.fromGen positiveLength
        static member Direction2D() = Arb.fromGen direction2D
        static member Vector2D() = Arb.fromGen vector2D
        static member Point2D() = Arb.fromGen point2D
        static member Axis2D() = Arb.fromGen axis2D
        static member Line2D() = Arb.fromGen line2D
        static member LineSegment2D() = Arb.fromGen lineSegment2D
        static member BoundingBox2D() = Arb.fromGen boundingBox2D
        static member Frame2D() = Arb.fromGen frame2D
        static member Polygon2D() = Arb.fromGen polygon2D

        static member Register() = Arb.register<ArbGeometry> () |> ignore
