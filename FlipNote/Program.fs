// Learn more about F# at http://fsharp.org

open FlipNote.ArgumentParser
open FlipNote.TemplateEngine

type Variables = Map<string,string>

let runTemplate (inputPath:string) (variables:VariableMap) (outputPath:string) =
    let outputPathKey = "OutputPath"
    let variables = 
        if variables.ContainsKey outputPathKey then
            variables
        else 
            variables.Add (outputPathKey, outputPath)
    let runtime = new Runtime(System.IO.File.ReadAllText(inputPath))
    runtime.Apply variables

let execute (input:ParsedInput) =
    let out = runTemplate input.InputPath input.Variables input.OutputPath
    do System.IO.File.WriteAllText(input.OutputPath,out)

[<EntryPoint>]
let main argv =
    let  input = parseArgument argv
    match input with
        | Ok input -> execute input
        | Error message -> printfn "%s" message 
    0 // return an integer exit code
