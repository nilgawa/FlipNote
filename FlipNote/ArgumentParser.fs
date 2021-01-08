module FlipNote.ArgumentParser

open Argu

type CommandArguments =
    | [<Mandatory>][<AltCommandLine("-i")>] In of input_path:string
    | [<Mandatory>][<AltCommandLine("-o")>] Out of output_path:string
    | [<AltCommandLine("-v")>] Variables of string list

    interface IArgParserTemplate with
        member s.Usage = 
            match s with
            | In _ -> "Input Template File Path."
            | Out _ -> "Output File Path."
            | Variables _ -> "Variables used in the template."

type VariableMap = Map<string,string>

type ParsedInput =
    struct
        val InputPath:string
        val OutputPath:string
        val Variables:VariableMap
        new(inputPath:string, outputPath:string, variables:VariableMap) = { InputPath = inputPath; OutputPath = outputPath; Variables = variables}
    end

let parseVariable (text:string) = 
    let splitted = text.Split('=')
    if splitted.Length <> 2 then 
        failwith "format is XXX=YYY."
    (splitted.[0],splitted.[1])
   
let parseArgument argv =
    try
        let parser = ArgumentParser.Create<CommandArguments>("flipnote")
        let parseResults = parser.Parse(argv)
        let inputPath= parseResults.GetResult In
        let outputPath = parseResults.GetResult Out
        let variables = parseResults.PostProcessResult(<@ Variables @>,List.map(parseVariable) >> Map.ofList )
        Ok <| new ParsedInput(inputPath,outputPath,variables)
    with
        | _ as message -> Error <|  message.ToString()