namespace Utilities

module Ut = 
    let SplitLinesSplitOn splitBy = 
        System.IO.File.ReadLines("input.txt") |> List.map (fun x -> x.Split(splitBy));
