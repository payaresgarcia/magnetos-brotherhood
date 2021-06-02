# magnetos-brotherhood
Brotherhood of Mutans

Magneto is recruiting all possible mutants to be part of his brotherhood. If you want to be part of it, you must prove that you are worthy by being a mutant.

Api link: https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/

Api link with Swagger: https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/swagger/index.html

Instructions for using the api from any client:
1. Copy the previous url (not swagger url)

2. Using your favorite http request client or if you are working on any client app
  
  2.1. System endpoint
    This allows you to know if you are able to hit the api and if its up and running.  You can use next.
    
    2.1.1. "/api/v1/system"
      You can hit this service by using "GET" request and it returns a message with the version of the api.
      e.g. https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/api/v1/system
           Response: 200 -> "Version 1.0"
    
    2.1.2. "/api/v1/system/info"
      You can hit this service by using "POST" request and passing a json body with the 'applicantName' property, this returns a nice message.
      e.g. https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/api/v1/system/info
           Body Request: {"applicantName": "your name here"}
           Response: 200 -> "I'm already watching you <your name here>, but we need to test you dna first!"
      
  2.2. Mutants endpoint
    This allows you enter dna sequences and know if they belong to mutants or humans.  You can also get statistics from this data.
    
    2.2.1 "/api/v1/mutants/mutant"
      You can hit this service by using "POST" request and passing a json body with the "dna" property, which is an array of strings.  This strings should be valid dna sequences; this sequences should be formed by letters 'A', 'C', 'T', 'G' from the dna hydrogen base; and it should build an NXN matrix where the number of letters in a sequence should be the same number of sequences.  This returns an http code status.
      e.g. https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/api/v1/mutants/mutant
           Body Request: {"dna":["ATGCGA","CAGTGC","TTATGT","AGAAGG","CCCATA","TCACTG"]}
           You can see there are six sequences and each sequence has six letters (from the hydrogen base)
           Response:
            If mutant:200 (Ok)
            If human: 403 (Forbidden)
           
    2.2.2 "/api/v1/mutants/stats"
      You can hit this service by using "GET" request and it returns statistics from mutants, humans, and ratio.  This returns a json with statistics.
      e.g. https://2aeqd0zcz6.execute-api.us-east-2.amazonaws.com/dev/api/v1/mutants/stats
           Response: 200 -> { "count_mutant_dna": 3, "count_human_dna": 3, "ratio": 1.0 }
