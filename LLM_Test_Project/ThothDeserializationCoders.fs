namespace TestAPI

//Compiler-independent template suitable for Shared as well

//************************************************

//Compiler directives
#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

//**************** GET ********************

type HelloResponseGetThoth = 
    {
        Message : string
        Timestamp : string
    } 

//**************** POST ********************

type HelloResponsePostThoth = 
    {
        Message : string
    }  

//**************** PUT ********************

type UserPayloadThoth =
    {
        Id : int
        Name : string
        Email : string
    }

type UserResponsePutThoth = 
    {
        Message : string
        UpdatedDataTableInfo : UserPayloadThoth
    }

module ThothDeserializationCoders =   

    //**************** GET ********************
    
    let internal decoderGet : Decoder<HelloResponseGetThoth> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string
                          Timestamp = get.Required.Field "Timestamp" Decode.string
                      }
            )

    //**************** POST ********************
     
    let internal decoderPost : Decoder<HelloResponsePostThoth> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string                     
                      }
            )

    //**************** PUT ********************
        
    let internal decoderPut : Decoder<UserResponsePutThoth> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string
                          UpdatedDataTableInfo =
                              get.Required.Field "UpdatedDataTableInfo"
                                  (Decode.object
                                      (fun get1 ->
                                                 {
                                                     Id = get1.Required.Field "Id" Decode.int
                                                     Name = get1.Required.Field "Name" Decode.string
                                                     Email = get1.Required.Field "Email" Decode.string
                                                 }
                                      )
                                  )
                      }
            )