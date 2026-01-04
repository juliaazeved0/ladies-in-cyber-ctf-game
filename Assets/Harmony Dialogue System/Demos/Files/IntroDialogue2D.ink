Hey #speaker:You
Hi #speaker:Red
How are you? #speaker:You
I'm good. #speaker:Red

-> main

=== main ===
What Did Blue tell you? #speaker:Red
    +[Nothing]
        Oh Ok #speaker:Red
        -> END

     +[We haven't Spoken]
        Oh Ok! #speaker:Red
        -> END
        
    +[Something]
        Tell... Wait I don't wanna know #speaker:red
        -> Ending

=== Ending ===
Alright #speaker:You
Ok. #speaker:red
-> END