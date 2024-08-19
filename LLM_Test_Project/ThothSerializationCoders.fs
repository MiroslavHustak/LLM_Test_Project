namespace TestAPI

//Compiler-independent template suitable for Shared as well

//************************************************

//Compiler directives
#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

//************************* POST ****************************

type HelloPayload =
    {
        Name : string
    }                      
   
//************************* PUT ****************************

type UserPayload =
    {
        Id : int
        Name : string
        Email : string
    }

module ThothSerializationCoders =   

    //**************** POST ********************

    let internal encoderPost (result : HelloPayload) =
        Encode.object
            [
                "name", Encode.string result.Name
            ]    

    //**************** PUT ********************

    let internal encoderPut (result : UserPayload) =
        Encode.object
            [
                
                "id", Encode.int result.Id
                "name", Encode.string result.Name
                "email", Encode.string result.Email
            ]