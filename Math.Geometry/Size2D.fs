[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Math.Geometry.Size2D

open Math.Units

// ---- Builders ----

let empty<'Units, 'Coordinates> : Size2D<'Units, 'Coordinates> =
    { Width = Quantity.zero
      Height = Quantity.zero }

let create<'Units, 'Coordinates> width height : Size2D<'Units, 'Coordinates> = { Width = width; Height = height }

// ---- Modifiers ----

let scale (scale: float) (size: Size2D<'Units, 'Coordinates>) : Size2D<'Units, 'Coordinates> =
    { Width = size.Width * scale
      Height = size.Height * scale }

let normalizeBelowOne (size: Size2D<'Units, 'Coordinates>) : Size2D<'Units, 'Coordinates> =
    scale (Quantity.create 1. / max size.Width size.Height) size

let withMaxSize<'Units, 'Coordinates>
    (maxSize: Length)
    (size: Size2D<'Units, 'Coordinates>)
    : Size2D<'Units, 'Coordinates> =
    size |> normalizeBelowOne |> scale maxSize.Value
