module Math.Geometry.Tests.Boolean2D

open NUnit.Framework
open FsCheck

open Math.Geometry.Test

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()
