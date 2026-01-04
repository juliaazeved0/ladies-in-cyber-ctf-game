Hey #speaker:You
Hi #speaker:Ebuka
How are you? #speaker:You
I'm good. #speaker:Ebuka
Have you checked out Jeffawe's Assets? #speaker:Ose
Jeffawe. Heard of him before. #speaker:You
Yeah. He makes Assets in Unity and Unreal. #speaker:Ebuka
I think he just released his first some time ago right? #speaker:Ose
Yeah. A dialogue System. #speaker:Ebuka
-> main

=== main ===
Nice. Have you tried it out before? #speaker:Ose
    +[Yes]
        -> Question

     +[No]
        Oh you definitely should. Check out his Asset Page on Unity. Jeffawe! #speaker:Ose
        -> Ending

=== Question ===
        Nice. Did you enjoy it? #speaker:Ose
        +[Yes] 
            -> Ending2
            
        +[No]
            Oh! You should tell him. Drop a comment on the Asset Page #speaker:Ose
            -> Ending

=== Ending ===
Alright I will #speaker:You
Ok. #speaker:Ose
Alright then. Bye #speaker:You
Bye #speaker:Ebuka
Bye #speaker:Ebuka
-> END

=== Ending2 ===
    Lovely #speaker:Ose
    -> END