open System
open Fish.CommandPrompt


let tomp = "echo \"steveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteveteve\" " |> runCmd @"C:\"
match tomp with
| CommandOutput.Success outputs -> List.iter (fun (x : string) -> Console.WriteLine x) outputs
| CommandOutput.Failure message -> Console.WriteLine message

Console.ReadLine () |> ignore