module Math.GeometryTests.Ellipse2D

open NUnit.Framework
open FsCheck.NUnit

open Math.Geometry


[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()

