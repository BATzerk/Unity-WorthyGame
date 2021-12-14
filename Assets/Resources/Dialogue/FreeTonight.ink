
VAR outcome = 0.0

- Hey, [UserName]!! It’s [Cont0]!
- Are you free to hang out for <color=\#DD8800>5 hours</color> tonight?
    * [YES]
    -- ->YesTonight
    * [NO]
    -- Oh. Why not?
        ** [I HAVE PLANS]
            --- Oh, okay. Maybe later this week?
                *** [NO. NEVER.]
                    ---- ~outcome = -2
                    ---- Oh. Okay. Well.
                    ---- ...
                    ---- Hope you enjoy your night anyway.
                    ---- -> Done
                *** [YEAH, MAYBE]
                    ---- ~outcome = 0.5
                *** [DEFINITELY!]
                    ---- ~outcome = 1
            --- Cool! Does tomorrow work?
                *** [YEP]
                    ---- ->YesTomorrow
                *** [NOPE]
                    ---- ~outcome = -0.8
                    ---- Ahh. No worries.
                    ---- Okay, let's shoot for another week then.
        ** [I DON'T WANT TO]
            --- ~outcome = -1
            --- Oh. Okay.
            --- ...
            --- Hope you enjoy your night anyway.
            --- -> Done
    * [DEPENDS]
        --- C'mon, [UserName], it'll be fun!
            ** [OK, OK]
                ---- ->YesTonight
            // ** [WHAT'S IN IT FOR ME]
            //     ---- ->
            ** [SORRY...]
                ---- ~outcome = -0.5
                ---- Ah, ok.
                ---- No worries.
                ---- Enjoy your evening!
            ** [MAYBE TOMORROW?]
                ---- ->YesTomorrow
- ->Done


=== YesTonight ===
- ~outcome = 1
// - [SetCont0Winner]
- Yay!!
- What shall we do??
    * [DINNER]
    * [MOVIE]
    * [DINNER AND MOVIE]
    * [NEITHER DINNER NOR MOVIE]
- Great. Can't wait!
- I'll remind you tonight!//TODO: Theoretically, schedule local notification. "[Cont0]: Hey! It's [Cont0]! Still on to spend an hour with me tonight?"
- -> Done


=== YesTomorrow ===
- ~outcome = 0.8
- Yay!!
- What shall we do??
    * [DINNER]
    * [MOVIE]
    * [DINNER AND MOVIE]
    * [NEITHER DINNER NOR MOVIE]
- Great. Can't wait!
- I'll remind you tomorrow!//TODO: Theoretically, schedule local notification. "[Cont0]: Hey! It's [Cont0]! Still on to spend an hour with me tonight?"
- -> Done


=== Done ===
// - [ShowDoneBtn]
// - Outcome: {outcome}.
- ->END
