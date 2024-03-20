EXTERNAL changeCurrency(amount, index)
EXTERNAL changePersonalObjective(amount, index)
EXTERNAL changeScore (amonut, index)
EXTERNAL changeKills (amount, index)

EXTERNAL PlayEventAudio(index)

-> paragraph_1

=== paragraph_1===
A group of spirits looks to be in a heated argument in the middle of the road. 

You slow down the carriage and ask them what the matter seems to be. The spirits tell you that they’re in a  debate trying to decide who is the scariest of them. Seeing as you guys should be unbiased, the spirits now want you to be the jury!

Who do you pick?

* [The spirit wearing a spooky mask] -> spooky_mask
* [The spirit looking for long term commitment] -> long_term_commitment
* [The "spirit" in a gorilla costume] -> pick_gorilla
* [Show them who the scariest spirits really are!] -> show_them

===spooky_mask===
~ changeScore (-20,0)
~ changeScore (-20,1)
~ changeScore (-20,2)
~ changeScore (-20,3)


You tell them that the mask clad spirit is the scariest one! The spirit wearing the spooky mask looks very happy with your decision! You think he does at least, it's hard to tell with the mask after all. The rest of the spirits, however, seem to be less enthused with your decision.

“Any old spirit could wear a spooky mask! And isn't he kind of trying too hard?” The rest of the spirits start to argue once more, seemingly not content with your decision. 

Welp, that could have gone better!

You all earn <b><color=\#8F00FF>+1 reputation of being a horrible decision makers,</color></b> which equals about <b><color=\#e06666>-20 total score</color></b>

Better luck next time! -> END

===long_term_commitment===
You tell them that the scariest spirit by far is the spirit looking for long term commitment. The spirit in question looks a little sad about it but the rest of them seem to actually agree with you.

“That's true, most spirits just aint ready for that yet.” One of the spirits says, whilst patting the relationship ready spirit on the back.

You tell the, now sad, spirit to keep her head up and that she will find the right ghoul for her! She thanks you for your kind words.

You all gain <b><color=\#fc8ff8>+the knowledge that you did something right,</color></b> which also grants each one of you <b><color=\#ffd966>+20 total score! </color></b>

Aw! And you guys call yourselves monsters!
 -> END


===pick_gorilla===
~ changePersonalObjective (-5, 2)
You tell them that the spirit in the gorilla costume is the scariest one by far. The spirits all look confusedly at you, seemingly not understanding what you mean.

“Gorilla costume-?” The spirit has just the time to finish his sentence before he gets gruesomely mauled by the, very real, gorilla. 

Too stunned to act, you all stand helplessly by as the gorilla brutally murders all of the spirits. Once happy with his ghastly murder the gorilla seems to be looking at you next!

Through the power of colleagues you all manage to fend off the gorilla long enough for all of you to once more enter the carriage. As soon as you are all inside you yell at the armadillo to quickly run away. 

The gorilla makes an attempt to follow, but is simply no match for the speed of a giant armadillo!

Turns out the gorilla really was the scariest spirit, so scary in fact that 5 souls residing in <color=\#ac70fe>Violet</color> seems to have fled her body to escape the gorilla

<color=\#ac70fe>Violet</color> loses <b><color=\#ac70fe>-5 souls</color></b> and you all gain <b><color=\#fc8ff8>+1 angry gorilla on your tail!</color></b>

Exciting! -> END

===show_them===
~ changePersonalObjective (5, 3)
These losers? Fuck no! The scariest spirits are <i>clearly you guys!</i> You decide to prove it the only way you know how; by running the group of spirits over.

The ride is loud and bumpy, each ‘thump’ paired with a new spirit’s agonizing screams as they are painfully crushed beneath the weight of the giant armadillo and carriage. 

Once the screams have quieted down, you all look out through the carriage's back window. Where the group of spirits once stood, now lies only a pile of gold, spirit dust, and a little blood?.

Yucky!

Your murderous actions reward you each with the title of  <b><color=\#fc8ff8>“The Scariest Spirits”.</color></b> Rad!

Although <color=\#96d87d>Jade</color> is usually one to break statues, it seems in this case breaking bones was just as rewarding! <color=\#96d87d>Jade</color> gets <b><color=\#96d87d>+5 shatter!</color></b>
 -> END

