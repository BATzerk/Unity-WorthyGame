
- FUNC_ShowEmbassy
- A: Prime Minister!\\nI have urgent news!
- P: Jeremy!\\n\\nWhat are you doing in the Embassy?
- P: I thought you were officiating Gabriel's wedding in Gladstone Park?
- A: I was, ma'am.\\n\\nUntil a UFO sucked up half the ceremony.
- P: Oh, God.\\n\\nNot AGAIN.
// - A: Yes.\\n\\nWhat shall we do??//The Aliens seem to enjoy picnics and weddings.");
- P: ...\\n\\nI know exactly who to call.
- ->IncomingCall


= IncomingCall
- FUNC_IncomingCallSeq
+ [Accept]
    -- ->AcceptCall
+ [Decline]
    -- FUNC_ShowEmbassy
    -- P: ...
    -- P: They rejected my call?\\n\\nLet me call back...
    -- ->IncomingCall


= AcceptCall
- FUNC_HideIncomingCallUI
- PM: [UserName]! It's me.

* [I told you not to call.]
    -- PM: I'm sorry, [UserName], but I have no other options.
// * [Katrín?]
//     -- PM: Yes.
//     ** [Jakobsdóttir?]
//         --- PM: Yes. Prime Minister of Iceland.
//     ** [Davíðsdóttir?]
//         --- PM: No, Katrín Jakobsdóttir.\\nHow many Katríns do you know?
//         *** [Just you]
//         *** [A few]
//         *** [Like a hundred]
//         --- PM: [UserName].
* [whooo dis new phone]
    -- PM: It's Katrín.
    ** [Jakobsdóttir?]
        --- PM: Yes. Prime Minister of Iceland.
    ** [Davíðsdóttir?]
        --- PM: No, Jakobsdóttir. How many Katríns do you know?
        *** [Just you]
        *** [A few]
        *** [Like a hundred]
        --- PM: [UserName].

- PM: We... I...
* [Yes?]
* [Spit it out, Katrín.]
- PM: Iceland needs your help.

* [Economy?]
    -- PM: Not quite.
    -- PM: Think more Alien/UFO.
    ** [Oh.]
* [Tourism?]
    -- PM: Not quite.
    -- PM: Think more Alien/UFO.
    ** [Oh.]
    // -- PM: Yes.
* [Aliens?]
    -- PM: I-- yes. How did you know?
    ** [I am Aliens]
        --- PM: Good.\\n\\nThen you will know how to handle these ones.
    ** [Grapevine]
        --- PM: Then you've also heard what must be done.
    ** [I'm kind of a god, Katrín]
        --- PM: Then you know what you must do.

- PM: We need you to shoot down a UFO.
// * [No.]
//     -- PM: Why not?
//     ** [My priority's on board]
//         --- PM: Then don't shoot to kill, [UserName].\\n\\nJust get it grounded.
//     ** [I'm not into violence]
//         --- PM: Then don't shoot to kill, [UserName].\\n\\nJust get it grounded.
//     ** [Kinda busy right now]
//         --- PM: [UserName]. Please.
* [Why?]
    -- PM: It's sucking up tons of our citizens.\\n\\nNo bueno.
* [Easy peasy lemon squeezy, baby!]
    -- PM: You are our champion, [UserName].


- PM: I'm sending you the coordinates now.
- FUNC_ShowShootDown

- FUNC_ShowEmbassy
- A: Threat neutralized, Madam Prime Minister!
- P: Yes, all thanks to [UserName].

- FUNC_CompleteMinigame

- ->END




