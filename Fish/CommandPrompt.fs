namespace Fish

module CommandPrompt =
    open System
    open System.Text
    open System.Diagnostics
    open System.IO
    open System.Collections.Generic
    
    type CommandOutput =
        | Success of string list
        | Failure of string

    /// Allows multiple commands to be piped to a single instance of cmd.exe,
    /// similar to the way a sequence of commands can be run from a batch file.
    let runCmd startLocation (cmds : string list) =
        let errorString = new StringBuilder ()
        let outOut = new List<string> ()

        use p = new Process ()
 
        p.StartInfo.UseShellExecute <- false
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.RedirectStandardError <- true
        p.StartInfo.WorkingDirectory <- startLocation
        p.StartInfo.FileName <- Path.Combine (Environment.SystemDirectory, "cmd.exe")
        p.StartInfo.Arguments <- "/K @echo off"
    
        p.StartInfo.RedirectStandardInput <- true

        p.OutputDataReceived.Add (fun x -> outOut.Add x.Data)

        //TODOES make it figure out which command ruined everything, make it wait 

        p.ErrorDataReceived.Add (fun x -> errorString.Append x.Data |> ignore)
        
        p.Start () |> ignore
        p.BeginOutputReadLine ()
        p.BeginErrorReadLine ()
 
        for cmd in cmds do
            match errorString.Length with
            | 0 -> p.StandardInput.WriteLine cmd
            | _ -> ()

        p.StandardInput.WriteLine "exit"
        p.WaitForExit ()

        //TODOES isolate and remove command inputs here and organize the outputs by command

        match errorString.Length with
        | 0 -> CommandOutput.Success(List.ofSeq outOut)
        | _ -> CommandOutput.Failure(errorString.ToString ())