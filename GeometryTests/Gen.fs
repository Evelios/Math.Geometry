namespace GeometryTests

module Gen =
    open System
    open FsCheck

    open Geometry
    open FSharp.Extensions

    let map7 fn a b c d e f g =
        Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.apply (Gen.map fn a) b) c) d) e) f) g

    /// Generates a random number from [0.0, 1.0]
    let rand =
        Gen.choose (0, Int32.MaxValue)
        |> Gen.map (fun x -> float x / (float Int32.MaxValue))

    let floatBetween low high =
        Gen.map (fun scale -> (low + (high - low)) * scale) rand

    let float =
        Arb.generate<NormalFloat> |> Gen.map float

    let private epsilonLength<'Unit> () = Length<'Unit>.create Epsilon

    let angle = Gen.map Angle.radians float

    let length = Gen.map Length.meters float

    let lengthBetween (a: Length<'Unit>) (b: Length<'Unit>) : Gen<Length<'Unit>> =
        Gen.map Length.create<'Unit> (floatBetween (Length.unpack a) (Length.unpack b))

    let direction2D : Gen<Direction2D<TestSpace>> =
        Gen.two float
        |> Gen.where (fun (x, y) -> x <> 0. || y <> 0.)
        |> Gen.map
            (fun (x, y) ->
                let magnitude = sqrt ((x * x) + (y * y))
                { X = x / magnitude; Y = y / magnitude })

    let vector2D : Gen<Vector2D<Meters, TestSpace>> = Gen.map2 Vector2D.xy length length

    let vector2DWithinRadius (radius: Length<'Unit>) : Gen<Vector2D<'Unit, 'Coordinates>> =
        Gen.map2 Vector2D.polar (lengthBetween Length.zero radius) angle

    let twoCloseVector2D =
        Gen.map2 (fun first offset -> (first, first + offset)) vector2D (vector2DWithinRadius (epsilonLength ()))

    let point2D : Gen<Point2D<Meters, TestSpace>> = Gen.map2 Point2D.xy length length

    let point2DWithinOffset radius (point: Point2D<'Unit, 'Coordinates>) =
        Gen.map (fun offset -> point + offset) (vector2DWithinRadius radius)

    /// Generate two points that are within Epsilon of each other
    let twoClosePoint2D =
        Gen.map2 (fun first offset -> (first, first + offset)) point2D (vector2DWithinRadius (epsilonLength ()))

    let axis2D =
        Gen.map2 Axis2D.through point2D direction2D

    let frame2D : Gen<Frame2D<Meters, TestSpace, TestDefines>> = Gen.map2 Frame2D.withAngle angle point2D

    let line2D =
        Gen.map2 Tuple2.pair point2D point2D
        |> Gen.filter (fun (p1, p2) -> p1 <> p2)
        |> Gen.map (Tuple2.map Line2D.through)

    let lineSegment2D =
        Gen.map2 Tuple2.pair point2D point2D
        |> Gen.filter (fun (p1, p2) -> p1 <> p2)
        |> Gen.map (Tuple2.map LineSegment2D.from)

    let boundingBox2D : Gen<BoundingBox2D<Meters, TestSpace>> =
        Gen.map2 BoundingBox2D.from point2D point2D

    let point2DInBoundingBox2D (bbox: BoundingBox2D<'Unit, 'Coordinates>) =
        Gen.map2 Point2D.xy (lengthBetween bbox.MinX bbox.MaxX) (lengthBetween bbox.MinY bbox.MaxY)

    let lineSegment2DInBoundingBox2D (bbox: BoundingBox2D<'Unit, 'Coordinates>) =
        Gen.two (point2DInBoundingBox2D bbox)
        |> Gen.where (fun (a, b) -> a <> b)
        |> Gen.map (Tuple2.map LineSegment2D.from)

    type ArbGeometry =
        static member Angle() = Arb.fromGen angle
        static member Length() = Arb.fromGen length
        static member Direction2D() = Arb.fromGen direction2D
        static member Vector2D() = Arb.fromGen vector2D
        static member Point2D() = Arb.fromGen point2D
        static member Axis2D() = Arb.fromGen axis2D
        static member Line2D() = Arb.fromGen line2D
        static member LineSegment2D() = Arb.fromGen lineSegment2D
        static member BoundingBox2D() = Arb.fromGen boundingBox2D
        static member Frame2D() = Arb.fromGen frame2D

        static member Register() = Arb.register<ArbGeometry> () |> ignore
