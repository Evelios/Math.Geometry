module GeometryTests.Interval

open NUnit.Framework
open FsCheck.NUnit
open FsCheck

open Geometry

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()
