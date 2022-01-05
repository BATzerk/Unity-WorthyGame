
VAR wcS = "<color=\#BB57D0>" // worthyColorStart
VAR wcE = "</color>" // worthyColorEnd
VAR worth = "<color=\#BB57D0>worth</color>"
VAR Worth = "<color=\#BB57D0>Worth</color>"
VAR worthy = "<color=\#BB57D0>worthy</color>"
VAR Worthy = "<color=\#BB57D0>Worthy</color>"
VAR Jill = "<color=\#d9b212>Jill</color>"

- -> GameIntro


=== GameIntro ===
//*
- FuncContinue_SetBackgroundImage:YellowOrange
- N: Hi, [UserName]!\\nI'm Waddlesworth Worthington III.
- N: But you can call me {Jill}.
* [Hi, {Jill}.]
- N: This is not a video game.\\n\\nIt's a reminder:
- N: You.\\n\\nAre.\\n\\n{Worthy}.
* [Mmm... Idk, {Jill}.]
    -- N: Well, how worthy do you feel?
    ** [Less than somewhat.]
    ** [Somewhat.]
    ** [More than somewhat.]
    -- N: Ok, then follow up question:
* [Worthy of what?]
    -- N: You're {worthy} of TONS of exciting stuff!\\n\\nThat's what this game is all about!
- N: Do you sometimes feel not good enough?
* [Never.]
    -- N: You always feel good enough?
    ** [Yep, pretty much.]
        --- N: Beautiful!\\n\\nThen this should be a pretty cozy experience.
    ** [More or less.]
        --- N: Beautiful!\\n\\nThen this should be a pretty cozy experience.
    ** [I have no idea what I'm doing.]
        --- N: Neither do I!\\n\\nThat's exciting!
* [Of course.]
    -- N: That's totally normal!\\n\\nAnd also divorced from reality.
    -- N: Consider this game a splash in the face with a cool, refreshing bucket of reality.
    ** [Splash!!]
        --- N: 'At's the spirit, [UserName]!
    ** [Splash...]
        --- N: I do love your honesty.\\n\\nKeep that up!
* [What are you getting at, {Jill}?]
    -- N: I'm getting at <i>your essence</i>.\\nYour innate, sweet, <u>immutable</u> value.
    ** [Oh, boy...]
    ** [Oh, boy!]
    ** [Oh. Boy.]
    -- N: That's the spirit, [UserName]!
//*/


- N: No matter WHAT you do, you'll ALWAYS be {worthy}.
- FuncContinue_OpenMinigame_Painter
- FuncContinue_HideCharViewBody_N
- N: I'd like you to paint 2 MASTERPIECES to go in my studio.
* [Can do!]
    -- N: You da bomb, [UserName].
* [I'm not a painter...]
    -- FuncContinue_PlayVideo_DoesntMatter
    -- ShowTapToContinue(1.1)

- FuncContinue_HideCharViews
- FuncContinue_MinigameStepForward
- ShowTapToContinue(1.1)

- N: Try to make them as good as possible, and see how it affects your {wcS}PRAISEWORTHINESS{wcE}.
- FuncContinue_ShowWorthyMeter
- FuncContinue_SetWorthyMeterNoun:PRAISEWORTHY
- FuncContinue_SetWorthyMeterPercentFull:1
- ShowTapToContinue(1.2)
- N: Oh, look! You're already at 100%!\\n\\nI wonder if that can change?

- FuncContinue_HideCharViews
- FuncHalt_MinigameStepForward

- FuncContinue_MinigameStepForward
- N: What a masterpiece!\\nDo the other one now!

- FuncContinue_HideCharViews
- FuncHalt_MinigameStepForward

- FuncContinue_MinigameStepForward
- ShowTapToContinue(1.2)

- N: This is art.\\nThis is proper art.\\nAnd your 100% {worth} was never in question.


- ->END


=== PostPainter ===
// - FuncContinue_SetBackgroundImage:GarbageDump
- FuncContinue_ShowCharViewBody_N
- N: Let's see if we can LOWER your {wcS}praiseworthiness{wcE}.\\n\\nI want you to make two TERRIBLE paintings.
- N: Make 'em real bad.
- -> END



=== PostPainterTerrible ===
- N: Wow, those were some terrible paintings.\\n\\nPoo emojis were a nice touch.
- N: And your {wcS}praiseworthiness{wcE} is still at 100%.
- N: There is nothing you can do to change how {wcS}worthy you are of praise{wcE}.
- N: - END OF CONTENT FOR NOW -
- -> END














// - N: Hi, [UserName]! My name is Waddlesworth Worthington.
// - N: But you can call me Jill.
// * [Hi, Jill.]
// - N: I've made a bunch of MINIGAMES to make you {wcS}MORE WORTHY{wcE}!
// - N: What's something you want to be {wcS}worthy{wcE} of?
// * [Belonging]
// * [Acknowledgement]
// * [Forgiveness]
// * [Broomsticks]
// - N: You might not be worthy of it now, but you should be by the end of this game.

// - N: You’re actually the first person to play my minigames here.\\n\\nI’m excited to see how this goes!
- N: Here is your {wcS}WorthyMeter{wcE}!
- N: Your goal is to become {wcS}100% WORTHY{wcE}!
- N: What do you want to become {wcS}worthy{wcE} of?
* [Praise]
    -- StartMinigame_Praise
* [Self-love]
    -- StartMinigame_SelfLove
- -> END


/*
* [No, I have to pee first.]
    -- N: Then let’s both take a quick bathroom break.\\n\\nLet me know when you're back!
    ** [I'm back.]
        --- ->BackFromBathroom
    ** [I'm not back yet.]
        --- N: Okay. Take your time in there!
        *** [Keep stalling.]
            ---- N: Okay, let's go ahead and start with you still in the bathroom.
            ---- -> END
        *** [Okay, ready.]
            ---- ->StillNotReadyLoop

= StillNotReadyLoop
- N: Okay. Just let me know when you're ready.
+ [I'm back.]
    -- -> BackFromBathroom
+ [I'm still not ready.]
    -- ->StillNotReadyLoop


= BackFromBathroom
- N: Did you wash your hands?
* [Of course.]
    -- N: Empty bladder. Clean hands.\\n\\nLet's go!
    -- -> END
* [I don't remember.]
    -- N: You wanna go back and wash your hands?
    ** [No. I didn't actually go to the bathroom.]
        --- N: What were you doing, then?
        *** [Literally nothing.]
        *** [None of your beeswax, Jill.]
            ---- N: I see.\\n\\nWell... let's get started!
    ** [Yeah. Gimme one sec.]
        --- ->StillNotReadyLoop


--> END
*/



=== PostJokeTeller ===
- N: Ha ha! Those were some funny jokes!
- N: But it doesn't look as if telling jokes changed your deservingness of good friendships.
// - N: Maybe the WorthyMeter is broken? Can we check?
- N: Try saying something dumb to bring down your worthiness a little bit.
* [i'm a astronot]
* [I need a vacation... from MY VACATION!!!]
* [soup's up! no wait I mean surf's up oh no]
* [bimgo bamgo bonjo gingoo]
// * [I need to put my hands in your pockets. It's an emergency.]
- N: Hmm.
- N: I mean, that was a really dumb thing to say.
- N: ...But apparently you're still 100% worthy of good friendships.
- N: ...
- N: I have an idea! Go pick a friend up from the airport!\\nThat'll DEFINITELY make you more worthy.
- -> END







// - N: Hi, [UserName]! I'm Untitled Circle.
// - N: I know what you're thinking.
// * [Is that your birthname?]
//     -- Yes.
// * [Circles cannot ask questions.]
//     -- While that's correct, we can still make statements!
// * [What? You don't know me.]
//     -- But I'm looking forward to!

// = ExplainWhat
// - AutoAdvanceAfterSpeech
// - N: Well! We're gonna play minigames...
// - FuncContinue_ShowFinalRankSample
// - ShowTapToContinue(1.2)
// - AutoAdvanceAfterSpeech
// - N: Then, by the end, we'll have a <color=\#1F71DE>ranked list</color> of your life priorities today!
// - FuncContinue_RevealMoreFinalRankSample
// - ShowTapToContinue(2)
// - N: Hopefully these results will give you something to think about!
// - FuncContinue_HideFinalRankSample
// * [Why priorities?]
//     -- ->ExplainWhy
// * [NEXT]
//     -- ->END

// = ExplainWhy
// - N: The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.
// * [Got it.]
//     -- N: Awesome.
// * [What if they change?]
//     -- N: Don't worry, [UserName].\\n\\nClarity for today will be enough for today.
// - 
// * {numQsAnswered==1} [What do I do?]
//     -- ->ExplainWhat
// * {numQsAnswered>1} [NEXT]
//     -- ->END








=== GeneralFeelingsQuery ===
VAR comfort=0
VAR guilt=0
VAR unfairness=0
- N: Mind if I ask a few Q's about how you feel?
    * [SURE]
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Comfort:
    * [HIGH]
        -- ~comfort=2
    * [MEDIUM]
        -- ~comfort=1
    * [LOW]
        -- ~comfort=0
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Guilt:
    * [HIGH]
        -- ~guilt=2
    * [MEDIUM]
        -- ~guilt=1
    * [LOW]
        -- ~guilt=0
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Unfairness:
    * [HIGH]
        -- ~unfairness=2
    * [MEDIUM]
        -- ~unfairness=1
    * [LOW]
        -- ~unfairness=0
- N: Comfort: {comfort}. Guilt: {guilt}. Unfairness: {unfairness}.
- -> END




/*
- -> GameIntro
=== GameIntro ===
VAR numQsAnswered = 0



- N: Hi, [UserName]! I'm Zorro.\\nPrepare to become worthy!
- N: We're gonna play some minigames to make you worthy of things.
* [Worthy of what?]
    -- ->ExplainWhat
* [Makes complete sense]
    -- ->ExplainWhy

= ExplainWhat
~ numQsAnswered++
// - N: Sure, let me explain this game:
- AutoAdvanceAfterSpeech
- N: Well! We're gonna play minigames...
- FuncContinue_ShowFinalRankSample
- ShowTapToContinue(1.2)
- AutoAdvanceAfterSpeech
- N: Then, by the end, we'll have a <color=\#1F71DE>ranked list</color> of your life priorities today!
- FuncContinue_RevealMoreFinalRankSample
- ShowTapToContinue(2)
- N: Hopefully these results will give you something to think about!
- FuncContinue_HideFinalRankSample
* {numQsAnswered==1} [Why priorities?]
    -- ->ExplainWhy
* {numQsAnswered>1} [NEXT]
    -- ->END

= ExplainWhy
- ~numQsAnswered++
- N: The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.
* [Got it.]
    -- N: Awesome.
* [What if they change?]
    -- N: Don't worry, [UserName].\\n\\nClarity for today will be enough for today.
- 
* {numQsAnswered==1} [What do I do?]
    -- ->ExplainWhat
* {numQsAnswered>1} [NEXT]
    -- ->END

*/






