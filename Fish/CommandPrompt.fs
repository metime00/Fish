namespace Fish

module CommandPrompt =
    open System
    open System.Diagnostics
    open System.IO
 
    /// Allows multiple commands to be piped to a single instance of cmd.exe,
    /// similar to the way a sequence of commands can be run from a batch file.
    let runCmd startLocation (cmds : string list) =
        let errerOut = new Collections.Generic.List<string> ()
        let outOut = new Collections.Generic.List<string> ()

        use p = new Process ()
 
        p.StartInfo.UseShellExecute <- false
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.RedirectStandardError <- true
        p.StartInfo.WorkingDirectory <- startLocation
        p.StartInfo.FileName <- Path.Combine (Environment.SystemDirectory, "cmd.exe")
    
        p.StartInfo.RedirectStandardInput <- true
        
        p.OutputDataReceived.Add (fun x -> outOut.Add x.Data)
        p.ErrorDataReceived.Add (fun x -> errerOut.Add x.Data)
        p.Start () |> ignore
        p.BeginOutputReadLine ()
        p.BeginErrorReadLine ()
 
        cmds |> List.iter p.StandardInput.WriteLine

        p.StandardInput.WriteLine "exit"
        p.WaitForExit ()
        (List.ofSeq outOut, List.ofSeq errerOut)