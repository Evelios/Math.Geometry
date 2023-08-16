module Math.Geometry.Tests.Frame2D

open NUnit.Framework
open FsCheck

open Math.Geometry.Test

[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()

[<Test>]
let ``Empty Test`` () = Assert.Pass()
