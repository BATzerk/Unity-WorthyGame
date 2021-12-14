VAR roseStr = "<color=\#FF00AA>rose</color>"
VAR rosesStr = "<color=\#FF00AA>roses</color>"


- FuncHalt_Anim_Intro
* [NEXT]
- FuncHalt_Anim_IntroContViews


* [HELLO]
- User: Hello, priorities. It's [UserName].

* [<i>GENTLE</i>]
    -- User: <i>(deep sigh)</i>\\nI... You're all here for a reason.
* [<i>TOUGH</i>]
    -- User: Prepare to get WRECKED by LOVE.\\nIf you're NOT picked, you can take a HIKE.
- 

* [<i>GENTLE</i>]
    -- User: I care about all of you...\\nBut I have to follow my heart.\\n\\nHaving to say goodbye to two of you tonight is... really tough.
* [<i>TOUGH</i>]
    -- User: NOBODY $*%!s with [UserName].\\nNOBODY.\\n...And don't you THINK about touching my car.
- 

* [ROSES]
- User: If I give you a {roseStr}, I want you to stay.
- FuncHalt_Anim_ShowRoses
- AutoAdvanceAfterSpeech
- User: My first {roseStr} goes to...
- FuncHalt_SetContViewsInteractableTrue

- User: [LastPickedCont], I choose you. Because...
* [ROMANTIC]
    -- User: You're my treasure.\\nYou're my rock.\\nYou are the lighthouse in my heart.
* [DIPLOMATIC]
    -- User: If I could give you an award, I would.\\nBut I do not have any awards. I only have {rosesStr} right now.
    // -- \\nPlease accept this {roseStr}.
- 

* [SECOND ROSE]
- AutoAdvanceAfterSpeech
- User: Next, I pick...
- FuncHalt_SetContViewsInteractableTrue

- User: [LastPickedCont], I choose you.\\nI...
* [ROMANTIC]
    -- User: I want to go to San Antonio and play ukulele with you.\\nThis is, we can do this someday.
* [DIPLOMATIC]
    -- User: We can visit San Antonio, and we can play ukulele together.\\nThis is, we can do this someday.
- User: Only one {roseStr} left.\\n\\nI...

* [GENTLE]
    -- User: [ContUnchosen0]. [ContUnchosen1]. [ContUnchosen2].\\nChoosing between you is... I'm...
    ** [DON'T CRY]
        --- User: I am strong.\\nLike a big, strong bus filled with strong people.
    ** [PRETEND TO CRY]
        --- User: ::sob...sob::\\n::cough::\\n...\\nAhem.
        *** [WIPE FALSE TEARS]
            ---- N: ::wipes tears::
    ** [LEGIT CRY]
        --- User: ::sob...sob::\\n::cough, sputter::\\nAhem.
        *** [WIPE TEARS]
            ---- N: ::wipes tears::
* [TOUGH]
    -- User: [ContUnchosen0]. [ContUnchosen1]. [ContUnchosen2].\\nYou're a bunch of baguettes on the side of the road.\\n\\nGet bent.
-

* [FINAL ROSE]
- AutoAdvanceAfterSpeech
- User: I've made my decision.\\nI pick...
- FuncHalt_SetContViewsInteractableTrue


- User: [LastPickedCont]. Please accept this {roseStr}.
* [NEXT]
- User: [ContUnchosen0] and [ContUnchosen1]...

* [GENTLE]
    -- User: I love you. But I'm not <i>in</i> love with you.\\nI'll see you two around.
* [TOUGH]
    -- User: As far as I'm concerned, you can fall off the earth.\\nThat's a promise.
-

* [END]
- ->DONE



// - User: Before we start, I just want to say...
    // -- Each of you is here because you matter. To me.
    // -- I picked you all for a reason.
    
    // -- Two of you are goin' HOME today.
// * [<i>SOFT</i>]
//     -- Each of you is here because you matter to me.
// * [<i>HARD</i>]
//     -- You might matter. You might NOT.
// - 

    // -- I ain't PLAYIN' with you.
    // -- This is REAL LIFE.
    // -- This is ANYone's game.

    
    