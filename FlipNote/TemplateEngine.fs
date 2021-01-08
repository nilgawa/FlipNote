module FlipNote.TemplateEngine

type Runtime =
    class
        val Input:string

        new(input:string) = { Input = input }


        

        member x.Apply (variables:Map<string,string>) :string =
            let converter (input :string) (key :string) (value :string) :string = input.Replace(key,value)
            Map.fold converter x.Input variables 
    end
