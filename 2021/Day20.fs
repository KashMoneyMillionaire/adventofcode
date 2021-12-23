module Day20

open Utilities

let solve () =
    
    let convertPixel c =
        if c = '#' then 1 else 0
    
    let printPixel (matrix: _ list list) =
        
        let printItem i =
            let c = if i = 1 then "#" else "."
            printf $"%s{c}"
        
        let printRow r =
            r |> Seq.map printItem |> Seq.toList |> ignore
            printf "\n"
        
        matrix |> Seq.map printRow |> Seq.toList |> ignore    
        printf "\n"
        matrix

    let padImage image iter =
        let filler = iter % 2
        image
        |> Seq.map (fun line -> seq { filler; yield! line; filler; })
        |> Seq.transpose
        |> Seq.map (fun line -> seq { filler; yield! line; filler; })
        |> Seq.transpose
        |> seqToList
    
    let getPixel (algo: int[]) num =
        algo.[num]
    
    let algo, image =
        ReadInputLines "Day20" "input.txt"
        |> splitSeq ""
        |> pairMap (
            (fun s -> Seq.head s |> Seq.map convertPixel |> Seq.toArray),
            (fun s -> s |> Seq.map seq |> mapDeep convertPixel)
        )
    
    let parsePixel (image: int seq) =
        image
        |> Seq.toList
        |> Binary.parse
        |> getPixel algo
    
    let pixelOrDefault (img: int list list) iter (x,y) =
        if x < 0 || y < 0 then
            iter % 2
        else if x >= img.Length || y >= img[0].Length then
            iter % 2
        else
            img.[y].[x]
    
    let buildPixel img iter x y _ =
        let surrounding =
            seq {
                seq { (-1, -1); (0,-1); (1,-1) }
                seq { (-1, 0);  (0,0);  (1,0)  }
                seq { (-1, 1);  (0,1);  (1,1)  }
            }
            |> mapDeep (fun (l, r) -> (l + x, r + y))
            |> Seq.concat
            |> Seq.toList
        
        surrounding
        |> Seq.map (pixelOrDefault img iter)
        |> Seq.toList
        |> parsePixel        
        
    let enhance (img: int list list) iter =
        printf $"iteration: {iter}\n"
        let paddedImage = padImage img iter
        
        paddedImage
        |> matrixMapL (buildPixel paddedImage iter)
    
    let finalImage =
        (seqToList image, seq { 0 .. 49 })
        ||> Seq.fold enhance
//        |> printPixel
    
    let litCount =
        finalImage
        |> filterMatrixL (fun _ _ c -> c = 1)
        |> List.concat
        |> List.length
    
    printfn $"Part 1: {litCount}"
    printfn $"Part 2: "
