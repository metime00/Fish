open System
open Fish.CommandPrompt


let tomp = [ "echo \"steve\" "; "dir /b"; "cd Windows" ] |> runCmd @"C:\"
match tomp with
| CommandOutput.Success outputs -> List.iter (printfn "%s") outputs
| CommandOutput.Failure message -> Console.WriteLine message

Console.ReadLine () |> ignore