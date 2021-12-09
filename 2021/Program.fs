module AdventOfCode._2021

open System

let rec prompt forever =
    printfn "Which day would you like to run?"
    
    match Console.ReadLine() with
                | "1" -> Day01.solve()
                | "2" -> Day02.solve()
                | "3" -> Day03.solve()
                | "6" -> Day06.solve()
                | _ -> printfn "Could not find day"

    prompt forever
   
[<EntryPoint>] 
prompt()