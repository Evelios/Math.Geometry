module Math.Geometry.Tests.Ellipse2D

open NUnit.Framework
open FsCheck.NUnit

open Math.Geometry
open Math.Geometry.Test


[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()

