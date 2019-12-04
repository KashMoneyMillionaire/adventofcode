open System

[<EntryPoint>]
printfn "Which day would you like to run?"
let dayToRun = Console.ReadLine()

match dayToRun with
            | "3" -> Day03.solve
            | "4" -> Day04.solve
            | _ -> printfn "Could not find day"

Console.Read()