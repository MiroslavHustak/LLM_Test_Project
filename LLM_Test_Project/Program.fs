namespace LLM_API

open System
open FsHttp
open Newtonsoft.Json

open TestAPI

module API = 

    type Correction = 
        | AnswerIsNull
        | AnswerIsProbablyNotNull

    type OpenAIRequest = 
        {
            model : string
            prompt : string
            max_tokens : int
        }

    type ErrorMsg = 
        {
            message : string  
            ``type``: string
            param : obj
            code : string
        }

    type OpenAIErrorResponse =
        {
            error: ErrorMsg 
        }

    type Choice =
        {
            text : string
            index : int
            logprobs : obj option
            finish_reason : string
        }
    
    type Usage =
        {
            prompt_tokens: int
            completion_tokens: int
            total_tokens: int
        }
    
    type OpenAIResponse =
        {
            id : string
            ``object`` : string
            created : int64
            model : string
            choices : Choice[]
            usage : Usage
        }    

    type ResultOpenAI = 
        | ResultAIResponse of OpenAIResponse
        | ResultAIErrorResponse of OpenAIErrorResponse
                
    let generatePoem (topic : string) param =
         
            async
                {
                    try
                        //git push --set-upstream origin master
                        let url = "https://api.openai.com/v1/completions"

                        let requestPayload =
                            {
                                model = "gpt-3.5-turbo-instruct"  
                                prompt = sprintf "Write a poem about %s" topic
                                max_tokens = 100  // Limit the number of tokens
                            }

                        // POST request FsHttp
                        let! response = 
                            http 
                                {
                                    POST url
                                    AuthorizationBearer apiKey 
                                    body
                                    jsonSerialize requestPayload
                                }
                            |> Request.sendAsync
                    
                        match param with
                        | AnswerIsNull
                            -> 
                             let! responseContent = Response.deserializeJsonAsync<OpenAIErrorResponse> response
                             return Ok <| ResultAIErrorResponse responseContent 

                        | AnswerIsProbablyNotNull 
                            ->  
                             try                        
                                 let! responseContent = Response.deserializeJsonAsync<OpenAIResponse> response
                                 return Ok <| ResultAIResponse responseContent                        
                             with
                             | ex -> return Error (string ex.Message) 
                    with
                    | ex -> return Error (string ex.Message)
                }

    let topic = "the beauty of pure functional programming"

    let result = 
        generatePoem topic AnswerIsProbablyNotNull |> Async.RunSynchronously  
         
    match result with
    | Ok result -> 
                 match result with  
                 | ResultAIResponse result 
                     -> 
                      match result.id |> Option.ofObj with
                      | Some result -> 
                                     printfn "%A" result
                      | None        -> 
                                     let result = generatePoem topic AnswerIsNull |> Async.RunSynchronously
                                     printfn "%A" result

                  | ResultAIErrorResponse result 
                      -> 
                       printfn "%A" result

    | Error err -> 
                 printfn "%s" "Nothing worked :-("
                 printfn "%s" err

    
    CallRestApiWeather.runRestApiWeather ()

    Console.ReadKey() |> ignore

    //CallRestApi.runRestApi ()

    CallRestApiThoth.runRestApiThoth ()

    //Console.ReadKey() |> ignore
    
    //CallRpc.runRpc ()

    Console.ReadKey() |> ignore

    (*

        "{
        "error": {
            "message": "You exceeded your current quota, please check your plan and billing details. For more information on this error, read the docs: https://platform.openai.com/docs/guides/error-codes/api-errors.",
            "type": "insufficient_quota",
            "param": null,
            "code": "insufficient_quota"
        }
        }
        "
    *)