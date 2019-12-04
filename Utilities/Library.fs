namespace Utilities

module Ut = 
    let SplitLinesSplitOn (day: string) (splitBy: char) = 
        System.IO.File.ReadLines(day + "/input.txt") |> Seq.map (fun x -> x.Split(splitBy)) |> Seq.toList;
    
    let stringAsInt : string -> int = int

    let charsAsInt : char list -> int  = fun (chars: char list) -> 
        let str = new System.String(List.toArray(chars)) 
        stringAsInt(str)