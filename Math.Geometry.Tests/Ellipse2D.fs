module Math.Geometry.Tests.Ellipse2D

open NUnit.Framework

open Math.Geometry.Test


[<SetUp>]
let Setup () = Gen.ArbGeometry.Register()
