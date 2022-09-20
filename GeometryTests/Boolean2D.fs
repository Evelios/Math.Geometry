module Math.GeometryTests.Boolean2D

open NUnit.Framework
open FsCheck.NUnit
open FsCheck

open Math.Geometry

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()
