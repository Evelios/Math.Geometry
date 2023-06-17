module Math.Geometry.Tests.Boolean2D

open NUnit.Framework
open FsCheck.NUnit
open FsCheck

open Math.Geometry
open Math.Geometry.Test

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()
