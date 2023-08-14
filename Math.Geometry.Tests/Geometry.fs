module Math.Geometry.Tests.Geometry

open NUnit.Framework
open FsCheck

open Math.Units.Test
open Math.Geometry
open Math.Geometry.Test


[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()


[<Test>]
let ``Geometry interface bunching`` () =
    let geometries : IGeometry list =
        [ Direction2D.x
          Vector2D.zero
          Point2D.origin
          Axis2D.x
        ]
        
        
    for geometry in geometries do
        match geometry with
        | :? Direction2D<obj> as _ -> Assert.Pass()
        | :? Vector2D<obj, obj> as _ -> Assert.Pass()
        | :? Point2D<obj, obj> as _ -> Assert.Pass()
        | :? Axis2D<obj, obj> as _ -> Assert.Pass()
        | _ -> Assert.Fail($"Could not determine geometry type:\n {geometry}")
        
    
    
