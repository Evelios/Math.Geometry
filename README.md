# Math.Geometry

[![NuGet version](https://badge.fury.io/nu/Math.Geometry.svg)](https://badge.fury.io/nu/Math.Geometry)

`Math.Geometry` is a functional-first 2D spatial geometry library for F#, heavily based on the [elm-geometry](https://package.elm-lang.org/packages/ianmackenzie/elm-geometry/latest/) framework by Ian Mackenzie. It provides a rich API for working with geometric data types like points, vectors, arcs, polygons, and coordinate frames, all while maintaining strict type safety through the use of units and phantom types.

This library is built on top of [Math.Units](https://github.com/evelios/Math.Units) (a port of `elm-units`), ensuring that geometric operations are dimensionally consistent and unit-aware.

## :closed_book: [Documentation](https://evelios.github.io/Math.Geometry/index.html)

Full API documentation is available at [https://evelios.github.io/Math.Geometry/](https://evelios.github.io/Math.Geometry/).

## Features

- **Type-Safe Geometry**: Track units (meters, pixels, etc.) and coordinate systems at compile time.
- **2D Data Types**: Points, Vectors, Directions, Line Segments, Arcs, Circles, Ellipses, Polygons, and more.
- **Transformations**: Rotation, scaling, translation, and mirroring.
- **Coordinate System Conversions**: Easily move geometry between different reference frames.
- **Functional API**: Designed for piping and composition.

## Overview

`Math.Geometry` includes a wide variety of data types:

- **Vectors & Points**: The building blocks of geometry.
- **Directions**: Unit vectors representing orientation.
- **Line Segments, Triangles, Bounding Boxes**: Basic shapes and containers.
- **Polylines & Polygons**: Complex multi-vertex shapes.
- **Rectangles**: Axis-aligned or rotated rectangles.
- **Circles, Arcs, Ellipses**: Curved geometry.
- **Axes & Frames**: Define coordinate systems and reference points.
- **Intersection & Boolean Operations**: Find intersection points and perform clipping.

## Units and Coordinate Systems

Most types in `Math.Geometry` include two phantom type parameters: `'Units` and `'Coordinates`.

```fsharp
Point2D<Pixels, ScreenCoordinates>
```

- `'Units`: Tracks what units the geometry is in (e.g., `Meters`, `Pixels`, or `Unitless`).
- `'Coordinates`: Tracks which coordinate system the geometry is defined in.

This prevents mixing geometry from different contexts, such as trying to add a point in `ScreenCoordinates` to a vector in `WorldCoordinates`.

### Examples

#### Creating Points and Vectors

```fsharp
open Math.Units
open Math.Geometry

// Create a point at (2m, 4m)
let p1 = Point2D.meters 2.0 4.0

// Create a vector of 3 feet in the Y direction
let v1 = Vector2D.withLength (Length.feet 3.0) Direction2D.y

// Point/Vector arithmetic
let p2 = p1 + v1
```

#### Transformations

```fsharp
// Rotate a point 45 degrees around the origin
let rotatedPoint = 
    p1 |> Point2D.rotateAround Point2D.origin (Angle.degrees 45.0)

// Mirror a triangle across an axis
let mirroredTriangle =
    triangle |> Triangle2D.mirrorAcross axis
```

#### Coordinate Conversions

```fsharp
// Define a local frame
let localFrame = Frame2D.atPoint p1

// Take a global point and express it in local coordinates
let localPoint = p2 |> Point2D.relativeTo localFrame
```

## Development

To generate the API documentation you need to run the following commands.

```bash
dotnet tool install fsdocs-tool
dotnet fsdocs build --eval
dotnet fsdocs watch --eval
```

## Attribution

This project is a port of [elm-geometry](https://github.com/ianmackenzie/elm-geometry) by Ian Mackenzie. We are deeply grateful for his work and the work of all `elm-geometry` contributors.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
