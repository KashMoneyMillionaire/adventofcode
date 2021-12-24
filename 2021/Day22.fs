module Day22

open Utilities

type Cube = bool[,,]

type Bound = (int * int) * (int * int) * (int * int) 

type Instruction = {
    Dir: string
    Bounds: Bound
}

let solve () =

    let top = 101
    
    let parseInput =
        function
        | ParseRegex "(.+) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)"
                     [ String dir; Integer x1; Integer x2; Integer y1; Integer y2; Integer z1; Integer z2 ] ->
            {Dir = dir; Bounds = ((x1, x2), (y1, y2), (z1, z2))}
        | _ -> failwith "bad input"

    let clamp l r =
        if l < 0 && r > 101 then (0, 101)
        else if r < 0 then (-1, -1)
        else if l > 101 then (-1, -1)
        else if l >= 0 && r <= 101 then (l, r)
        else (max 0 l, min 101 r)
    
    let clamp ((minX, maxX), (minY, maxY), (minZ, maxZ)) =
        (clamp minX maxX, clamp minY maxY, clamp minZ maxZ)
    
    let hasValue (l,r) =
        l <> -1 && r <> -1
    
    let combineCube (cube: Cube) (instr: Instruction) =
        let { Dir = dir; Bounds = bounds } = instr
        let x, y, z = clamp bounds
        
        if hasValue x && hasValue y && hasValue z then
            seq { fst x .. snd x } >< seq { fst y .. snd y } >|< seq { fst z .. snd z }
            |> Seq.filter (fun (x,y,z) -> 0 <= x && x <= top && 0 <= y && y <= top && 0 <= z && z <= top)
            |> Seq.iter (fun (x,y,z) -> Array3D.set cube x y z (dir = "on"))
    
    let adjustPositive b instr =
        let { Dir = dir; Bounds = bounds } = instr
        let (minX, maxX), (minY, maxY), (minZ, maxZ) = bounds
        { Dir = dir; Bounds = ((minX+b, maxX+b), (minY+b, maxY+b), (minZ+b, maxZ+b)) }
        
    let cube = Array3D.create top top top  (false)

    ReadInputLines "Day22" "input.txt"
    |> Seq.map parseInput
    |> Seq.map (adjustPositive 50)
    |> Seq.iter (combineCube cube)
    
    let onCount = cube |> sumBy (fun c -> if c then 1 else 0)    
    
    printfn $"Part 1: {onCount}"
    printfn $"Part 2: "
